﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJamGame16.Translation
{
    public class Translate
    {
        private readonly string[] _languages =
        {
            "english",
            "french"
        };

        public static string FixFrench(string str)
        {
            return str
                .Replace('ç', 'c')
                .Replace('â', 'e')
                .Replace('à', 'a')
                .Replace('ê', 'e')
                .Replace('è', 'e')
                .Replace('é', 'e');
        }

        private Translate()
        {
            foreach (var lang in _languages)
            {
                _translationData.Add(lang, JsonConvert.DeserializeObject<Dictionary<string, string>>(Resources.Load<TextAsset>(lang).text));
            }
        }

        private static Translate _instance;
        public static Translate Instance
        {
            private set => _instance = value;
            get
            {
                _instance ??= new Translate();
                return _instance;
            }
        }

        public bool Exists(string key) => _translationData["english"].ContainsKey(key);

        public string Tr(string key, params string[] arguments)
        {
            var langData = _translationData[_currentLanguage];
            string sentence;
            if (langData.ContainsKey(key))
            {
                sentence = langData[key];
            }
            else
            {
                sentence = _translationData["english"][key];
            }
            for (int i = 0; i < arguments.Length; i++)
            {
                sentence = sentence.Replace("{" + i + "}", arguments[i]);
            }
            return FixFrench(sentence);
        }

        private string _currentLanguage = "english";
        public string CurrentLanguage
        {
            set
            {
                if (!_translationData.ContainsKey(value))
                {
                    throw new ArgumentException($"Invalid translation key {value}", nameof(value));
                }
                _currentLanguage = value;
                foreach (var tt in UnityEngine.Object.FindObjectsOfType<TMP_TextTranslate>())
                {
                    tt.UpdateText();
                }
            }
            get => _currentLanguage;
        }

        private readonly Dictionary<string, Dictionary<string, string>> _translationData = new();
    }
}