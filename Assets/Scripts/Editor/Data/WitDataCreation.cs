﻿/*
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Globalization;
using System.Text.RegularExpressions;
using com.facebook.witai.data;
using UnityEditor;
using UnityEngine;

namespace com.facebook.witai.Data
{
    public class WitDataCreation
    {
        const string PATH_KEY = "Facebook::Wit::ValuePath";

        public static WitConfiguration FindDefaultWitConfig()
        {
            string[] guids = AssetDatabase.FindAssets("t:WitConfiguration");
            if(guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<WitConfiguration>(path);
            }

            return null;
        }

        #if !WIT_DISABLE_UI
        [MenuItem("Assets/Create/Wit/Add Wit to Scene")]
        #endif
        public static void AddWitToScene()
        {
            var witGo = new GameObject();
            witGo.name = "Wit";
            var wit = witGo.AddComponent<Wit>();
            wit.Configuration = FindDefaultWitConfig();
        }

        #if !WIT_DISABLE_UI
        [MenuItem("Assets/Create/Wit/Values/String Value")]
        #endif
        public static void WitStringValue()
        {
            CreateStringValue("");
        }

        public static WitStringValue CreateStringValue(string path)
        {
            var asset = ScriptableObject.CreateInstance<WitStringValue>();
            CreateValueAsset("Create String Value", path, asset);
            return asset;
        }

        #if !WIT_DISABLE_UI
        [MenuItem("Assets/Create/Wit/Values/Float Value")]
        #endif
        public static void WitFloatValue()
        {
            CreateFloatValue("");
        }

        public static WitFloatValue CreateFloatValue(string path)
        {
            var asset = ScriptableObject.CreateInstance<WitFloatValue>();
            CreateValueAsset("Create Float Value", path, asset);
            return asset;
        }

        #if !WIT_DISABLE_UI
        [MenuItem("Assets/Create/Wit/Values/Int Value")]
        #endif
        public static void WitIntValue()
        {
            CreateStringValue("");
        }

        public static WitIntValue CreateIntValue(string path)
        {
            var asset = ScriptableObject.CreateInstance<WitIntValue>();
            CreateValueAsset("Create Int Value", path, asset);
            return asset;
        }

        private static void CreateValueAsset(string label, string path, WitValue asset)
        {
            asset.path = path;
            var saveDir = EditorPrefs.GetString(PATH_KEY, Application.dataPath);
            string name;

            if (!string.IsNullOrEmpty(path))
            {
                name = Regex.Replace(path, @"\[[\]0-9]+", "");
                name = name.Replace(".", " ");
                name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
            }
            else
            {
                name = asset.GetType().Name;
            }

            var filePath = EditorUtility.SaveFilePanel(label, saveDir, name, "asset");
            if (!string.IsNullOrEmpty(filePath))
            {
                EditorPrefs.SetString(PATH_KEY, filePath);
                if (filePath.StartsWith(Application.dataPath))
                {
                    filePath = filePath.Substring(Application.dataPath.Length - 6);
                }
                AssetDatabase.CreateAsset(asset, filePath);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
