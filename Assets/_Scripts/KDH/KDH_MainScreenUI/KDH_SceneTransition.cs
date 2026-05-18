using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class KDH_SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            fadeImage.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            StartCoroutine(FadeIn());
        }
    }

    public void GoToNextScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = timer / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 1f);

        float timer = 0f;
        float alpha = 1f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            alpha = 1f - (timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}