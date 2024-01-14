using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Configuration
{
    public class GameConfigEditor : MonoBehaviour
    {
        private static string ConfigsPath => Application.dataPath + "/Resources/Config/";

        public List<ResourceDef> resources;
        public List<ProducerDef> producers;
        public List<ManagerDef> managers;
        public NewGameDef newGameSetup;

        [Button]
        public void SaveToPersistent()
        {
            var resourcesJson = JsonConvert.SerializeObject(resources);
            var producersJson = JsonConvert.SerializeObject(producers);
            var managersJson = JsonConvert.SerializeObject(managers);
            var newGameSetupJson = JsonConvert.SerializeObject(newGameSetup);

            var resourcesPath = ConfigsPath + "resources.json";
            File.WriteAllText(resourcesPath, resourcesJson);

            var producersPath = ConfigsPath + "producers.json";
            File.WriteAllText(producersPath, producersJson);

            var managersPath = ConfigsPath + "managers.json";
            File.WriteAllText(managersPath, managersJson);

            var newGameSetupPath = ConfigsPath + "newGame.json";
            File.WriteAllText(newGameSetupPath, newGameSetupJson);

            Debug.Log("Successfully saved configuration");
        }
    }
}