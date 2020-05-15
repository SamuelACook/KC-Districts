
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
    Description: Handles Expanding & Contracting District UI
*/

namespace Slooth.KingdomAndCastles.District{

    public class Expand : MonoBehaviour
    {
        private GameObject contentBar;
        private Button button;
        private Transform image;

        private void Start()
        {
            this.contentBar = Assets.District_UI_Children.Find(i => i.name == "ContentBar").gameObject;
            this.button = this.transform.GetComponent<Button>();
            this.button.onClick.AddListener(delegate { this.ToggleContentBar(); });
            this.image = this.transform.GetChild(0);
        }

        public void ToggleContentBar()
        {
            contentBar.SetActive(!contentBar.activeSelf);
            this.image.Rotate(0f, 0f , 180f, Space.Self);

            if(!contentBar.activeSelf)
                ToggleSwatchPicker.inst.picker.SetActive(false);
            SfxSystem.inst.PlayFromBank("ui_openbuildmenu", base.transform.position, 0f, null, null);
        }
    }
}