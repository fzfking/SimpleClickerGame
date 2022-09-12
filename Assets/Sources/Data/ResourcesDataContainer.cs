using System;
using UnityEngine;

namespace Sources.Data
{
    [Serializable]
    public class ResourcesDataContainer
    {
        public ResourceData[] Resources => ResourceDataArray;
        [SerializeField] private ResourceData[] ResourceDataArray;
    }
}