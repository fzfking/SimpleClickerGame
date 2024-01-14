using System;
using AYellowpaper.SerializedCollections;

namespace Configuration
{
    [Serializable]
    public partial class ManagerDef
    {
        public byte Id;
        public string NameKey;
        public string DescriptionKey;
        public string IconKey;
        public string ImageKey;
        public SerializedDictionary<byte, double> Price;
        public byte ProducerId;
    }
}