using Sources.Architecture.Interfaces;
using UnityEngine;

namespace Sources.Data
{
    [CreateAssetMenu(menuName = "Data/Create static data", fileName = "StaticDataContainer", order = 0)]
    public class StaticDataContainer: ScriptableObject, ILoadable
    {
        public ResourcesDataContainer ResourcesDataContainer;
        public GeneratorsDataContainer GeneratorsDataContainer;
        public ManagersDataContainer ManagersDataContainer;
        public UIData UIData;
    }
}