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
    public static class Utilities
    {
        
        ///<summary> A Recursive function that add all children typeof <c>Transform</c> underneath initial function call <pararef name="currentChild"/>.
        /// Required Parameter : <param name="currentChild"> Root or CurrentChild</param>
        /// Optional Parameter : <param name="list">List to Add to</param> - Default => <c> new List<Transform>(1)</c>, <param name="filter">Items to Include</param> - Default => <c>null</c>
        ///<returns> <c>List<Transform></c> of all children including parent.</returns> </summary>
        public static List<Transform> CollectChildren(Transform currentChild, List<Transform> list = null, List<String> filter = null)
        {
            if(currentChild == null)
                return null;

            if(list == null){
                list = new List<Transform>(1);
            }

            if(filter == null){
                list.Add(currentChild);
            }else{
                if(filter.Contains(currentChild.name)){
                    list.Add(currentChild);
                }
            }

            foreach(Transform child in currentChild){
                list = CollectChildren(child, list, filter);
            }

            return list;
        }

        
    }
}