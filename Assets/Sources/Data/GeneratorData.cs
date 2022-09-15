using Sources.Architecture.Interfaces;
using UnityEngine;

namespace Sources.Data
{
    [CreateAssetMenu(fileName = "Generator", menuName = "Data/Create generator", order = 0)]
    public class GeneratorData : VisualScriptableObject
    {
        [SerializeField] private ResourceData _productionResource;
        [SerializeField] private ResourceData _costResource;
        [SerializeField] private double _baseProduction;
        [SerializeField] private double _baseCost;
        [SerializeField] private float _baseDelay;
        [SerializeField] private bool isUnlockedByDefault;

        public ResourceData ProductionResource => _productionResource;
        public ResourceData CostResource => _costResource;
        public double BaseProduction => _baseProduction;
        public double BaseCost => _baseCost;
        public float BaseDelay => _baseDelay;
        public bool IsUnlockedByDefault => isUnlockedByDefault;


    }
}