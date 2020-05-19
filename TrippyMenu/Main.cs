using MelonLoader;
using NET_SDK;
using NET_SDK.Harmony;
using System;
using System.IO;
using UnityEngine;

namespace TrippyMenu
{
    public static class BuildInfo
    {
        public const string Name = "TrippyMenu"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "Alternity"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.1"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class TrippyMenu : MelonMod
    {
        public static Patch OptionsMenu_ShowPage;

        //The current way of tracking menu state.
        //TODO: Hook to the SetMenuState function without breaking the game
        public static MenuState.State menuState;
        public static MenuState.State oldMenuState;

        public static bool menuSpawned = false;
        public static OptionsMenuButton toggleButton = null;
        public static OptionsMenuSlider speedSlider = null;

        public static float timer = 0.0f;
        public static float defaultPhaseSeconds = 14.28f;

        public static Config config = new Config();
        public static string path = Application.dataPath + "/../Mods/Config/TrippyMenu.json";

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

        public override void OnApplicationStart()
        {
            LoadConfig();
        }

        public override void OnUpdate()
        {
            //Tracking menu state
            menuState = MenuState.GetState();

            //If menu changes
            if (menuState != oldMenuState)
            {
                if (menuState == MenuState.State.Launched)
                {
                    GameplayModifiers.I.mPsychedeliaPhase = 0;
                    timer = 0;
                    
                }
                oldMenuState = menuState;
            }

            if (menuState == MenuState.State.SettingsPage)
            {
                OptionsMenu[] optionsMenus = GameObject.FindObjectsOfType<OptionsMenu>();

                bool miscPageFound = false;

                for (int i = 0; i < optionsMenus.Length; i++)
                {
                    if (optionsMenus[i].mPage == OptionsMenu.Page.Misc)
                    {
                        miscPageFound = true;
                    }
                }

                if (miscPageFound && !menuSpawned)
                {
                    for (int i = 0; i < optionsMenus.Length; i++)
                    {
                        if (optionsMenus[i].mPage == OptionsMenu.Page.Misc)
                        {
                            MelonModLogger.Log("Hi");


                            string toggleText = "OFF";

                            if (config.activated)
                            {
                                toggleText = "ON";
                            }

                            optionsMenus[i].AddHeader(0, "Trippy Menu");

                            toggleButton = optionsMenus[i].AddButton
                                (0,
                                toggleText,
                                new Action(() =>
                                {
                                    if (config.activated)
                                    {
                                        config.activated = false;
                                        toggleButton.label.text = "OFF";
                                        SaveConfig();
                                        GameplayModifiers.I.mPsychedeliaPhase = 0;
                                        timer = 0;
                                    }
                                    else
                                    {
                                        config.activated = true;
                                        toggleButton.label.text = "ON";
                                        SaveConfig();
                                    }
                                }),
                                null,
                                "Turns Trippy Menu on or off");

                            speedSlider = optionsMenus[i].AddSlider
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

                            menuSpawned = true;
                            //optionsMenu.scrollable.AddRow(optionsMenu.AddHeader(0, "Trippy Menu"));

                            //optionsMenu.scrollable.AddRow(toggleButton.gameObject);
                            //optionsMenu.scrollable.AddRow(speedSlider.gameObject);
                        }
                    }
                }
                else if (!miscPageFound)
                {
                    menuSpawned = false;
                }

            }

            if (menuState != MenuState.State.Launched && menuState != MenuState.State.TitleScreen && config.activated)
            {
                float phaseTime = defaultPhaseSeconds / config.speed;

                if (timer <= phaseTime)
                {
                    timer += Time.deltaTime;

                    float forcedPsychedeliaPhase = timer / (phaseTime);
                    GameplayModifiers.I.mPsychedeliaPhase = forcedPsychedeliaPhase;
                    //MelonModLogger.Log("Psychedelia phase: " + forcedPsychedeliaPhase.ToString());
                }
                else
                {
                    timer = 0;
                }
            }
        }

        /*
        public override void OnLevelWasLoaded(int level)
        {
            MelonModLogger.Log("OnLevelWasLoaded: " + level.ToString());
        }

        public override void OnLevelWasInitialized(int level)
        {
            MelonModLogger.Log("OnLevelWasInitialized: " + level.ToString());
        }

        public override void OnFixedUpdate()
        {
            MelonModLogger.Log("OnFixedUpdate");
        }

        public override void OnLateUpdate()
        {
            MelonModLogger.Log("OnLateUpdate");
        }

        public override void OnGUI()
        {
            MelonModLogger.Log("OnGUI");
        }

        public override void OnApplicationQuit()
        {
            MelonModLogger.Log("OnApplicationQuit");
        }

        public override void OnModSettingsApplied()
        {
            MelonModLogger.Log("OnModSettingsApplied");
        }
        */
    }
}
