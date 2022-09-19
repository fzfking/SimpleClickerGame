using System;
using Sources.Architecture.Interfaces;
using Sources.Data.StaticViews;
using Sources.Presenters.HelperViews;
using UnityEngine;

namespace Sources.Data
{
    [Serializable]
    public class UIData
    {
        [SerializeField] private GameSceneView UIPreset;
        private GameSceneView _instance;
        private GameSceneView Instance => _instance ??= GameObject.Instantiate(UIPreset);
        public Transform Get<T>() where T : IVisualData => Instance.Get<T>();
        public ProgressBar ProgressBar => Instance.Progress;
        public BuyAmountButton BuyAmountButton => Instance.BuyAmount;
        public InformationWindow InformationWindow => Instance.InformationWindow;
        public OfflineProductionWindow OfflineProductionWindow => Instance.OfflineProductionWindow;
    }
}