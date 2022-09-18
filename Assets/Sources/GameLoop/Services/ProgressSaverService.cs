using System;
using System.Globalization;
using Sources.Architecture.Interfaces;
using Sources.Models;
using UnityEngine;

namespace Sources.GameLoop.Services
{
    public class ProgressSaverService : IProgressSaverService
    {
        public void Save<TData>(TData data) where TData : IVisualData
        {
            var type = data.GetType();
            if (type == typeof(Resource))
            {
                var resource = (IResource)data;
                PlayerPrefs.SetString($"Resource: {resource.Name}",
                    resource.CurrentValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(Generator))
            {
                var generator = (IGenerator)data;
                PlayerPrefs.SetInt($"Generator Level: {generator.Name}",
                    generator.Level.Value);
                PlayerPrefs.SetFloat($"Generator progress: {generator.Name}",
                    generator.Progress.Value);
            } 
            else if (type == typeof(Manager))
            {
                var manager = (IManager)data;
                PlayerPrefs.SetString($"Manager: {manager.Name}",
                    manager.IsActive.ToString());
            }
            else
            {
                throw new Exception($"There is no any data type associated with {type} to save progress.");
            }

            PlayerPrefs.Save();
        }
    }
}