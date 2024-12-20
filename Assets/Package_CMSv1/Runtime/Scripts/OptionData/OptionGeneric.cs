using UnityEngine;

namespace ezygamers.cmsv1
{
    [System.Serializable]
    public class OptionGeneric : OptionData
    {
        public Sprite sprite;
        public string text;
        public AudioClip audio;

        public override AudioClip GetAudioClip()
        {
            return audio;
        }

        public override Sprite GetSprite()
        {
            return sprite;
        }

        public override string GetText()
        {
            return text;
        }
    }


}
