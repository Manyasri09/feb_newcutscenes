using UnityEngine;

namespace ezygamers.cmsv1
{
    [System.Serializable]
    public class ImageOption : OptionData
    {
        public Sprite sprite;
        public string text;

        public override AudioClip GetAudioClip()
        {
            return null;
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
