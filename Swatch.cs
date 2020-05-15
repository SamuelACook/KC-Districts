
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
    Description: 
*/
namespace Slooth.KingdomAndCastles.District{
    public class Swatch : MonoBehaviour
    {
        public Color color;
        
        private Button button;
        private Button deleteButton;
        private GameObject selected;
        private Image display;
        

        void Awake()
        {
            this.button = this.transform.GetComponent<Button>();
            this.button.onClick.AddListener(delegate { District.inst.AssignColor(this.color); });
            this.deleteButton = this.transform.GetChild(1).GetComponent<Button>();
            this.deleteButton.onClick.AddListener(delegate {
                District.PlayerAddedColors.Remove(ColorConversions.ConvertColorToVector3(this.color));
                Destroy(this.gameObject);
            });

            this.selected = this.transform.GetChild(0).gameObject;
            
            this.display = this.transform.GetComponent<Image>();
            
            District.AssignColorEvent += SetSelected;
            ToggleDeleteMode.AcivateDeleteMode += EnableDeleteButton;
        }

        void Start()
        {
            this.display.color = this.color;
            this.gameObject.SetActive(true);
        }

        void OnDestroy()
        {
            District.AssignColorEvent -= SetSelected;
            ToggleDeleteMode.AcivateDeleteMode -= EnableDeleteButton;
        }

        void SetSelected(Color color)
        {
            if(this.color == color)
                this.selected.SetActive(true);
            else
                this.selected.SetActive(false);
        }

        void EnableDeleteButton()
        {
            if(District.PlayerAddedColors.Contains(ColorConversions.ConvertColorToVector3(this.color))){
                GameObject btn = this.deleteButton.gameObject;
                btn.SetActive(!btn.activeSelf);
            }
        }
        

    }
}