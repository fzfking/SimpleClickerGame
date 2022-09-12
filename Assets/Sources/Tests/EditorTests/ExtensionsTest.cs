using System;
using System.Linq;
using NUnit.Framework;
using Sources.Architecture;
using Sources.Architecture.Extensions;
using UnityEngine;

namespace Sources.Tests.EditorTests
{
    public class ExtensionsTest
    {
        [Test]
        public void GenerationPostfixesBeginTest()
        {
            var value = 10580000000000d;
            Assert.AreEqual("10.58 AA", value.ToResourceFormat());
        }
        [Test]
        public void GenerationPostfixesEndTest()
        {
            Assert.AreEqual(" ZZ", HelperExtensions.Postfixes.Last());
        }
    }
}
