using Sources.Architecture.Interfaces;
using UnityEngine;

namespace Sources.Data
{
    [CreateAssetMenu(fileName = "Manager", menuName = "Data/Create manager", order = 0)]
    public class ManagerData : VisualScriptableObject
    {
        [SerializeField] private IGenerator _generator;

        public IGenerator Generator => _generator;
    }
}