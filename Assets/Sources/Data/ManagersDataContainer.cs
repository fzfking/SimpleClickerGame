using System;
using UnityEngine;

namespace Sources.Data
{
    [Serializable]
    public class ManagersDataContainer
    {
        public ManagerData[] Managers => ManagerDataArray;
        [SerializeField] private ManagerData[] ManagerDataArray;
    }
}