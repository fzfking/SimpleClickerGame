using Sources.Architecture.Interfaces;
using UnityEngine;

namespace Sources.Data
{
    [CreateAssetMenu(fileName = "Manager", menuName = "Data/Create manager", order = 0)]
    public class ManagerData : VisualScriptableObject
    {
        public GeneratorData Generator => GeneratorData;
        public double Value => CostValue;
        public ResourceData Resource => CostResource;

        [SerializeField] private GeneratorData GeneratorData;
        [SerializeField] private double CostValue;
        [SerializeField] private ResourceData CostResource;
    }
}