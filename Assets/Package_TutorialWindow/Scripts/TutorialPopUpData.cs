using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TutorialPopUpData", menuName = "PopUp/Tutorial Data")]
public class TutorialPopUpData : ScriptableObject
{
    public string heading;          // The heading text for the tutorial
    public VideoClip tutorialVideo; // The video clip for the tutorial
    [TextArea] public string description; // The description text
}
