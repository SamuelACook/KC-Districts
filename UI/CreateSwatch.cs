
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Assets;
using Assets.Code;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

/*
    Author: Slooth
    Date: 04/06/2020
    Mod: District
    Description: Allows the player to customize the color of any buildings roof.
*/

namespace Slooth.KingdomAndCastles.District{
    public class CreateSwatch : MonoBehaviour
    {
        private Button button;

        private void Start()
        {
            this.button = this.transform.GetComponent<Button>();
            this.button.onClick.AddListener(delegate {
                SfxSystem.inst.PlayFromBank("BuildingSelectWell", base.transform.position, 0f, null, null);
                District.inst.AddSwatch(ColorPicker.inst.CurrentColor, District.SwatchType.Player);
                ColorPicker.inst.gameObject.SetActive(false);
            });
        }
    }
}