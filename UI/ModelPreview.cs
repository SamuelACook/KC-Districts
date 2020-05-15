using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Assets;
using Assets.Code;
using TMPro;

namespace Slooth.KingdomAndCastles.District{
    public class ModelPreview: MonoBehaviour
    {

        public Transform displayModel;
        public Material displayMaterial;
        public static ModelPreview inst;
        
        
        void Start()
        {

            ModelPreview.inst = this; 
            Building b = GameState.inst.GetPlaceableByUniqueName("tavern");
            
            Transform original;
            if (b.DisplayModel)
            {
                original = b.DisplayModel.transform;
            }
            else
            {
                original = b.transform.Find("Offset").GetChild(0);
            }

            Transform transform = UnityEngine.Object.Instantiate<Transform>(original);
            transform.transform.localScale = new Vector3(55f,55f,55f);
            int layer = LayerMask.NameToLayer("UI");
            foreach (Transform transform2 in Util.ComponentsInNodeAndAllDescendants<Transform>(transform.gameObject))
            {
                transform2.gameObject.layer = layer;
            }
            transform.SetParent(this.transform.Find("ModelDisplay"), false);
            foreach (MeshRenderer meshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
            {

                meshRenderer.material.shader = AssetsLoader.GetAsset("DistrictBuilding") as Shader;

                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                meshRenderer.receiveShadows = false;

                meshRenderer.material.SetFloat("_MinHeight", float.MinValue);
                meshRenderer.material.SetFloat("_MaxHeight", float.MaxValue);
                meshRenderer.material.SetFloat("_WallProgress", 1f);

                meshRenderer.material.SetTexture("_MainTex", AssetsLoader.GetAsset("largehousetex_default") as Texture2D);
                meshRenderer.material.SetTexture("_MaskTex", AssetsLoader.GetAsset("largehousetex_mask") as Texture2D);

            }

            this.displayModel = transform;
            this.displayMaterial = this.displayModel.GetComponent<MeshRenderer>().material;


            ColorPicker.inst.onValueChanged.AddListener(UpdateColor);
        }

        private void Update()
        {
            base.transform.GetChild(1).transform.localRotation = Quaternion.Euler(-30f, 0f, 0f);
            base.transform.GetChild(1).Rotate(Vector3.up, Time.unscaledTime * 45f);
        }

        private void UpdateColor(Color color)
        {
            this.displayMaterial.color = color;
        }

    }
}
