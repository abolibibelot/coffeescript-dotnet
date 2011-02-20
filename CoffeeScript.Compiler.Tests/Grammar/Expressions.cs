using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CoffeeScript.Compiler.Tests.Grammar
{
    [TestFixture]
    public class Expressions : GrammarFixture
    {

        [Test]
        public void SimpleAssign()
        {
            var tree = Parse("assign");

            var i = 0;
        }
    }
}
