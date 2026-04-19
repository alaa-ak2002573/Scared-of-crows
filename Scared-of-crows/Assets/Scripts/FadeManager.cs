using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
    public Image fadePanel;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeOut());
    }

    public void FadeFromBlack()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}