﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Harvester.Core.Messaging;

/* Copyright (c) 2012-2013 CBaxter
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
 * IN THE SOFTWARE. 
 */

namespace Harvester.Core.Filters
{
    public abstract class FilterBase : ICreateFilterExpressions
    {
        private readonly IEnumerable<ICreateFilterExpressions> children;
        private readonly IHaveExtendedProperties extendedProperties;
        private readonly HashSet<Type> supportedTypes;
        
        public abstract String FriendlyName { get; }
        public abstract Boolean CompositeExpression { get; }
        public IEnumerable<ICreateFilterExpressions> Children { get { return children; } }
        public IHaveExtendedProperties ExtendedProperties { get { return extendedProperties; } }
        protected virtual IEnumerable<Type> UnsupportedTypes { get { return Types.Empty; } }
        protected virtual IEnumerable<Type> SupportedTypes { get { return Types.BuiltIn; } }

        protected FilterBase(IHaveExtendedProperties extendedProperties, IEnumerable<ICreateFilterExpressions> children)
        {
            Verify.NotNull(extendedProperties, "extendedProperties");

            this.supportedTypes = GetSupportedTypes();
            this.extendedProperties = extendedProperties;
            this.children = children ?? Enumerable.Empty<ICreateFilterExpressions>();
        }

        public Expression CreateExpression(FilterParameters parameters)
        {
            Verify.NotNull(parameters, "parameters");

            return BuildExpression(parameters);
        }

        public Boolean IsTypeSupported(Type type)
        {
            return supportedTypes.Contains(type);
        }

        private HashSet<Type> GetSupportedTypes()
        {
            return new HashSet<Type>(SupportedTypes.Except(UnsupportedTypes));
        }
        
        protected abstract Expression BuildExpression(FilterParameters parameters);
    }
}
