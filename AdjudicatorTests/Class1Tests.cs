﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adjudicator;
using Xunit;

namespace AdjudicatorTests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1Tests
    {
        [Fact]
        public void HelloTest()
        {
            var c = new Class1();
            var actual = c.Hello();
            Assert.Equal("Hello World", actual);
        }
        public Class1Tests()
        {
        }
    }
}
