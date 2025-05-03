using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour 
{
    public void LoadGameWithHUD() 
    {
        Debug.Log("LoadGameWithHUD method called."); // Debug log to confirm method invocation
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene() 
    {
        // Load Game scene (replaces current scene)
        AsyncOperation gameSceneLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        yield return new WaitUntil(() => gameSceneLoad.isDone);
        Debug.Log("Game scene loaded successfully.");
    }
}

