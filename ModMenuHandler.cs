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
    public class ModMenuHandler
    {
        
        public void Init()
        {
            var config = ModConfigBuilder
                .Create("District", Mod.version.ToString(2), "Slooth")
                .Build();

            ModSettingsBootstrapper.Register(config, OnModRegistered, OnModRegistrationFailed);
        }

        private void OnModRegistered(ModSettingsProxy proxy, SettingsEntry[] saved)
        {
        }

        private void OnModRegistrationFailed(Exception err)
        {
        }

    }
}
