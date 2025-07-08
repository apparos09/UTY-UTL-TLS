using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

namespace util
{
    // The game data class, which is used to store game data for saving/loading.
    [System.Serializable]
    public class GameData
    {
        // Shows if the game data is valid.
        public bool valid = false;
    }

    // The save system class.
    public class SaveSystem : MonoBehaviour
    {
        // The instance of Save System
        private static SaveSystem instance;

        // Becomes 'true' when the save system is instanced.
        private static bool instanced = false;

        // If set to 'true', the game allows the player to save and load data.
        public bool allowSaveLoad = true;

        // Game Data
        // The last game save. This is only for testing purposes.
        public GameData lastSave;

        // The data that was loaded.
        public GameData loadedData;

        [Header("File Reader")]
        // The file reader (bytes)
        public FileReaderBytes fileReader = null;

        // TODO: implement a manager to get the save data from.
        // public GameplayManager gameManager;

        // The name of the file with the file extension. It overwrites the value in FileReader.
        [Tooltip("The file name. It overwrites the file name in FileReader. Make sure to include the file extension.")]
        public string fileName = "save.dat";

        // The primary file directory. I recommend using this when in the UnityEditor.
        [Tooltip("The primary file directory.")]
        public string filePath1 = "Assets\\Resources\\Data\\";

        // The secondary file directory. I recommend using this when running from a build folder.
        [Tooltip("The secondary file directory. This is used if the primary directory doesn't exist")]
        public string filePath2 = "Data\\";


        // FEEDBACK //
        // The timer for getting feedback from saving (uses unscaled time).
        private WaitForSecondsRealtime feedbackTimer = new WaitForSecondsRealtime(2);

        // The function being called to get feedback on the save progress.
        private Coroutine feedbackFunction;

        // An object that gets displayed when showing feedback on saving.
        public GameObject feedbackObject;
        
        // The text for providing feedback on the saving.
        public TMP_Text feedbackText;

        // The default saving data.
        private const string FEEDBACK_STRING_DEFAULT = "Saving Data";

        // The string shown when having save feedback.
        public string feedbackString = "Saving Data";

        // Becomes 'true' when a save is in progress.
        private bool saveInProgress = false;

        // Private constructor so that only one save system object exists.
        private SaveSystem()
        {
            // ...
        }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                Initialize();

                instanced = true;
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // Gets the instance.
        public static SaveSystem Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = instance = FindAnyObjectByType<SaveSystem>(FindObjectsInactive.Include);
                    

                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Save System (singleton)");
                        instance = go.AddComponent<SaveSystem>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Set save and load operations.
        public void Initialize()
        {
            // The result.
            bool result;

            // Checks if the file reader exists.
            if (fileReader == null)
            {
                // Tries to grab component.
                if (!TryGetComponent<FileReaderBytes>(out fileReader))
                {
                    // Add component.
                    fileReader = gameObject.AddComponent<FileReaderBytes>();
                }
            }

            // Set the file name.
            fileReader.fileName = fileName;

            // Checks if the primary file path exists.
            if (FileReader.FilePathExists(filePath1))
            {
                fileReader.filePath = filePath1;
            }
            // Checks if the secondary file path exists.
            else if (FileReader.FilePathExists(filePath2))
            {
                fileReader.filePath = filePath2;
            }
            // No file path exists, so generate the first file path.
            else
            {
                fileReader.filePath = filePath1; // Set path.
                fileReader.CreateFilePath(); // Create path.
            }                         

            // Checks if the file exists.
            result = fileReader.FileExists();

            // If the file exists, the save system checks if it's empty.
            if (result)
            {
                // If the file is empty, delete the file.
                bool empty = fileReader.IsFileEmpty();

                // If empty, delete the file.
                if (empty)
                    fileReader.DeleteFile();

            }

            // Save system has been instanced.
            instanced = true;
        }

        // Checks if the game manager has been set.
        private bool IsGameplayManagerSet()
        {
            // TODO: check that the game manager is set so that the game data can be pulled from it.

            return true;
        }

