﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FilterActivator = System.Func<Harvester.Core.Messaging.IHaveExtendedProperties, Harvester.Core.Filters.ICreateFilterExpressions>;

namespace Harvester.Core.Filters
{
    public class DynamicFilterExpression : IFilterMessages
    {
        private static readonly IEnumerable<ICreateFilterExpressions> NoChildren = Enumerable.Empty<ICreateFilterExpressions>();
        private static readonly IEnumerable<FilterDefinition> EmptyDefinitions = Enumerable.Empty<FilterDefinition>();
        private static readonly IEnumerable<String> EmptyApplications = Enumerable.Empty<String>();
        private static readonly IEnumerable<Int32> EmptyProcesses = Enumerable.Empty<Int32>();
        private static readonly IDictionary<String, FilterActivator> FilterActivators;
        private static readonly HashSet<String> Properties;

        private readonly ParameterExpression systemEvent = Expression.Parameter(typeof(SystemEvent), "e");
        private readonly FilterParameters filterParameters;
        private volatile IFilterMessages dynamicFilter;
        private readonly IFilterMessages staticFilter;

        private IEnumerable<FilterDefinition> textFilters;
        private IEnumerable<String> applicationFilters;
        private IEnumerable<Int32> processFilters;

        public SystemEventLevel LevelFilter { get; set; }
        public IEnumerable<Int32> ProcessFilters { get { return processFilters ?? EmptyProcesses; } set { processFilters = value ?? EmptyProcesses; } }
        public IEnumerable<String> ApplicationFilters { get { return applicationFilters ?? EmptyApplications; } set { applicationFilters = value ?? EmptyApplications; } }
        public IEnumerable<FilterDefinition> TextFilters { get { return textFilters ?? EmptyDefinitions; } set { textFilters = value ?? EmptyDefinitions; } }
        private IEnumerable<FilterDefinition> UsernameFilters { get { return TextFilters.Where(definition => FiltersOnProperty(definition, "Username")); } }
        private IEnumerable<FilterDefinition> MessageFilters { get { return TextFilters.Where(definition => FiltersOnProperty(definition, "Message")); } }
        private IEnumerable<FilterDefinition> SourceFilters { get { return TextFilters.Where(definition => FiltersOnProperty(definition, "Source")); } }

        static DynamicFilterExpression()
        {
            Properties = new HashSet<String>(typeof(SystemEvent).GetProperties().Select(property => property.Name));
            FilterActivators = CoreAssembly.Reference
                                           .GetTypes()
                                           .Where(type => !type.IsAbstract && type.IsClass && typeof(ICreateFilterExpressions).IsAssignableFrom(type))
                                           .Select(type => (ICreateFilterExpressions)Activator.CreateInstance(type, FilterDefinition.Empty, NoChildren))
                                           .Where(filter => !filter.CompositeExpression)
                                           .ToDictionary(filter => filter.FriendlyName, filter => CreateFilterActivator(filter.GetType()));
        }

        public DynamicFilterExpression(IFilterMessages filter)
        {
            Verify.NotNull(filter, "filter");

            staticFilter = filter;
            systemEvent = Expression.Parameter(typeof(SystemEvent), "e");
            filterParameters = new FilterParameters { systemEvent };
        }

        private static FilterActivator CreateFilterActivator(Type type)
        {
            return extendedProperties => (ICreateFilterExpressions)Activator.CreateInstance(type, extendedProperties, NoChildren);
        }
        
        private static Boolean FiltersOnProperty(FilterDefinition definition, String propertyName)
        {
            return String.Compare(propertyName, definition.Property, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public IEnumerable<String> GetSupportedFilterNames(Type type)
        {
            Verify.NotNull(type, "type");

            return FilterActivators.Values
                                   .Select(activator => activator.Invoke(FilterDefinition.Empty))
                                   .Where(filter => !filter.CompositeExpression && filter.IsTypeSupported(type))
                                   .Select(filter => filter.FriendlyName)
                                   .ToList();
        }

        public Boolean HasProperty(String propertyName)
        {
            Verify.NotWhitespace(propertyName, "propertyName");

            return Properties.Contains(propertyName);
        }

        public Boolean Exclude(SystemEvent e)
        {
            return e == null || staticFilter.Exclude(e) || dynamicFilter != null && dynamicFilter.Exclude(e);
        }

        public void Update()
        {
            var filters = new List<ICreateFilterExpressions>();

            if (LevelFilter > SystemEventLevel.Trace)
                filters.Add(CreateGreaterThanOrEqualToFilter("Level", LevelFilter));

            if (ProcessFilters.Any())
                filters.Add(new OrElseFilter(FilterDefinition.Empty, ProcessFilters.Select(pid => CreateEqualToFilter("ProcessId", pid))));

            if (ApplicationFilters.Any())
                filters.Add(new OrElseFilter(FilterDefinition.Empty, ApplicationFilters.Select(name => CreateEqualToFilter("ProcessName", name))));

            if (SourceFilters.Any())
                filters.Add(new OrElseFilter(FilterDefinition.Empty, SourceFilters.Select(CreateTextFilter)));

            if (UsernameFilters.Any())
                filters.Add(new OrElseFilter(FilterDefinition.Empty, UsernameFilters.Select(CreateTextFilter)));

            if (MessageFilters.Any())
                filters.Add(new OrElseFilter(FilterDefinition.Empty, MessageFilters.Select(CreateTextFilter)));

            dynamicFilter = new StaticFilterExpression(systemEvent, new AndAlsoFilter(FilterDefinition.Empty, filters).CreateExpression(filterParameters));
        }

        private static ICreateFilterExpressions CreateGreaterThanOrEqualToFilter(String propertyName, Object value)
        {
            return new GreaterThanOrEqualFilter(FilterDefinition.Create(propertyName, value), NoChildren);
        }

        private static ICreateFilterExpressions CreateEqualToFilter(String propertyName, Object value)
        {
            return new EqualFilter(FilterDefinition.Create(propertyName, value), NoChildren);
        }

        private static ICreateFilterExpressions CreateTextFilter(FilterDefinition definition)
        {
            FilterActivator activator;
            if (!FilterActivators.TryGetValue(definition.FriendlyName, out activator))
                throw new ArgumentException(String.Format(Localization.FilterTypeNotKnown, definition.FriendlyName), "definition");

            var expressionBuilder = activator.Invoke(definition);
            if (definition.Negate)
                expressionBuilder = new NotFilter(FilterDefinition.Empty, new[] { expressionBuilder });

            return expressionBuilder;
        }
    }
}
