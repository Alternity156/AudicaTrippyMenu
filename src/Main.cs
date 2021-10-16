using System;
using Harmony;
using MelonLoader;
using UnityEngine;
using System.Reflection;

namespace AudicaModding
{
    public static class Config
    {
        public const string Category = "TrippyMenu";

        public static bool Enabled;
        public static float Speed;

        public static void RegisterConfig()
        {
            MelonPrefs.RegisterBool(Category, nameof(Enabled), false, "Enables psychedelia.");
            MelonPrefs.RegisterFloat(Category, nameof(Speed), 1.0f, "Cycle speed [0.1,100,0.1,1]");
            OnModSettingsApplied();
        }

        public static void OnModSettingsApplied()
        {
            foreach (var fieldInfo in typeof(Config).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fieldInfo.FieldType == typeof(bool))
                    fieldInfo.SetValue(null, MelonPrefs.GetBool(Category, fieldInfo.Name));

                if (fieldInfo.FieldType == typeof(float))
                    fieldInfo.SetValue(null, MelonPrefs.GetFloat(Category, fieldInfo.Name));
            }
        }
    }

    public class AudicaMod : MelonMod
    {
        public static class BuildInfo
        {
            public const string Name = "TrippyMenus";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "Alternity"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.1.4"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
        }

        public override void OnApplicationStart()
        {
            Config.RegisterConfig();
        }

        public override void OnModSettingsApplied()
        {
            Config.OnModSettingsApplied();
        }

        private static bool playPsychedelia => Config.Enabled;
        private static float speed => Config.Speed;

        public static float timer = 0.0f;
        public static float defaultPhaseSeconds = 14.28f;

        public static void InSong()
        {
            Config.Enabled = false;
            GameplayModifiers.I.mPsychedeliaPhase = 0;
            timer = 0;
        }

        public override void OnUpdate()
        {
            if (!playPsychedelia) return;

            float phaseTime = defaultPhaseSeconds / speed;

            if (timer <= phaseTime)
            {
                timer += Time.deltaTime;

                float forcedPsychedeliaPhase = timer / (phaseTime);
                GameplayModifiers.I.mPsychedeliaPhase = forcedPsychedeliaPhase;
            }
            else timer = 0;
        }
    }

    internal static class Hooks
    {
        [HarmonyPatch(typeof(MenuState), "SetState", new Type[] { typeof(MenuState.State) })]
        private static class PatchSetState
        {
            private static void Postfix(MenuState __instance, ref MenuState.State state)
            {
                if (Config.Enabled == false) return;

                if (Config.Enabled && (state == MenuState.State.Launched || state == MenuState.State.TitleScreen))
                {

                }
                else if (!Config.Enabled && state != MenuState.State.Launched && state != MenuState.State.TitleScreen) Config.Enabled = true;



            }
        }
    }
}




