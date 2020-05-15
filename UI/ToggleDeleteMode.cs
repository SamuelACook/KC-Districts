using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets;
using Assets.Code;
using TMPro;




namespace Slooth.KingdomAndCastles.District{
    public class ToggleDeleteMode : MonoBehaviour
    {
        private Button button;
        public static Action AcivateDeleteMode;
        private bool isActive = false;
        private Image image;

        private void Awake() {
            image = this.transform.GetComponent<Image>();
            this.button = this.transform.GetComponent<Button>();
            this.button.onClick.AddListener(delegate { 
                AcivateDeleteMode();
                this.isActive = !this.isActive;
                this.ChangeColor();
            });
        }


        private void ChangeColor()
        {
            if(isActive)
                image.color = Color.red;
            else
                image.color = Color.white;
        }



    }
}