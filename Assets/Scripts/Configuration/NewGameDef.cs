using System;
using AYellowpaper.SerializedCollections;

namespace Configuration
{
    [Serializable]
    public partial class NewGameDef
    {
        public SerializedDictionary<byte, double> ResourcesByDefault;
        public SerializedDictionary<byte, double> ProducersByDefault;
        public SerializedDictionary<byte, bool> ManagersByDefault;
    }
}