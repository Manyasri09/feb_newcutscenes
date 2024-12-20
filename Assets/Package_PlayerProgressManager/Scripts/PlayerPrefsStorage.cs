using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerProgressSystem
{
    public class PlayerPrefsStorage : IPlayerProgressStorage
    {
        public void SetString(string key, string value) => PlayerPrefs.SetString(key, value);
        public string GetString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);
        public void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        public int GetInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);
        public bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);
        public void DeleteAll() => PlayerPrefs.DeleteAll();
        public void Save() => PlayerPrefs.Save();
    }
}