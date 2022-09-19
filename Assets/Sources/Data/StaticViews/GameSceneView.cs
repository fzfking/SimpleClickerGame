using System;
using System.Collections.Generic;
using Sources.Architecture.Interfaces;
using Sources.Presenters.HelperViews;
using UnityEngine;

namespace Sources.Data.StaticViews
{
    public class GameSceneView : MonoBehaviour
    {
        public ProgressBar Progress => ProgressBar;
        public BuyAmountButton BuyAmount => BuyAmountButton;
        public InformationWindow InformationWindow => InfoWindow;
        public OfflineProductionWindow OfflineProductionWindow => OfflineProduction;
        
        [SerializeField] private Transform ResourcesParent;
        [SerializeField] private Transform GeneratorsParent;
        [SerializeField] private Transform ManagersParent;
        [SerializeField] private ProgressBar ProgressBar;
        [SerializeField] private BuyAmountButton BuyAmountButton;
        [SerializeField] private InformationWindow InfoWindow;
        [SerializeField] private OfflineProductionWindow OfflineProduction;
        
        private IReadOnlyDictionary<Type, Transform> _parents;

        public Transform Get<T>() where T: IVisualData
        {
            if (_parents == null)
            {
                InitDictionary();
            }

            return _parents[typeof(T)];
        }

        private void InitDictionary()
        {
            _parents = new Dictionary<Type, Transform>()
            {
                [typeof(IResource)] = ResourcesParent,
                [typeof(IGenerator)] = GeneratorsParent,
                [typeof(IManager)] = ManagersParent,
            };
        }

        private void OnEnable()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }
}