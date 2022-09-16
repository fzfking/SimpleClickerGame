using System;
using System.Collections;
using NUnit.Framework;
using Sources.Architecture.Extensions;
using Sources.Models;
using UniRx;
using UnityEngine;
using UnityEngine.TestTools;

namespace Sources.Tests.EditorTests
{
    [TestFixture]
    public class ModelsTest
    {
        private CompositeDisposable _compositeDisposable;
        [Test]
        public void ResourceTest()
        {
            var resource = Resource.CreateMock(0);
            Assert.AreEqual(0, resource.CurrentValue.Value);
            resource.Increase(10);
            Assert.AreEqual(10, resource.CurrentValue.Value);
        }

        [UnityTest]
        public IEnumerator GeneratorTest()
        {
            _compositeDisposable = new CompositeDisposable();
            var resource = Resource.CreateMock(0);
            resource.CurrentValue.Subscribe(x =>
            {
                Debug.Log(x.ToResourceFormat());
            }).AddTo(_compositeDisposable);
            var generator = Generator.CreateMock(resource, 4, 1);
            Assert.AreEqual(0, resource.CurrentValue.Value);
            generator.TryProduce();
            yield return null;
            Assert.AreEqual(1, resource.CurrentValue.Value);
            for (int i = 0; i < 20; i++)
            {
                generator.TryProduce();
                yield return null;
            }

            Assert.AreEqual(21, resource.CurrentValue.Value);
            Assert.AreEqual(1, generator.Level.Value);
            for (int i = 0; i < 2; i++)
            {
                generator.TryUpgrade(1);
            }

            Assert.AreEqual(3, generator.Level.Value);
            _compositeDisposable.Dispose();
        }

        [UnityTest]
        public IEnumerator GeneratorsDelayTest()
        {
            _compositeDisposable = new CompositeDisposable();
            var resource = Resource.CreateMock(0);
            var generator = Generator.CreateMock(resource, 4, 1, 2f);
            generator.Progress.Subscribe(x =>
            {
                Debug.Log("Progress: " + x);
            }).AddTo(_compositeDisposable);
            generator.Progress.Where(x => x == 0f).Skip(1).Subscribe(x =>
            {
                _compositeDisposable.Clear();
            }).AddTo(_compositeDisposable);
            generator.TryProduce();
            while (_compositeDisposable.Count > 0)
            {
                yield return null;
            }
            Assert.AreEqual(1, resource.CurrentValue.Value);
        }
    }
}