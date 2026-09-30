using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public SceneTransition sceneTransition;
    public void PlayGame()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}