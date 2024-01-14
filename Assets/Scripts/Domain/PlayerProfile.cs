using System;
using System.Collections.Generic;
using System.Text;
using Configuration;
using MemoryPack;
using UnityEngine;

namespace Domain
{
    [MemoryPackable]
    public partial class PlayerProfile : IDisposable
    {
        [MemoryPackIgnore] public IReadOnlyDictionary<byte, double> Resources => resources;
        [MemoryPackIgnore] public IReadOnlyDictionary<byte, double> Producers => producers;
        [MemoryPackIgnore] public IReadOnlyDictionary<byte, bool> Managers => managers;
        [MemoryPackIgnore] public double LevelsBuyAmount => levelsBuyAmount;
        
        [MemoryPackInclude] private readonly Dictionary<byte, double> resources;
        [MemoryPackInclude] private readonly Dictionary<byte, double> producers;
        [MemoryPackInclude] private readonly Dictionary<byte, bool> managers;
        [MemoryPackInclude] private double levelsBuyAmount;

        public PlayerProfile(Dictionary<byte, double> resources, Dictionary<byte, double> producers, Dictionary<byte, bool> managers, double levelsBuyAmount)
        {
            this.resources = resources;
            this.producers = producers;
            this.managers = managers;
            this.levelsBuyAmount = levelsBuyAmount;
        }

        public void SetResource(byte resourceId, double amount)
        {
            resources[resourceId] = amount;
        }
        
        public void SetProducer(byte producerId, double amount)
        {
            producers[producerId] = amount;
        }
        
        public void SetManager(byte managerId, bool value)
        {
            managers[managerId] = value;
        }

        public void SetLevelsBuyAmount(double amount)
        {
            levelsBuyAmount = amount;
        }

        public void Save()
        {
            var serializedProfile = MemoryPackSerializer.Serialize(this);
            PlayerPrefs.SetString(nameof(PlayerProfile), Encoding.UTF8.GetString(serializedProfile));
            PlayerPrefs.Save();
        }

        public static PlayerProfile Load(NewGameDef defaults)
        {
            var serializedProfile = PlayerPrefs.GetString(nameof(PlayerProfile), string.Empty);
            var profile = string.IsNullOrEmpty(serializedProfile) 
                ? new PlayerProfile(defaults.ResourcesByDefault, defaults.ProducersByDefault, defaults.ManagersByDefault, 1) 
                : MemoryPackSerializer.Deserialize<PlayerProfile>(Encoding.UTF8.GetBytes(serializedProfile));

            return profile;
        }

        public void Dispose()
        {
            Save();
        }
    }
}