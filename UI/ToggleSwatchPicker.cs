
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
    public class ToggleSwatchPicker : MonoBehaviour
    {
        public GameObject picker;
        private Button button;

        public static ToggleSwatchPicker inst;

        private void Start()
        {
            ToggleSwatchPicker.inst = this;
            this.picker = Assets.District_UI_Children.Find(i => i.name == "Picker").gameObject;
            this.button = this.transform.GetComponent<Button>();
            this.button.onClick.AddListener(delegate { this.TogglePicker(); });
        }

        public void TogglePicker()
        {
            picker.SetActive(!picker.activeSelf);
            SfxSystem.inst.PlayFromBank("ui_openbuildmenu", base.transform.position, 0f, null, null);
        }
    }
}