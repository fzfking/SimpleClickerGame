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
            [typeof(ResourcePresenter)] = @"StaticData/Presenters/ResourcePresenter",
            [typeof(StaticDataContainer)] = @"StaticData/StaticDataContainer",
            [typeof(GeneratorPresenter)] = @"StaticData/Presenters/GeneratorPresenter",
            [typeof(ManagerPresenter)] = @"StaticData/Presenters/ManagerPresenter",
            [typeof(Popup)] = @"StaticData/Helpers/Popup",
            [typeof(LockedGeneratorPresenter)] = @"StaticData/Presenters/LockedGeneratorPresenter",
        };
    }
}