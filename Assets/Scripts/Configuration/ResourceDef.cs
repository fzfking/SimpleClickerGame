using System;

namespace Configuration
{
    [Serializable]
    public class ResourceDef
    {
        public byte Id;
        public string NameKey;
        public string DescriptionKey;
        public string IconKey;
        public string ImageKey;
    }
}