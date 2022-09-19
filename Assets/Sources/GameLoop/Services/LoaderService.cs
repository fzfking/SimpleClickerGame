using System;
using Sources.Architecture.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Sources.GameLoop.Services
{
    public class LoaderService: ILoaderService
    {
        public TResource Load<TResource>() where TResource : class, ILoadable
        {
            var type = typeof(TResource);
            var path = SelectPath(type);
            return Resources.Load(path, type) as TResource;
        }

        private string SelectPath(Type type)
        {
            return Constants.Paths[type];
        }
    }
}