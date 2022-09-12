using UnityEditor;

namespace Sources.Data
{
    public static class StaticDataLoader
    {
        private const string StaticDataPath = @"Assets/Data/StaticData/StaticDataContainer.asset";
        private static StaticDataContainer _dataContainer;
        public static StaticDataContainer DataContainer => _dataContainer ??= LoadDataContainer();

        private static StaticDataContainer LoadDataContainer()
        {
            return AssetDatabase.LoadAssetAtPath<StaticDataContainer>(StaticDataPath);
        }
    }
}