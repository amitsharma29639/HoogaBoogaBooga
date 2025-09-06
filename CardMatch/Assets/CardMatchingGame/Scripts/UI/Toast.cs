using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;

public class Toast : MonoBehaviour
{
    public static Toast Instance { get; private set; }

    [Header("Toast UI")]
    [SerializeField] private CanvasGroup toastCanvasGroup; // Use CanvasGroup for fade
    [SerializeField] private TextMeshProUGUI toastText;               // The text to show
    [SerializeField] private float fadeDuration = 0.5f;    // Fade in/out speed

    private bool isShowing = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameObject.SetActive(true);

        if (toastCanvasGroup != null)
        {
            isShowing = false;
            toastCanvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Show a toast message (only if one is not already active).
    /// </summary>
    public void Show(string message)
    {
        Debug.Log("isshowing"+ isShowing);
        if (isShowing) return; // Block new toast if one is already showing
        ShowToast(message);
    }

    private void  ShowToast(string message)
    {
        isShowing = true;

        toastText.text = message;
        gameObject.SetActive(true);
        
        toastCanvasGroup.DOFade(1f, fadeDuration).OnComplete(async () =>
        {
            await Task.Delay(1000);
            toastCanvasGroup.DOFade(0f, fadeDuration ).OnComplete(() =>
            {
                toastCanvasGroup.gameObject.SetActive(false);
                isShowing = false;
            });
            
        });
        
    }
}