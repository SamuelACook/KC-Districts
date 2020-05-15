using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

using Assets;
using Assets.Code;

namespace Slooth.KingdomAndCastles.District
{
    public class Assets
    {
        private static readonly string BUNDLE_NAME = "districtbundle";
        private static AssetBundle assetBundle;

        public static List<Transform> District_UI_Children;

        public static void LoadAssets(KCModHelper _helper)
        {
            _helper.Log("-----------LOADING ASSETS------------");
            assetBundle = KCModHelper.LoadAssetBundle(_helper.modPath + "\\AssetBundle", BUNDLE_NAME);

            if(assetBundle == null)
                throw new Exception("Asset Bundle Can't Be Null");
            
            AssetsLoader.LoadAsset<GameObject>(assetBundle, "assets/workspace/District.Prefab");
            AssetsLoader.LoadAsset<Texture>(assetBundle, "assets/workspace/_Textures/largehousetex_default.png");
            AssetsLoader.LoadAsset<Texture>(assetBundle, "assets/workspace/_Textures/largehousetex_mask.png");
            AssetsLoader.LoadAsset<Shader>(assetBundle, "assets/workspace/DistrictBuilding.shader");
            AssetsLoader.LoadAsset<ComputeShader>(assetBundle, "assets/workspace/GenerateSVTexture.compute");

            foreach(AssetInfo info in AssetsLoader.assets)
                _helper.Log(info.ToString());
            _helper.Log("-----------ASSETS LOADED------------");
        }


        public static void SetupAssets(KCModHelper _helper)
        {
            _helper.Log("-----------HOOKING UP ASSETS------------");
            
            // Setup District UI
            GameObject District_UI_GameObject = GameObject.Instantiate(AssetsLoader.GetAsset("District") as GameObject);
            Transform Worker_UI = GameState.inst.playingMode.GameUIParent.transform.Find("WorkerUICanvas").Find("WorkerUI");
            District_UI_GameObject.transform.SetParent(Worker_UI, false);

            // Collect Specific Children In District UI
            List<String> filter = new List<String>(new String[] {
                "District(Clone)", "Expand", "CreateSwatch", "ToggleSwatchPicker", 
                "Dropdown", "Options", "Option", "ContentBar", "Picker", "Fill", 
                "InputField", "BoxSlider", "Hue", "Background", "ModelPreview", 
                "ToggleDeleteMode"});
            District_UI_Children = Utilities.CollectChildren(District_UI_GameObject.transform, null, filter);

            GameObject Option_Prefab = District_UI_Children.Find(i => i.name == "Option").gameObject;
            AssetInfo option_Info = new AssetInfo("Option.Prefab", Option_Prefab.GetType(), Option_Prefab);
            AssetsLoader.assets.Add(option_Info);
            _helper.Log(option_Info.ToString());


            // Mod Components
            District district_Component = District_UI_GameObject.AddComponent<District>();
            Expand expand_Component = District_UI_Children.Find(i => i.name == "Expand").gameObject.AddComponent<Expand>();
            CreateSwatch createSwatch_Component = District_UI_Children.Find(i => i.name == "CreateSwatch").gameObject.AddComponent<CreateSwatch>();
            ModelPreview modelPreview_Component = District_UI_Children.Find(i => i.name == "ModelPreview").gameObject.AddComponent<ModelPreview>();
            ToggleSwatchPicker toggleSwatchPicker_Component = District_UI_Children.Find(i => i.name == "ToggleSwatchPicker").gameObject.AddComponent<ToggleSwatchPicker>();
            ToggleDeleteMode toggleDeleteMode_Componenet = District_UI_Children.Find(i => i.name == "ToggleDeleteMode").gameObject.AddComponent<ToggleDeleteMode>();


            district_Component.AddSwatch(Color.white, District.SwatchType.Default);
            district_Component.AddSwatch(Color.blue, District.SwatchType.Default);
            district_Component.AddSwatch(Color.cyan, District.SwatchType.Default);
            district_Component.AddSwatch(Color.gray, District.SwatchType.Default);
            district_Component.AddSwatch(Color.green, District.SwatchType.Default);
            district_Component.AddSwatch(Color.magenta, District.SwatchType.Default);
            district_Component.AddSwatch(Color.red, District.SwatchType.Default);
            district_Component.AddSwatch(Color.yellow, District.SwatchType.Default);
            

            // Color Picker
            ColorPicker colorPicker_Component = District_UI_Children.Find(i => i.name == "Picker").gameObject.AddComponent<ColorPicker>();
            ColorImage colorImage_Component = District_UI_Children.Find(i => i.name == "Fill").gameObject.AddComponent<ColorImage>();
            colorImage_Component.picker = colorPicker_Component;
            HexColorField hexColorField_Component = District_UI_Children.Find(i => i.name == "InputField").gameObject.AddComponent<HexColorField>();
            hexColorField_Component.hsvpicker = colorPicker_Component;
            BoxSlider boxSlider_Component = District_UI_Children.Find(i => i.name == "BoxSlider").gameObject.AddComponent<BoxSlider>();
            boxSlider_Component.handleRect = boxSlider_Component.transform.Find("Handle Slide Area").Find("Handle").GetComponent<RectTransform>();
            SVBoxSlider sVBoxSlider = boxSlider_Component.gameObject.AddComponent<SVBoxSlider>();
            sVBoxSlider.picker = colorPicker_Component;
            ColorSlider colorSlider_Component = District_UI_Children.Find(i => i.name == "Hue").gameObject.AddComponent<ColorSlider>();
            colorSlider_Component.hsvpicker = colorPicker_Component;
            colorSlider_Component.type = ColorValues.Hue;
            ColorSliderImage colorSliderImage_Component = colorSlider_Component.transform.Find("Background").gameObject.AddComponent<ColorSliderImage>();
            colorSliderImage_Component.picker = colorPicker_Component;
            colorSliderImage_Component.type = ColorValues.Hue;
            colorSliderImage_Component.direction = Slider.Direction.BottomToTop;

            _helper.Log("-----------ASSETS HOOKED UP------------");
        }

        

    }
}