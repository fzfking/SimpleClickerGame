using Sources.Architecture.Interfaces;
using UnityEngine;

namespace Sources.Data
{
    public abstract class VisualScriptableObject : ScriptableObject, IVisualData
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
    }
}