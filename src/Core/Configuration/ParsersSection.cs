﻿using System;
using System.Configuration;

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

namespace Harvester.Core.Configuration
{
    public class ParsersSection : ConfigurationSection
    {
        [ConfigurationProperty(null, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ParserElementCollection Parsers { get { return (ParserElementCollection)base[""] ?? new ParserElementCollection(); } }
    }

    [ConfigurationCollection(typeof(ParserElement), AddItemName = "parser", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ParserElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ParserElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            var parser = (ParserElement) element;

            return String.IsNullOrWhiteSpace(parser.Name) ? parser.TypeName : parser.Name;
        }
    }

    public class ParserElement : ExtendableConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public String TypeName { get { return (String)base["type"]; } }
        
        [ConfigurationProperty("name", IsRequired = false, DefaultValue = "")]
        public String Name { get { return (String)base["name"]; } }
    }
}
