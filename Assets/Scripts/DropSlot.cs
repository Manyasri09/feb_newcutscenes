
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class DropSlot : MonoBehaviour, IDropHandler
{
   
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            string word = eventData.pointerDrag.GetComponentInChildren<TextMeshProUGUI>().text;
            Actions.answercheck?.Invoke(word);
        }
    }
}
