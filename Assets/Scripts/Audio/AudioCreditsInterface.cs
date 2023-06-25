using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace util
{
    public class AudioCreditsInterface : MonoBehaviour
    {
        // The audio references object.
        public AudioCredits audioCredits;

        // The credit index for the audio reference.
        private int creditIndex = 0;

        // The user interface for the credits menu.
        [Header("UI")]

        // The title text.
        public TMP_Text titleText;

        // The text for the back button.
        public TMP_Text backButtonText;

        [Header("UI/Credit")]
        // The name of the song.
        public TMP_Text songTitleText;

        // The name of the artist(s).
        public TMP_Text artistsText;

        // The name of the album/group that the song comes from.
        public TMP_Text collectionText;

        // The source of the song, which will be a website most likely.
        public TMP_Text sourceText;

        // The link to the the song (website, website page, etc.). This is a link to the source you used.
        public TMP_Text link1Text;

        // The link to the the song (website, website page, etc.). This second link is for the orgination of the audio.
        public TMP_Text link2Text;

        // The text for the copyright information.
        public TMP_Text copyrightText;

        // The page number text, which is a fraction (000/000)
        public TMP_Text pageNumberText;



        // Start is called before the first frame update
        void Start()
        {
            // Loads credit and sets page number.
            UpdateCredit();
        }

        // Sets the index of the page.
        public void SetPageIndex(int newIndex)
        {
            // The reference count.
            int refCount = audioCredits.GetCreditCount();

            // No references to load.
            if (refCount == 0)
            {
                creditIndex = -1;
                return;
            }

            // Sets the new index, clamping it so that it's within the page count.
            creditIndex = Mathf.Clamp(newIndex, 0, refCount - 1);

            // Updates the displayed credit.
            UpdateCredit();
        }

        // Goes to the previous page.
        public void PreviousPage()
        {
            // Generates the new index.
            int newIndex = creditIndex - 1;

            // Goes to the end of the list.
            if (!audioCredits.IndexInBounds(newIndex))
                newIndex = audioCredits.GetCreditCount() - 1;

            SetPageIndex(newIndex);
        }

        // Goes to the next page.
        public void NextPage()
        {
            // Generates the new index.
            int newIndex = creditIndex + 1;

            // Goes to the start of the list.
            if (!audioCredits.IndexInBounds(newIndex))
                newIndex = 0;

            SetPageIndex(newIndex);
        }

        // Sets the page number text.
        public virtual void UpdatePageNumberText()
        {
            // Updates the page number.
            pageNumberText.text = (creditIndex + 1).ToString() + "/" + audioCredits.GetCreditCount().ToString();
        }

        // Updates the credit.
        public void UpdateCredit()
        {
            // No credit to update, or index out of bounds.
            if (audioCredits.GetCreditCount() == 0 || !audioCredits.IndexInBounds(creditIndex))
                return;

            // Gets the credit.
            AudioCredits.AudioCredit credit = audioCredits.audioCredits[creditIndex];

            // Updates all of the information.
            songTitleText.text = credit.title;
            artistsText.text = credit.artists;
            collectionText.text = credit.collection;
            sourceText.text = credit.source;
            link1Text.text = credit.link1;
            link2Text.text = credit.link2;
            copyrightText.text = credit.copyright;

            // Updates the page number.
            UpdatePageNumberText();
        }
    }
}