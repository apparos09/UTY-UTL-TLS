using UnityEngine;

namespace util
{
    // An audio dictionary that reads in information from a file.
    // NOTE: this file hasn't been tested.
    public abstract class AudioDictionaryFileReader : AudioDictionary
    {
        // Reads lines from a file.
        public FileReaderLines fileReaderLines;

        // If 'true', a file reader is added if none is set on start.
        [Tooltip("Adds FileReaderLines component to this object if it doesn't exist on Start().")]
        public bool addFileReaderIfNotSet = true;

        // If 'true', the file is read on start if one exists.
        [Tooltip("Reads the file in the Start() function if true.")]
        public bool readFileOnStart = true;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            // If not set...
            if(fileReaderLines == null)
            {
                // Tries to get the file reader for this audio dictionary.
                if(!TryGetComponent(out fileReaderLines))
                {
                    // Debug.LogWarning("No FileReaderLines has been set. Adding FileReaderLines component.");
                    fileReaderLines = gameObject.AddComponent<FileReaderLines>();
                }
            }

            // If the file should be read.
            if(readFileOnStart)
            {
                TryReadFile();
            }
        }

        // Tries to load the contents from the file.
        public bool TryReadFile(bool loadContents = true)
        {
            // If the file reader exists.
            if(fileReaderLines != null)
            {
                // If the file exists, read from it.
                if(fileReaderLines.FileExists())
                {
                    // Reads the file.
                    fileReaderLines.ReadFile();

                    // Loads the file contents if requested.
                    if(loadContents)
                    {
                        LoadFileContents();
                    }

                    // File was read successfully.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Called to load the file contents into the dictionary.
        // Each line should be its own entry, so presumably the information will be split...
        // By some character, likely a tab or a comma.
        public abstract void LoadFileContents();

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}