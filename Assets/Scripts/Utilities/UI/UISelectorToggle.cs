using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // A selector for a toggle UI element.
    public class UISelectorToggle : UISelectorElement
    {
        [Header("Element")]

        // The toggle for this UI element.
        public Toggle toggle;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Tries to auto grab the component.
            if (toggle == null)
                toggle = GetComponent<Toggle>();
        }


    }
}
