using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerProgressSystem
{
    public interface IPlayerProgressStorage
    {
        void SetString(string key, string value);
        string GetString(string key, string defaultValue = "");
        void SetInt(string key, int value);
        int GetInt(string key, int defaultValue = 0);
        bool HasKey(string key);
        void DeleteKey(string key);

        void DeleteAll();
        void Save();
    }
}