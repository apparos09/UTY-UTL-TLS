using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // A keyboard UI element.
    // TODO: clicking on an element with the mouse doesn't switch the selected element. Try to fix that.
    public class UISelectorElement : MonoBehaviour
    {
        // The selector.
        public UISelector selector;

        // The selectable.
        public Selectable selectable;

        // The selectable's image.
        public Image uiImage;

        [Header("Colors")]
        // If 'true', the colours for the button are automatically set.
        public bool autoSetColors = true;

        // The normal and highlighted colours.
        public Color normalColor;
        public Color highlightedColor;

        [Header("Top")]
        public UISelectorElement topLeftElement;
        public UISelectorElement topMiddleElement;
        public UISelectorElement topRightElement;

        [Header("Middle")]
        public UISelectorElement middleLeftElement;
        public UISelectorElement middleRightElement;

        [Header("Bottom")]
        public UISelectorElement bottomLeftElement;
        public UISelectorElement bottomMiddleElement;
        public UISelectorElement bottomRightElement;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            // Tries to get the component.
            if(selectable == null)
                selectable = GetComponent<Selectable>();

            // If the UI image has not been set.
            if(uiImage == null)
            {
                // Tries to grab the component.
                if(!TryGetComponent<Image>(out uiImage))
                {
                    // Sets to selectable if failed.
                    uiImage = selectable.image;
                }
            }

            // If colours should be automatically set.
            if (autoSetColors)
            {
                normalColor = selectable.colors.normalColor;
                highlightedColor = selectable.colors.highlightedColor;
            }
        }

        // Selects a button based on the provided directions. If null is returned, no button exists at that location.
        public UISelectorElement GetElement(int xDirec, int yDirec)
        {
            // The element array.
            // NOTE: row 0 is the bottom row, row 1 is the middle row, and row 2 is the top row.
            // It's designed this way so that the directional input makes sense (e.g., +1 y means go up).
            UISelectorElement[,] elementArr = {
            { bottomLeftElement, bottomMiddleElement, bottomRightElement },
            { middleLeftElement, this, middleRightElement },
            { topLeftElement, topMiddleElement, topRightElement}
            };

            // The index (original), which is the middle row and column.
            int midRow = 1;
            int midCol = 1;

            // The selected index (clamped in a [-1, 1] range).
            // (X) is for columns, and (Y) is for rows
            int selRow = midRow + Mathf.Clamp(yDirec, -1, 1);
            int selCol = midCol + Mathf.Clamp(xDirec, -1, 1);

            // Grabs the element.
            UISelectorElement element = elementArr[selRow, selCol];

            // Returns the element.
            return element;
        }

        // Called when an element has been highlighted, but not selected.
        public virtual void HighlightElement()
        {
            uiImage.color = highlightedColor;
        }

        // Called when an element has been unhilighted.
        public virtual void UnhighlightElement()
        {
            uiImage.color = normalColor;
        }

        // Triggers the UI element.
        public virtual void TriggerElement()
        {
            selectable.Select();
        }
    }
}