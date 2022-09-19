﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sources.Architecture.Extensions;
using Sources.Architecture.Interfaces;
using Sources.Presenters;
using UniRx;
using UnityEngine;

namespace Sources.GameLoop.States
{
    public class GameLoopState : IPayloadableState<GeneratorPresenter[]>
    {
        private readonly IServiceLocator _allServices;
        private readonly List<IDeInitiable> _initiables;
        private readonly List<IVisualData> _saveables;
        private IPopupService _popupService;
        private GeneratorPresenter[] _generatorPresenters;

        public GameLoopState(IServiceLocator allServices, List<IDeInitiable> initiables, List<IVisualData> saveables)
        {
            _allServices = allServices;
            _initiables = initiables;
            _saveables = saveables;
        }

        public void Enter(GeneratorPresenter[] payload)
        {
            _popupService = _allServices.Get<IPopupService>();
            _generatorPresenters = payload;
            foreach (var presenter in _generatorPresenters)
            {
                presenter.Ended += InvokePopup;
            }
        }

        public void Exit()
        {
            var progressSaver = _allServices.Get<IProgressSaverService>();
            SaveProgress<IResource>(progressSaver);
            SaveProgress<IGenerator>(progressSaver);
            SaveProgress<IManager>(progressSaver);
            PlayerPrefs.SetString("ExitTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.Save();

            foreach (var initiable in _initiables)
            {
                initiable.DeInit();
            }
        }

        private void SaveProgress<T>(IProgressSaverService progressSaver) where T: IVisualData
        {
            foreach (var saveable in _saveables.OfType<T>())
            {
                progressSaver.Save(saveable);
            }
        }

        private void InvokePopup(Vector2 position, double value)
        {
            _popupService.ShowPopup(position, $"+{value.ToResourceFormat()}");
        }
    }
}