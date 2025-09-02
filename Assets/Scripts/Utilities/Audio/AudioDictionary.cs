/*
 * References:
 * - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2?view=net-9.0
 */
using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // Loads audio file information into a dictionary.
    // This can be used with a storage file to provide information on how loopable audio should be looped.
    public class AudioDictionary : MonoBehaviour
    {
        // Info struct
        // In practice, some elements likely won't be filled if they aren't used or if Unity doesn't have a default way to get them.
        public struct AudioInfo
        {
            // Valid to read information on this file.
            public bool valid;

            // The name of the audio file and its file extension.
            public string fileName;

            // The file type (extension) for the file.
            // NOTE: getting the name of a asset in Unity provides its name, but not its file extension.
            // Not sure if there's a way to get the file extension, but since Unity largely ignores this...
            // It has been left out.
            public string fileType;

            // The file path. It should also include the file name.
            public string filePath;

            // The length of the audio file (in seconds).
            public float length;

            // The start and end of the audio file's loop (in seconds).
            // If the file wasn't designed to loop, start should be 0 and end should be the audio length.
            public float loopStart;
            public float loopEnd;
        }

        // The dictionary for the audio file.
        public SortedDictionary<string, AudioInfo> dictionary = new SortedDictionary<string, AudioInfo>();

        // Awake is called when the script instance is being loaded
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            // ...
        }

        // Generates blank audio info object.
        public AudioInfo GenerateBlankAudioInfo()
        {
            AudioInfo info = new AudioInfo();

            // No data, so invalid to read from.
            info.valid = false;

            // The name of the audio file.
            info.fileName = "";
            info.fileType = "";
            info.filePath = "";

            // Length
            info.length = 0;

            // Loop
            info.loopStart = 0;
            info.loopEnd = 0;

            // Return theo bject.
            return info;
        }


        // // Update is called once per frame
        // protected virtual void Update()
        // {
        //     // ...
        // }
    }
}