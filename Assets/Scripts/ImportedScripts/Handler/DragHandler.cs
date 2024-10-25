using UnityEngine;
using UnityEngine.EventSystems;

namespace ezygamers.dragndropv1
{
    //This class handle the drag logic of the GameObject
    //to the strategy defined by IDragHandler
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
       
        //it used for handling the drag operations 
        public IDragStrategy dragStrategy;

        //if the gamobject is UI element or Not
        [SerializeField] bool isUI;

        //this holds the initial position of the draggable object
        // Store the original position
        private Vector3 originalPosition;
        private RectTransform rectTransform;

        //fields related to making the object POP
        [SerializeField] private bool toPop;
        public Vector3 targetScale = new Vector3(1.02f, 1.02f, 1.02f);
        public float pulseSpeed = 0.2f;

        private void Start()
        {

            if (isUI)
            {
                //create a new instance of UIDragFactory
                DragStrategyFactory factory= new UIDragFactory();
                //use the factory to create and assign a UIDragStrategy to dragStrategy
                dragStrategy = factory.CreateDraggable(this.gameObject);
                rectTransform = GetComponent<RectTransform>();
            }

            //TODO:create the strategy for Non UI Gameobject in else block

            if (toPop)
            {
                AnimationHelper.StartPulse(gameObject, targetScale, pulseSpeed);
            }
        }

        //when usen begins dragging a gameobject
        //if dragStrategy is assigned.
        public void OnBeginDrag(PointerEventData eventData)
        {
            AnimationHelper.StopPulse(gameObject);
            // Store the original position before drag starts
            if (isUI)
            {
                originalPosition = rectTransform.anchoredPosition3D;
            }
            else
            {
                originalPosition = transform.position;
            }
            dragStrategy?.OnBeginDrag(eventData);
        }


        //when user is dragging the GameObject
        //if dragStrategy is assigned.
        public void OnDrag(PointerEventData eventData) => dragStrategy?.OnDrag(eventData);
        //when the user stops dragging the GameObject
        //if dragStrategy is assigned.
        public void OnEndDrag(PointerEventData eventData)
        {
            dragStrategy?.OnEndDrag(eventData);

            // Return to original position
            if (isUI)
            {
                // For UI elements, use LeanTween to smoothly animate back to original position
                LeanTween.value(gameObject, rectTransform.anchoredPosition3D, originalPosition, 0.5f)
                         .setOnUpdate((Vector3 pos) => {rectTransform.anchoredPosition3D = pos;});
            }
            else
            {
                // For non-UI elements, use LeanTween to smoothly animate back to original position
                LeanTween.move(gameObject, originalPosition, 0.5f);
            }

            if (toPop)
            {
                AnimationHelper.StartPulse(gameObject, targetScale, pulseSpeed);
            }


        }
    }

}