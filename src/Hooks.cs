using Harmony;
using System.Reflection;
using System;

namespace AudicaModding
{
    internal static class Hooks
    {
        public static void ApplyHooks(HarmonyInstance instance)
        {
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(OptionsMenu), "ShowPage", new Type[] { typeof(OptionsMenu.Page) })]
        private static class PatchShowPage
        {
            private static void Postfix(OptionsMenu __instance, ref OptionsMenu.Page page)
            {
                if (AudicaMod.menuSpawned && page != OptionsMenu.Page.Misc) AudicaMod.menuSpawned = false;
                else if(!AudicaMod.menuSpawned && page == OptionsMenu.Page.Misc) AudicaMod.CreateSettingsButton(__instance);
            }
        }

        [HarmonyPatch(typeof(MenuState), "SetState", new Type[] { typeof(MenuState.State) })]
        private static class PatchSetState
        {
            private static void Postfix(MenuState __instance, ref MenuState.State state)
            {
                if (!AudicaMod.config.activated) return;

                if (AudicaMod.playPsychadelia && (state == MenuState.State.Launched || state == MenuState.State.TitleScreen))
                {
                    AudicaMod.playPsychadelia = false;
                    GameplayModifiers.I.mPsychedeliaPhase = 0;
                    AudicaMod.timer = 0;
                }
                else if (!AudicaMod.playPsychadelia && state != MenuState.State.Launched && state != MenuState.State.TitleScreen) AudicaMod.playPsychadelia = true;
                


            }
        }
    }

    
}