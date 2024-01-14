using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Domain.Services
{
    public interface IConfigLoader
    {
        UniTask<T> LoadAsync<T>(string fileName);
    }
    public class ConfigLoader : IConfigLoader
    {
        private static string ConfigsPath => Application.dataPath + "/Resources/Config/";

        public async UniTask<T> LoadAsync<T>(string fileName)
        {
            var textFile = await Resources.LoadAsync("Config/" + fileName).ToUniTask();
            var serialized = (textFile as TextAsset)?.text;
            Resources.UnloadAsset(textFile);
            return JsonConvert.DeserializeObject<T>(serialized ?? string.Empty);
        }
    }
}