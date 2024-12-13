using UnityEngine;
using UnityEngine.UI;

public class HighlightImage : MonoBehaviour
{
    private bool isHighlighted = false;
    public Image borderImage;

    public void HighlightEnable()
    {
        if (!isHighlighted)
        {
            borderImage.gameObject.SetActive(true);
            isHighlighted = true;
        }
    }

    
}
