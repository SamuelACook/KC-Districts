using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets;
using Assets.Code;
using TMPro;

namespace Slooth.KingdomAndCastles.District{
    public class District: MonoBehaviour
    {

        public static District inst;
        public static event Action<Color> AssignColorEvent;

        public static Dictionary<Guid, Vector3[]> DistrictBuildingData = new Dictionary<Guid, Vector3[]>();

        public static List<Vector3> PlayerAddedColors = new List<Vector3>();
        public static List<Vector3> DefaultAddedColors = new List<Vector3>();

        public static List<Swatch> Swatches = new List<Swatch>();

        public TMP_Dropdown dropdown;

        private Building currentBuilding;
        private Vector3[] currentColors;
       
        private EventHandler<OnLoadedEvent> loadEvent;
        private EventHandler<OnSaveEvent> saveEvent;

        private Transform OptionsContainer;
        private GameObject Option;

        public enum SwatchType{
            Default,
            Player
        }


        void Awake() {
            District.inst = this;
            this.dropdown = Assets.District_UI_Children.Find(i => i.name == "Dropdown").GetComponent<TMP_Dropdown>();
            this.dropdown.onValueChanged.AddListener(delegate {DropdownValueChanged();});
            this.Init();
        }

        void Init()
        {
            this.OptionsContainer = Assets.District_UI_Children.Find(i => i.name == "Options");
            this.Option = AssetsLoader.GetAsset("Option") as GameObject;
        }

        void OnEnable()
        {
            this.dropdown.ClearOptions();

            this.currentBuilding = GameUI.inst.GetBuildingSelected();
            if(currentBuilding == null)
                return;

            List<String> items = new List<String>(this.currentBuilding.renderersWithBuildingShader.Count);
            foreach(Renderer renderer in this.currentBuilding.renderersWithBuildingShader)
            {
                items.Add(renderer.name);
            }
            this.dropdown.AddOptions(items);
            this.dropdown.value = 0;
            this.dropdown.RefreshShownValue();

            AssignColorEvent(this.currentBuilding.fullMaterial[this.dropdown.value].color);
        }


        void DropdownValueChanged()
        {
            SfxSystem.inst.PlayFromBank("ui_buttonclick", base.transform.position, 0f, null, null);
            AssignColorEvent(this.currentBuilding.fullMaterial[this.dropdown.value].color);
        }

        public void AddSwatch(Color color, SwatchType type)
        {
            // Check for Bad Data
            if(color == null)
                return;

            // No Player Duplicates
            if(Swatches.FindIndex(i => i.color == color) >= 0)
                return;
            
            Swatch swatch = MakeSwatch(color);

            if(swatch == null)
                return;
            
            if(type == SwatchType.Player)
                District.PlayerAddedColors.Add(ColorConversions.ConvertColorToVector3(color));
            else
                District.DefaultAddedColors.Add(ColorConversions.ConvertColorToVector3(color));
        }

        public Swatch MakeSwatch(Color color)
        {
            if(color == null)
                return null;

            if(Option == null|| OptionsContainer == null)
                this.Init();

            GameObject obj = GameObject.Instantiate(Option, OptionsContainer);
            obj.SetActive(true);
            Swatch swatch = obj.AddComponent<Swatch>();
            swatch.color = color;

            obj.transform.SetSiblingIndex(OptionsContainer.childCount - 2);

            Swatches.Add(swatch);
            return swatch;
        }


        public void AssignColor(Color color)
        {
            if(District.DistrictBuildingData.ContainsKey(this.currentBuilding.guid))
                currentColors = District.DistrictBuildingData[this.currentBuilding.guid];
            else
                currentColors = new Vector3[this.currentBuilding.fullMaterial.Count];

            currentColors[this.dropdown.value] = ColorConversions.ConvertColorToVector3(color);

            this.ApplyShader(this.currentBuilding.fullMaterial[this.dropdown.value]);
            this.currentBuilding.fullMaterial[this.dropdown.value].color = color;
            this.currentBuilding.useSharedMaterialIfPossible = false;
            AssignColorEvent(color);

            District.DistrictBuildingData[this.currentBuilding.guid] = this.currentColors;

            this.currentBuilding.UpdateMaterialSelection();
        }

