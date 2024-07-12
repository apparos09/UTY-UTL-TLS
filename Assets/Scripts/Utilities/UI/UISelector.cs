using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // NOTE: this is a custom way to use a UI menu with the keyboard.
    // Unity has a built-in method for doing this, and that should be used instead.

    // A script used for selecting elements in the UI.
    public class UISelector : MonoBehaviour
    {
        // The selected button.
        public UISelectorElement selectedElement;

        // The key used to select the highlighted element.
        public KeyCode selectKey = KeyCode.Space;

        // If 'true', the default element is highlighted.
        [Tooltip("If true, the default selected element is highlighted.")]
        public bool highlightOnStart = true;

        // If 'true', inputs are allowed through the selector.
        public bool allowInput = true;

        // If 'true', inactive UI elements are also selected.
        [Tooltip("Include inactive elements when selecting elements.")]
        public bool includeInactive = true;

        // Start is called before the first frame update
        void Start()
        {
            // Highlight this element by default.
            if(selectedElement != null)
            {
                selectedElement.SelectElement();
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If input should be allowed, and an element is selected.
            if(allowInput && selectedElement != null)
            {
                // The x and y directions
                int xDirec = 0, yDirec = 0;

                // Ver. 1- automatically goes as far in a given direction as long as the button is held.
                // xDirec = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
                // yDirec = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

                // Ver. 2 - OnKeyDown only
                // TODO: having horizontal and vertical input at the same time doesn't work well. Fix that.

                // Horizontal
                if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) // Left
                {
                    xDirec = -1;
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow) ||  Input.GetKeyDown(KeyCode.D)) // Right
                {
                    xDirec = 1;
                }

                // Veritcal
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) // Up
                {
                    yDirec = 1;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) // Down
                {
                    yDirec = -1;
                }


                // If there are directional inputs.
                if (xDirec != 0 || yDirec != 0)
                {
                    // The new element.
                    UISelectorElement newElement = selectedElement.GetElement(xDirec, yDirec);

                    // If the selected element is not null, set it as the new element.
                    if(newElement != null) 
                    {
                        // Checks if inactive elements should be usable.
                        if((includeInactive && newElement.selectable.interactable) || 
                            (!includeInactive && newElement.isActiveAndEnabled && newElement.selectable.interactable))
                        {
                            selectedElement.UnhighlightElement();
                            selectedElement = newElement;
                            selectedElement.SelectElement();
                        }
                            
                    }
                }

                // If the player has selected the space bar.
                if(Input.GetKeyDown(selectKey))
                {
                    // If the selected element has been set.
                    if(selectedElement != null)
                    {
                        // Triggers the element.
                        selectedElement.TriggerElement();
                    }
                }
            }
        }
    }
}