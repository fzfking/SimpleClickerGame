using System;
using AYellowpaper.SerializedCollections;

namespace Configuration
{
    [Serializable]
    public class ProducerDef
    {
        public byte Id;
        public string NameKey;
        public string DescriptionKey;
        public string IconKey;
        public string ImageKey;
        public double BaseProduction;
        public SerializedDictionary<double, double> LevelMultiplier;
        public SerializedDictionary<double, double> ProductionTimeByLevel;
        public byte ProducedResourceId;
        public SerializedDictionary<byte, double> Price;
    }
}