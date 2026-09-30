using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 0.5f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        fadePanel.alpha = 1f;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            fadePanel.alpha =
                Mathf.Lerp(1f, 0f, timer / fadeDuration);

            yield return null;
        }

        fadePanel.alpha = 0f;
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        fadePanel.alpha = 0f;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            fadePanel.alpha =
                Mathf.Lerp(0f, 1f, timer / fadeDuration);

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}