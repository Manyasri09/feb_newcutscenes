using UnityEngine;

namespace ezygamers.cmsv1
{
    [System.Serializable]
    public class AudioContent
    {
        public AudioClip audioClip;

        public AudioContent(AudioClip audioClip)
        {
            this.audioClip = audioClip;
        }
    }

}
