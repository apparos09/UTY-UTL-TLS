using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace util
{
    // Sets the event manager's selectable automatically.
    public class EventSystemAutoSelect : MonoBehaviour
    {
        // The event system.
        public EventSystem eventSystem;

        // The first object selected.
        public GameObject firstSelected;

        // The last object selected.
        public GameObject lastSelected;

        // Sets the first select to the current object...
        // When the window is disabled.
        [Tooltip("Sets the current object to the last selected object on enable. If false, the first object is used.")]
        public bool setToLastOnEnable = true;

        // If 'true', lastSelected ignores deselections.
        // NOTE: to turn off deselecting by clicking on the background the InputSystemUIInputModule script must be used.
        // This is part of an InputSystem package that can be included from Unity.
        [Tooltip("Has lastSelected ignore deselections of the UI if true.")]
        public bool ignoreDeselectsForLast = false;

        // Start is called before the first frame update
        void Start()
        {
            // Sets this to the default selected element.
            eventSystem.firstSelectedGameObject = firstSelected;
        }

        // This function is called when the object becomes enabled and active.
        private void OnEnable()
        {
            // Refreshes the first selected object.
            eventSystem.firstSelectedGameObject = firstSelected;

            // Checks if the last selected object should be used...
            // Or the first selected object. Only works if last selected is set.
            if (setToLastOnEnable && lastSelected != null)
                SetSelectedGameObjectToLastSelected();
            else
                SetSelectedGameObjectToFirstSelected();
        }

        // // This function is called when the behavior becomes disabled or inactive
        // private void OnDisable()
        // {
        //     
        // }

        // Sets the event system's selected game object to the first selected object.
        public void SetSelectedGameObjectToFirstSelected()
        {
            eventSystem.SetSelectedGameObject(firstSelected);
        }

        // Sets the event system's selected game object to the last selected object.
        public void SetSelectedGameObjectToLastSelected()
        {
            eventSystem.SetSelectedGameObject(lastSelected);
        }

        // Update is called once per frame
        private void Update()
        {
            // NOTE: this can't be put in OnDisable because the event system's selected object...
            // Has already been changed by then.

            // If the last selected does not match the currently selected game object.
            if (lastSelected != eventSystem.currentSelectedGameObject)
            {
                // Checks if the lastSelected object should ignore deselections.
                if(ignoreDeselectsForLast) // Ignore deselect.
                {
                    // If the current selected object is not equal to null, save it as the last selected.
                    if(eventSystem.currentSelectedGameObject != null)
                    {
                        lastSelected = eventSystem.currentSelectedGameObject;
                    }
                }
                else // Include deselect.
                {
                    lastSelected = eventSystem.currentSelectedGameObject;
                }
            }

        }

    }
}