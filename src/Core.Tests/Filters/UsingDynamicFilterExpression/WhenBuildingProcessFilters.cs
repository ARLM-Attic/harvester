﻿using System;
using Harvester.Core.Filters;
using Xunit;
using Xunit.Extensions;

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

namespace Harvester.Core.Tests.Filters.UsingDynamicFilterExpression
{
    public class WhenBuildingProcessFilters : WithStaticFilterDefinition
    {
        [Fact]
        public void NeverReturnNull()
        {
            var dynamicFilter = new DynamicFilterExpression(StaticFilter) { ProcessFilters = null };

            Assert.NotNull(dynamicFilter.ProcessFilters);
        }

        [Theory, InlineData(1), InlineData(3), InlineData(5)]
        public void ExcludeIfEventProcessIdNotEqualToAnyProcessFilter(Int32 processId)
        {
            var dynamicFilter = new DynamicFilterExpression(StaticFilter) { ProcessFilters = new[] { 2, 4, 8, 16 } };
            dynamicFilter.Update();

            Assert.True(dynamicFilter.Exclude(new SystemEvent { ProcessId = processId }));
        }

        [Theory, InlineData(2), InlineData(4), InlineData(16)]
        public void IncludeIfEventProcessIdEqualToAnyProcessFilter(Int32 processId)
        {
            var dynamicFilter = new DynamicFilterExpression(StaticFilter) { ProcessFilters = new[] { 2, 4, 8, 16 } };
            dynamicFilter.Update();

            Assert.False(dynamicFilter.Exclude(new SystemEvent { ProcessId = processId }));
        }
    }
}
