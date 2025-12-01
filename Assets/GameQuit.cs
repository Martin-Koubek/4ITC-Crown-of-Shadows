using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameQuit : MonoBehaviour
{
    [SerializeField]private GameObject menu;

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        yield return null;
    }

    public void LoadLevel(string levelToLoad)
    { 
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }
}
