using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using Harmony;
using System.IO;

namespace AudicaModding
{
    public class AudicaMod : MelonMod
    {
        public static bool playPsychadelia = false;

        public static bool menuSpawned = false;
        public static OptionsMenuButton toggleButton = null;
        public static OptionsMenuSlider speedSlider = null;

        public static float timer = 0.0f;
        public static float defaultPhaseSeconds = 14.28f;

        public static Config config = new Config();
        public static string path = Application.dataPath + "/../Mods/Config/TrippyMenu.json";

        public static class BuildInfo
        {
            public const string Name = "TrippyMenus";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "Alternity"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "0.2.1"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
        }
        
		 public override void OnApplicationStart()
         {
            HarmonyInstance instance = HarmonyInstance.Create("AudicaMod");
            Hooks.ApplyHooks(instance);
            LoadConfig();
         }

        public static void SaveConfig()
        {
            Directory.CreateDirectory(Application.dataPath + "/../Mods/Config");
            string contents = Encoder.GetConfig(config);
            File.WriteAllText(path, contents);
            MelonModLogger.Log("Config saved");
        }

        public static void LoadConfig()
        {
            if (!File.Exists(path))
            {
                SaveConfig();
            }
            Encoder.SetConfig(config, File.ReadAllText(path));
            MelonModLogger.Log("Config loaded");
        }

        public static void UpdateSlider(OptionsMenuSlider slider, string text)
        {
            if (slider == null)
            {
                return;
            }
            else
            {
                slider.label.text = text;
                SaveConfig();
            }
        }

        public static void CreateSettingsButton(OptionsMenu optionsMenu)
        {
            string toggleText = "OFF";

            if (config.activated)
            {
                toggleText = "ON";
            }

            optionsMenu.AddHeader(0, "Trippy Menu");

            toggleButton = optionsMenu.AddButton
                (0,
                toggleText,
                new Action(() =>
                {
                    if (config.activated)
                    {
                        config.activated = false;
                        playPsychadelia = false;
                        toggleButton.label.text = "OFF";
                        SaveConfig();
                        GameplayModifiers.I.mPsychedeliaPhase = 0.00000001f;
                        timer = 0;
                    }
                    else
                    {
                        config.activated = true;
                        playPsychadelia = true;
                        toggleButton.label.text = "ON";
                        SaveConfig();
                    }
                }),
                null,
                "Turns Trippy Menu on or off");

            speedSlider = optionsMenu.AddSlider
                (
                0,
                "Trippy Menu Cycle Speed",
                "P",
                new Action<float>((float n) =>
                {
                    config.speed = Mathf.Round((config.speed + n) * 1000.0f) / 1000.0f;
                    UpdateSlider(speedSlider, "Speed : " + config.speed.ToString());
                }),
                null,
                null,
                "Changes color cycle speed"
                );
            speedSlider.label.text = "Speed : " + config.speed.ToString();

            //MelonModLogger.Log("Buttons created");
            menuSpawned = true;
        }

        public override void OnUpdate()
        {
            if (!playPsychadelia) return;

            float phaseTime = defaultPhaseSeconds / config.speed;

            if (timer <= phaseTime)
            {
                timer += Time.deltaTime;

                float forcedPsychedeliaPhase = timer / (phaseTime);
                GameplayModifiers.I.mPsychedeliaPhase = forcedPsychedeliaPhase;
            }
            else timer = 0;
        }
    }
}




























































