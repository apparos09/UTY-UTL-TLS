/*
 * References:
 *  - https://docs.unity3d.com/Packages/com.unity.textmeshpro@1.2/api/TMPro.TMP_Text.html
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace util
{
    // TODO: the text display jitters when characters are being added or removed.
    // This is because the game will always go one size over or under its intended size before...
    // The size is corrected. Disabling one of the two size changes (increase, decrease) gets rid of this...
    // But doing so prevents the resizing system from performing properly.
    // Not sure if you can fix this, as the text.isTextOverflowing function only gets updated after...
    // At least 1 frame has passed. Might be best to hide the text somehow while...
    // The text window is being resized.

    // Dynamically resizes a text object.
    // This is only available for TMP_Text since it has the ability to check if the text is overflowing.
    // NOTE: make sure the text is set to overflow.
    public class TMP_TextDynamicRectSize : MonoBehaviour
    {
        // The axis to be resized.
        // Horizontal Vertical: averages the size between the two axes.
        // Horizontal: expands size on the horizontal axis (width).
        // Vertical: expands size on the vertical axis (vertical).
        public enum resizeAxis { horizontalVertical, horizontal, vertical }

        // The text object.
        public TMP_Text text;

        // The saved text string, which is used to see when the text string has changed.
        private string savedText = null;

        // The text's rect transform.
        public RectTransform textRect;

        // The axis that the text will be resized on.
        // If resizing on horizontal and vertical, average out the size change.
        public resizeAxis axis;

        [Header("Size Change")]

        // Automatically resize the text rect.
        public bool autoRectResize = true;

        // The rect size change.
        // 0 = none, 1 = increase, -1 = decrease.
        private int rectSizeChange = 0;

        // The spacing used when increasing/decreasing the text rect size.
        [Tooltip("The size change incement when resizing the text rect.")]
        public int sizeInc = 10;

        [Header("Size Change/Decrease, Min")]

        // If true, the rect can decrease in size.
        [Tooltip("If true, the text rect can decrease in size.")]
        public bool sizeDecreaseEnabled = true;

        // The text rect size minimum.
        public Vector2 textRectSizeMin = Vector2.zero;

        // Uses the minimum size constraint for the text size.
        [Tooltip("Applies the minimum text rect size.")]
        public bool applyTextRectSizeMin = true;

        [Header("Size Change/Increase, Max")]

        // If true, the rect can increase in size.
        [Tooltip("If true, the text rect can increase in size.")]
        public bool sizeIncreaseEnabled = true;

        // The text rect size maximum.
        public Vector2 textRectSizeMax = new Vector2(200.0F, 200.0F);

        // Uses the maximum size constraint for the text size.
        [Tooltip("Applies the maximum text rect size.")]
        public bool applyTextRectSizeMax = true;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // By default this is 0 so that its applied in the first update.
            rectSizeChange = 0;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // If the text isn't set, grab the component.
            if(text == null)
                text = GetComponent<TMP_Text>();

            // If the text is set.
            if (text != null)
            {
                // If the text rect is null, get the component from the text.
                if (textRect == null)
                    textRect = text.GetComponent<RectTransform>();

                // The text isn't in overflow mode, so print a warning.
                if (text.overflowMode != TextOverflowModes.Overflow)
                {
                    Debug.LogWarning("The text isn't in overflow mode. This script may not work properly.");
                }
            }

            // Auto-resizing will happen in Update.
        }

        // Called when the text string has changed.
        protected virtual void OnTextChanged()
        {
            // Saves the text.
            savedText = text.text;

            // Set to 0 in case it may need to be resized.
            rectSizeChange = 0;
        }

        // Resizes the text rect.
        public void ResizeTextRect()
        {
            // Gets the text rect size before any changes.
            Vector2 textRectSizeOld = textRect.sizeDelta;

            // The new size of the text, which starts the same as theo ld size.
            Vector2 textRectSizeNew = textRectSizeOld;

            // The adjusted size increment change.
            int sizeIncAdj = sizeInc;

            // If the size increment is less than or equal to 0, default it to 1.
            if(sizeIncAdj <= 0)
            {
                Debug.LogWarning("The size increment is 0 or less. Cannot resize text rect.");
                return;
            }

            // The size increment vector.
            Vector2 sizeIncVec;

            // Checks what axis will be used to know what size increment to set.
            switch (axis)
            {
                // Average out the size along the horizontal and the vertical.
                default:
                case resizeAxis.horizontalVertical: // Horizontal-Vertical
                    sizeIncVec = new Vector2(sizeIncAdj / 2.0F, sizeIncAdj / 2.0F);
                    break;

                case resizeAxis.horizontal: // Horizontal
                    sizeIncVec = new Vector2(sizeIncAdj, 0.0F);
                    break;

                case resizeAxis.vertical: // Vertical
                    sizeIncVec = new Vector2(0.0F, sizeIncAdj);
                    break;
            }

            // NOTE: it appears that the text.isTextOverflowing doesn't update until...
            // A frame or two have passed, rather than immediately after the size has been changed.
            // As such, the checks in this function don't use loops, as they rely on the...
            // Text overflow parameter to be updated.
            // Just keep calling this function every time the size needs to be updated.

            // If the text is overflowing, make the display bigger.
            if (sizeIncreaseEnabled && text.isTextOverflowing)
            {
                // Set to the current size delta.
                textRectSizeNew = textRect.sizeDelta;

                // Increase the text rect size.
                textRect.sizeDelta += sizeIncVec;

                // If the text maximum should be applied.
                if (applyTextRectSizeMax)
                {
                    // Clamped size delta.
                    Vector2 clampedSize = textRect.sizeDelta;

                    // Bounds check for maximum x.
                    if (textRect.sizeDelta.x > textRectSizeMax.x)
                    {
                        // Apply the maximum size. 
                        clampedSize.x = textRectSizeMax.x;
                    }

                    // Bounds check for maximum y.
                    if (textRect.sizeDelta.y > textRectSizeMax.y)
                    {
                        // Apply the maximum size.
                        clampedSize.y = textRectSizeMax.y;
                    }

                    // Sets the new size.
                    textRect.sizeDelta = clampedSize;
                }

                // Overwrite with the new size.
                textRectSizeNew = textRect.sizeDelta;

                // Mark that a size increase ocurred.
                rectSizeChange = 1;
            }
            // If the text isn't overflowing, and a size increase didn't recently occur, make the display smaller.
            else if(sizeDecreaseEnabled && !text.isTextOverflowing && rectSizeChange != 1) 
            {
                // Set to the current size delta.
                textRectSizeNew = textRect.sizeDelta;

                // The reference size.
                Vector2 refSize = textRect.sizeDelta - sizeIncVec;

                // The clamped minimum size.
                Vector2 clampedSize = textRect.sizeDelta;

                // Decrease the text rect size if it wouldn't go below the minimum x.
                if(refSize.x >= textRectSizeMin.x)
                {
                    clampedSize.x = refSize.x;
                }

                // Decrease the text rect size if it wouldn't go below the minimum y.
                if (refSize.y >= textRectSizeMin.y)
                {
                    clampedSize.y = refSize.y;
                }

                // Set to the clamped size, and update new size.
                textRect.sizeDelta = clampedSize;

                // Overwrite with the new size.
                textRectSizeNew = textRect.sizeDelta;

                // The rect size has had a negative change.
                rectSizeChange = -1;
            }

            // If the text rect size delta isn't equal to the new size...
            // Set it to the new size.
            if(textRect.sizeDelta != textRectSizeNew)
            {
                textRect.sizeDelta = textRectSizeNew;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the text string isn't equal to the saved text, call the related function.
            if(text.text != savedText)
            {
                OnTextChanged();
            }

            // Automatically resizes the text rect.
            if(autoRectResize)
            {
                ResizeTextRect();
            }
        }
    }
}