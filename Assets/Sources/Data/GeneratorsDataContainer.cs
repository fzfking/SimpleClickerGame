using System;
using UnityEngine;

namespace Sources.Data
{
    [Serializable]
    public class GeneratorsDataContainer
    {
        public GeneratorData[] Generators => GeneratorDataArray;
        [SerializeField] private GeneratorData[] GeneratorDataArray;
    }
}