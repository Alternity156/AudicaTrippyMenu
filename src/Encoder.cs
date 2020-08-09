using MelonLoader;
using SimpleJSON;

namespace AudicaModding
{
    public class Encoder
    {
        public static string GetConfig(Config config)
        {
            var configJSON = new JSONObject();

            configJSON["activated"] = config.activated;
            configJSON["speed"] = config.speed;

            return configJSON.ToString(4);
        }

        public static void SetConfig(Config config, string data)
        {
            var configJSON = JSON.Parse(data);

            config.activated = configJSON["activated"];
            config.speed = configJSON["speed"];
        }
    }
}
