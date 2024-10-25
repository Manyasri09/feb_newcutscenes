using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;


namespace ezygamers.dragndropv1
{
    public class DropHandler : MonoBehaviour, IDropHandler
    {
        public string OptionID; //holds the value of option ID from the OptionID of Question Data -rohan37kumar
        private CMSGameEventManager eventManager;
        //this holds the initial position of the draggable object
        //[SerializeField] private GameObject originalPos;

        [Inject]
        public void Construct(CMSGameEventManager eventManager)
        {
            this.eventManager = eventManager;
        }


        // This method is called when an object is dropped onto this GameObject
        public void OnDrop(PointerEventData eventData)
        {
            //retrieve the DragHandler component from the object being dragged
            DragHandler draggableHandler = eventData.pointerDrag?.GetComponent<DragHandler>();


            if (draggableHandler != null)
            {

                //Get the transform of the draggableHandler GameObject
                var draggedGameObject = draggableHandler.gameObject.transform;

                //Snapping back the object to original position
                //RectTransform originalRect = originalPos.GetComponent<RectTransform>();
                //Vector2 targetPos = originalRect.anchoredPosition;
                //draggedGameObject.transform.position = originalPos.transform.position;

                Debug.Log($"Item Dropped on: {gameObject.name}");

                //nudging this object to show dropped on this
                LeanTween.moveX(this.gameObject, transform.position.x + 0.1f, 0.05f)
                         .setEase(LeanTweenType.easeInOutSine)
                         .setLoopPingPong((int)(0.5f / 0.05f))
                         .setOnComplete(() => transform.position = transform.position);

                eventManager.OptionSelected(OptionID); //triggering the event -rohan37kumar
                //Debug.Log(OptionID);
                //Debug.Log("action called");
            }
        }

    }
}
