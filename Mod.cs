using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

using Assets;
using Assets.Code;
using Harmony;

using Zat.Shared.ModMenu.API;

namespace Slooth.KingdomAndCastles.District
{
    public class Mod
    {
        public static readonly String title = "District";
        public static readonly String author = "Slooth";
        public static readonly Version version = new Version(2,1);
        public static readonly Environment ENV = Environment.Production;
        
        
        public ModMenuHandler modMenu;
        public static KCModHelper helper;
        public static Mod inst;


        public enum Environment
        {
            Development,
            Production
        }

        public void PreScriptLoad(KCModHelper _helper)
        {
            inst = this;

            modMenu = new ModMenuHandler();
            
            var harmony = HarmonyInstance.Create("DistrictHarmony");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            helper = _helper;
            Assets.LoadAssets(_helper);
        }

        public void OnScriptLoad(KCModHelper helper)
        {
            modMenu.Init();
        }


        public void SceneLoaded(KCModHelper _helper)
        {
            Assets.SetupAssets(_helper);
        }


        // Enable Development Output / Disable Output
        [HarmonyPatch(typeof(KCModHelper))]
        [HarmonyPatch("Log")]
        public static class EnvironmentPatch
        {
            public static bool Prefix(KCModHelper __instance)
            {
                if(Mod.ENV == Environment.Development)
                    return true;
                else
                    return false;
            }
        }

    }


}