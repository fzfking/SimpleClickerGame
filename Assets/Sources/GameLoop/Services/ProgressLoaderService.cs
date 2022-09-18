using System;
using System.Globalization;
using Sources.Architecture.Interfaces;
using Sources.Data;
using Sources.Data.ProgressContainers;
using Sources.Models;
using UnityEngine;

namespace Sources.GameLoop.Services
{
    public class ProgressLoaderService : IProgressLoaderService
    {
        public IProgressContainer Load<TData>(string name) where TData : IVisualData
        {
            var type = typeof(TData);
            if (type == typeof(IResource))
            {
                var value = Double.Parse(PlayerPrefs.GetString($"Resource: {name}", "0"));
                return new ResourceProgressContainer
                {
                    Value = value
                };
            }
            else if (type == typeof(IGenerator))
            {
                var level = PlayerPrefs.GetInt($"Generator Level: {name}", 0);
                var progress = PlayerPrefs.GetFloat($"Generator progress: {name}", 0f);
                return new GeneratorProgressContainer
                {
                    Level = level,
                    Progress = level
                };
            }
            else if (type == typeof(IManager))
            {
                var isBuyed = bool.Parse(PlayerPrefs.GetString($"Manager: {name}", false.ToString()));
                return new ManagerProgressContainer
                {
                    IsBuyed = isBuyed
                };
            }
            else
            {
                throw new Exception($"There is no type to load associated with {type}");
            }
        }
    }
}