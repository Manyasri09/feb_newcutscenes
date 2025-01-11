using System;
using System.Collections;
using UnityEngine;

namespace ezygamers.cmsv1
{
    [System.Serializable]
    public abstract class  OptionData
    {
        public string optionID;
        public abstract Sprite GetSprite();
        public abstract string GetText();
        public abstract AudioClip GetAudioClip();
    }

}
