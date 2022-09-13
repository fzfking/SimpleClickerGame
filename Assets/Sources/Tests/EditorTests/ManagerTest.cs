using System.Collections;
using NUnit.Framework;
using Sources.Models;
using UnityEngine.TestTools;

namespace Sources.Tests.EditorTests
{
    [TestFixture]
    public class ManagerTest
    {
        [UnityTest]
        public IEnumerator ManagerProductionTest()
        {
            var resource = Resource.CreateMock(0);
            var generator = Generator.CreateMock(resource, 2, 1, 0);
            var manager = Manager.CreateMock(generator);
            manager.ChangeActive(true);
            for (int i = 0; i < 5; i++)
            {
                yield return null;
            }
            manager.ChangeActive(false);
            
            for (int i = 0; i < 5; i++)
            {
                yield return null;
            }
            Assert.AreEqual(5, resource.CurrentValue.Value);
        }
    }
}