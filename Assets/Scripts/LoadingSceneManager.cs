using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    // Utility
    public util.AsyncSceneLoader loader;

    // The next scene.
    public string nextScene = "";

    // Start is called before the first frame update
    void Start()
    {
        // Loads the next scene.
        loader.LoadScene(nextScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
