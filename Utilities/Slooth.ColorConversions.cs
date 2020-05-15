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

namespace Slooth.KingdomAndCastles{
    
    public static class ColorConversions{

        public static Vector3 ConvertColorToVector3(Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static Color ConvertVector3ToColor(Vector3 vector)
        {
            return new Color(vector.x, vector.y, vector.z);
        }

        public static List<Vector3> ConvertArrayToList(Vector3[] array)
        {
            List<Vector3> list = new List<Vector3>(array.Length);

            for(int i = 0; i < array.Length; i++)
            {
                if(array[i] != null)
                    list.Add(array[i]);
            }

            return list;
        }


    }
}