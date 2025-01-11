using UnityEngine;
namespace ezygamers.cmsv1
{
    [System.Serializable]
    public class ImageContent
    {

        public Sprite image;               // The image asset
        public ImageContent(Sprite image)
        {
            this.image = image;
        }
    }

}
