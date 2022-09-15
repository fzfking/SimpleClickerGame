using System;
using System.Collections.Generic;
using Sources.Data;
using Sources.Presenters;
using Sources.Presenters.HelperViews;

namespace Sources.GameLoop
{
    public static class Constants
    {
        public static readonly Dictionary<Type, string> Paths = new Dictionary<Type, string>()
        {
            [typeof(ResourcePresenter)] = @"Assets/Data/StaticData/Presenters/ResourcePresenter.prefab",
            [typeof(StaticDataContainer)] = @"Assets/Data/StaticData/StaticDataContainer.asset",
            [typeof(GeneratorPresenter)] = @"Assets/Data/StaticData/Presenters/GeneratorPresenter.prefab",
            [typeof(ManagerPresenter)] = @"Assets/Data/StaticData/Presenters/ManagerPresenter.prefab",
            [typeof(Popup)] = @"Assets/Data/StaticData/Helpers/Popup.prefab",
        };
    }
}