        private void ApplyShader(Material material)
        {
            if (material.shader.name != "Custom/DistrictBuilding"){
                material.shader = AssetsLoader.GetAsset("DistrictBuilding") as Shader;
                material.SetFloat("_MinHeight", float.MinValue);
                material.SetFloat("_MaxHeight", float.MaxValue);
                material.SetFloat("_WallProgress", 1f);
                material.SetTexture("_MainTex", AssetsLoader.GetAsset("largehousetex_default") as Texture2D);
                material.SetTexture("_MaskTex", AssetsLoader.GetAsset("largehousetex_mask") as Texture2D);
                material.color = Color.white;
            }
        }


        private void SceneLoaded()
        {
            loadEvent = Broadcast.OnLoadedEvent.Listen(OnLoaded);
            saveEvent = Broadcast.OnSaveEvent.Listen(OnSave);
        }

        private void OnDestroy() {
            Broadcast.OnLoadedEvent.Unlisten(loadEvent);
            Broadcast.OnSaveEvent.Unlisten(saveEvent);
        }


        void LoadSwatches()
        {
            try{
                foreach(Vector3 vector in District.PlayerAddedColors.ToArray())
                {
                    Color color = ColorConversions.ConvertVector3ToColor(vector);
                    AddSwatch(color, SwatchType.Player);
                }
            }catch(Exception err)
            {
                Mod.helper.Log(err.ToString());
            }
        }

        void LoadBuildingColors()
        {
            foreach(KeyValuePair<Guid, Vector3[]> entry in District.DistrictBuildingData)
            {
                Building b = Player.inst.GetBuilding(entry.Key);
                if(b != null){
                    for(int i = 0; i < b.fullMaterial.Count; i++)
                    {
                        if(entry.Value[i] != null){
                            this.ApplyShader(b.fullMaterial[i]);
                            b.fullMaterial[i].color = ColorConversions.ConvertVector3ToColor(entry.Value[i]);
                            b.useSharedMaterialIfPossible = false;
                            b.UpdateMaterialSelection();
                        }

                    }
                }
            }
        }

        public class DistrictSave
        {
            public Dictionary<Guid, Vector3[]> DistrictBuildingData = new Dictionary<Guid, Vector3[]>();
            public Vector3[] PlayerAddedColors = new Vector3[32];
        }
        
        public void OnSave(object sender, OnSaveEvent saveEvent)
        {
            var saveObject = new DistrictSave(){
                DistrictBuildingData = District.DistrictBuildingData,
                PlayerAddedColors = District.PlayerAddedColors.ToArray()
            };
            LoadSave.SaveDataGeneric("DistrictModV2", "DistrictModV2Identifier", Newtonsoft.Json.JsonConvert.SerializeObject(saveObject));
        }

        public void OnLoaded(object sender, OnLoadedEvent loadedEvent)
        {
            try{
                foreach(Vector3 vector in District.PlayerAddedColors.ToArray())
                {
                    Color color = ColorConversions.ConvertVector3ToColor(vector);
                    Swatch swatch = District.Swatches.Find(i => i.color == color);
                    if(swatch != null){
                        District.Swatches.Remove(swatch);
                        Destroy(swatch.gameObject);
                    }
                }
            }catch(Exception err)
            {
                Mod.helper.Log(err.ToString());
            }

            string json = LoadSave.ReadDataGeneric("DistrictModV2", "DistrictModV2Identifier");
            if(json != null){
                var savedObject = Newtonsoft.Json.JsonConvert.DeserializeObject<DistrictSave>(json);
                District.DistrictBuildingData = savedObject.DistrictBuildingData;
                District.PlayerAddedColors = ColorConversions.ConvertArrayToList(savedObject.PlayerAddedColors);
            }else{Mod.helper.Log("JSON IS NULL");}

            LoadSwatches();
            LoadBuildingColors();
            
        }

    }
}