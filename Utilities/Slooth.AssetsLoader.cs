using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

using Assets;
using Assets.Code;


namespace Slooth.KingdomAndCastles
{
    public static class AssetsLoader
    {
        // List of Asset Info
        public static List<AssetInfo> assets = new List<AssetInfo>();

        public static AssetInfo LoadAsset<T>(AssetBundle bundle, String path) where T : UnityEngine.Object
        {
            Type type = typeof(T);
            var asset = bundle.LoadAsset<T>(path);

            if(asset == null){
                throw new Exception("Can't have a null asset!");
            }
            AssetInfo assetInfo = new AssetInfo(path, type, asset);
            assets.Add(assetInfo);
            return assetInfo;
        }

        public static UnityEngine.Object GetAsset(string name)
        {
            UnityEngine.Object asset = assets.Find(i => i.name == name).asset;
            return asset;
        }

    }

    public class AssetInfo
    {
        public String name;
        public String extension;
        public String path;
        public Type type;
        public UnityEngine.Object asset;

        public AssetInfo(String path, Type type, UnityEngine.Object asset)
        {
            int slashIndex = path.LastIndexOf('/');
            
            String nameWithExtension = path.Substring(((slashIndex >= 0 ? slashIndex + 1 : 0)));
            String extension = nameWithExtension.Substring((nameWithExtension.LastIndexOf('.') + 1));
            String name = nameWithExtension.Substring(0, (nameWithExtension.Length - extension.Length) - 1);

            this.name = name;
            this.extension = extension;
            this.path = path;
            this.type = type;
            this.asset = asset;
        }

        public override String ToString()
        {
            return $"\n Name: {this.name}\n Extension: {this.extension}\n Path: {this.path}\n Type: {this.type.ToString()}\n Asset: {this.asset.ToString()}";
        }

    }

}


