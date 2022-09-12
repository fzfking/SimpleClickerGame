using UnityEngine;

namespace Sources.Data
{
    [CreateAssetMenu(menuName = "Data/Create static data", fileName = "StaticDataContainer", order = 0)]
    public class StaticDataContainer: ScriptableObject
    {
        public ResourcesDataContainer ResourcesDataContainer;
        public GeneratorsDataContainer GeneratorsDataContainer;
    }
}