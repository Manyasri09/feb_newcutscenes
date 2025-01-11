using UnityEngine;

public class QuestionMarkController : MonoBehaviour
{
    [Header("Question Mark Settings")]
    [Tooltip("The Question Mark GameObject to display above the character.")]
    public GameObject questionMark;

    [Tooltip("Offset position of the Question Mark relative to the character.")]
    public Vector3 questionMarkOffset = new Vector3(0, 2, 0);

    private void Start()
    {
        // Ensure the question mark is initially hidden
        if (questionMark != null)
        {
            questionMark.SetActive(false);
        }
    }

    /// <summary>
    /// Show the question mark sprite.
    /// </summary>
    public void ShowQuestionMark()
    {
        if (questionMark != null)
        {
            questionMark.SetActive(true);
            // Update the position if necessary
            questionMark.transform.localPosition = questionMarkOffset;
        }
    }

    /// <summary>
    /// Hide the question mark sprite.
    /// </summary>
    public void HideQuestionMark()
    {
        if (questionMark != null)
        {
            questionMark.SetActive(false);
        }
    }
}
