using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIFader : MonoBehaviour
{
    public static UIFader Instance { get; private set; }

    [Header("Fade Settings")] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        fadeImage = GetComponent<Image>();
        var color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fade in the image (transparent → opaque).
    /// </summary>
    public void FadeIn(Action onComplete = null)
    {
        if (fadeImage == null) return;

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);

        fadeImage.DOFade(1f, fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => { onComplete?.Invoke(); });
    }

    /// <summary>
    /// Fade out the image (opaque → transparent).
    /// </summary>
    public void FadeOut(Action onComplete = null)
    {
        if (fadeImage == null) return;

        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

        fadeImage.DOFade(0f, fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                fadeImage.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }
}