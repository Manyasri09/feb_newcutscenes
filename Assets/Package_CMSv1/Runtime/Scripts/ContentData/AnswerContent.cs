using UnityEngine;
namespace ezygamers.cmsv1
{
    [System.Serializable]
    public class AnswerContent
    {
        public Object content;  // Using UnityEngine.Object to accept any Unity object

        public AnswerContent(Object content)
        {
            this.content = content;
        }

    }
}