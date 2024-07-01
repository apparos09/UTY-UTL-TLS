using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // A selector for a button UI element.
    public class UISelectorButton : UISelectorElement
    {
        [Header("Element")]
        
        // The button for this UI element.
        public Button button;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Tries to auto grab the component.
            if(button == null)
                button = GetComponent<Button>();
        }

    }
}