        // Sets the last bit of saved data to the loaded data object.
        public void SetLastSaveAsLoadedData()
        {
            loadedData = lastSave;
        }

        // Clears out the last save and the loaded data object.
        public void ClearLoadedAndLastSaveData(bool deleteFile)
        {
            lastSave = null;
            loadedData = null;


            // If the file should be deleted upon the last save being cleared.
            if (deleteFile)
            {
                // If the file exists, delete it.
                if (fileReader.FileExists())
                {
                    // Checks if a meta file exists so that that can be deleted too.
                    string meta = fileReader.GetFileWithPath() + ".meta";

                    // Deletes the main file.
                    fileReader.DeleteFile();

                    // If the meta file exists, delete that too.
                    if (File.Exists(meta))
                        File.Delete(meta);
                }
            }
        }

        // Converts an object to bytes (requires seralizable object) and returns it.
        static public byte[] SerializeObject(object data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, data); // Serialize the data for the memory stream.
            return ms.ToArray();
        }

        // Deserialize the provided data, converting it to an object and returning it.
        static public object DeserializeObject(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            ms.Write(data, 0, data.Length); // Write data.
            ms.Seek(0, 0); // Return to start of data.

            return bf.Deserialize(ms); // Return the content
        }

        // Checks if a save is in progress.
        public bool IsSaveInProgress()
        {
            return saveInProgress;
        }

        // Saves data.
        public bool SaveGame(bool async)
        {
            // TODO: replace this with a check for whatever class or object is expected to have the save data.
            // The game manager does not exist if false.
            // This is done because the game manager holds the save data.
            // if (!IsGameplayManagerSet())
            // {
            //     Debug.LogWarning("The GameplayManager object couldn't be found. Saving is unavailable.");
            //     return false;
            // }


            // TODO: uncomment this if you don't want the game to save if the file doesn't exist.
            // Checks that the file exists. If it doesn't exist, the save will fail.
            // if (!fileReader.FileExists())
            //     return false;

            // Determines if saving wa a success.
            bool success = false;

            // Generates the save data.
            // TODO: replace this line with a function call to get the save data.
            // GameData savedData = gameManager.GenerateSaveData();
            GameData savedData = null;

            // Stores the most recent save.
            lastSave = savedData;

            // Checks if save/load should be allowed.
            if (allowSaveLoad)
            {
                // Save to a file based on the providing conditions.
                if (async) // Asynchronous Save
                {
                    success = SaveToFileAsync(savedData);
                }
                else // Synchronous Save
                {
                    success = SaveToFileSync(savedData);
                }
            }
            else
            {
                success = false;
            }

            return success;
        }

        // Sets the feedback string to its default value.
        public void SetFeedbackStringToDefault()
        {
            feedbackString = FEEDBACK_STRING_DEFAULT;
        }

        // Refreshes the feedback elements.
        public void RefreshFeedbackElements()
        {
            // If the object exists, turn it on as needed.
            if(feedbackObject != null)
            {
                // If a save is in progress, turn the object on.
                // If no save is in progress, turn the object off.
                feedbackObject.SetActive(saveInProgress);
            }

            // If the text exists, set it.
            if (feedbackText != null)
            {
                // Checks if a save is in progress.
                // If it is, set it to the feedback string. If it's not, clear the text.
                if (saveInProgress)
                    feedbackText.text = feedbackString;
                else
                    feedbackText.text = string.Empty;
            }
        }

        // Save the information to a file syncrhonously.
        private bool SaveToFileSync(GameData data)
        {
            // Gets the file.
            string file = fileReader.GetFileWithPath();

            // Seralize the data.
            byte[] dataArr = SerializeObject(data);

            // If the data did not serialize properly, return false.
            if (dataArr.Length == 0)
                return false;

            // Save started.
            saveInProgress = true;

            // Write to the file.
            File.WriteAllBytes(file, dataArr);

            // Save finished.
            saveInProgress = false;

            // Data written successfully.
            return true;
        }

        // Saves the game asynchronously.
        public bool SaveToFileAsync(GameData data)
        {
            // Checks if the feedback method exists.
            if (feedbackFunction == null) // Save not in progress.
            {
                feedbackFunction = StartCoroutine(SaveToFileAsyncCourtine(data));
                return true;
            }
            else // The feedback method has been set, meaning the save is already in progress.
            {
                Debug.LogWarning("Save already in progress.");
                return false;
            }
        }

        // Save the information to a file asynchronously (cannot return anything).
        private IEnumerator SaveToFileAsyncCourtine(GameData data)
        {
            // Save started.
            saveInProgress = true;

            // Show saving text.
            RefreshFeedbackElements();

            // Gets the file.
            string file = fileReader.GetFileWithPath();

            // Seralize the data.
            byte[] dataArr = SerializeObject(data);

            // Yield return before file wrting begins.
            yield return null;

            // Show saving text in case the scene has changed.
            RefreshFeedbackElements();

            // Opens the file in the file stream.
            FileStream fs = File.OpenWrite(file);

            // NOTE: this is pretty scuffed, but because of the way it's set up I don't really have a better option.
            // FileStream.WriteAsync would probably be better.

            // Ver. 1 - write a fixed amount of bytes every frame.
            // // The number of bytes to write, and the offset.
            // int count = 32;
            // int offset = 0;

            // // While there's still bytes to write.
            // while(offset < dataArr.Length)
            // {
            //     // If the count exceeds the amount of remaining bytes, adjust it.
            //     if (offset + count > dataArr.Length)
            //         count = dataArr.Length - offset;
            // 
            //     fs.Write(dataArr, offset, count);
            // 
            //     // Increase the offset.
            //     offset += count;
            // 
            //     // Run other operations.
            //     // yield return null;
            // 
            //     // Pause the courtine for 2 seconds.
            //     yield return feedbackTimer;
            // }

            // Ver. 2 - write the data and suspend for the amount of time set to feedbackTimer.
            fs.Write(dataArr, 0, dataArr.Length);
            yield return feedbackTimer;

            // Show saving text in case scene has changed.
            RefreshFeedbackElements();

            // Close the file stream.
            fs.Close();

            // Save finished.
            saveInProgress = false;

            // Hide feedback text now that the save is done.
            RefreshFeedbackElements();

            // Save is complete, so set the method to null.
            if (feedbackFunction != null)
                feedbackFunction = null;
        }

        // Checks if the game has loaded data.
        public bool HasLoadedData()
        {
            // Used to see if the data is available.
            bool result;

            // Checks to see if the data exists.
            if (loadedData != null) // Exists.
            {
                // Checks to see if the data is valid.
                result = loadedData.valid;
            }
            else // No data.
            {
                // Not readable.
                result = false;
            }

            // Returns the result.
            return result;
        }

        // Removes the loaded data.
        public void ClearLoadedData()
        {
            loadedData = null;
        }

        // The gameplay manager now checks if there is loadedData. If so, then it will load in the data when the game starts.
        // Loads a saved game. This returns 'false' if there was no data.
        public bool LoadGame()
        {
            // Loading a save is not allowed.
            if (!allowSaveLoad)
                return false;

            // The result of loading the save data.
            bool success;

            // The file doesn't exist.
            if (!fileReader.FileExists())
            {
                return false;
            }

            // Loads the file.
            loadedData = LoadFromFile();

            // The data has been loaded successfully.
            success = loadedData != null;

            return success;
        }

        // Loads information from a file.
        private GameData LoadFromFile()
        {
            // Gets the file.
            string file = fileReader.GetFileWithPath();

            // Checks that the file exists.
            if (!fileReader.FileExists())
                return null;

            // Read from the file.
            byte[] dataArr = File.ReadAllBytes(file);

            // Data did not serialize properly.
            if (dataArr.Length == 0)
                return null;

            // Deseralize the data.
            object data = DeserializeObject(dataArr);

            // Convert to loaded data.
            GameData loadData = (GameData)(data);

            // Return loaded data.
            return loadData;
        }


    }
}