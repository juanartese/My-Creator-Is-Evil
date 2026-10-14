using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class AbilityNotificationUI : MonoBehaviour
{
    public static AbilityNotificationUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform bannerRect;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.35f;
    [SerializeField] private float displayDuration = 3.0f;
    [SerializeField] private float slideDistance = 30f;

    private Vector2 initialPosition;
    private Coroutine displayRoutine;

    private void Awake()
    {
        // Singleton pattern so any script can call it instantly
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (bannerRect == null) bannerRect = GetComponent<RectTransform>();

        initialPosition = bannerRect.anchoredPosition;
        canvasGroup.alpha = 0f;
    }

    public void ShowUnlockNotification(string title = "ABILITY UNLOCKED", string description = "Press [Space] in mid-air to Double Jump")
    {
        Debug.Log("<color=yellow>[UI BANNER]</color> Banner animation started!");

        if (titleText != null) titleText.text = title;
        if (promptText != null) promptText.text = description;

        if (displayRoutine != null)
        {
            StopCoroutine(displayRoutine);
        }

        displayRoutine = StartCoroutine(AnimateBanner());
    }

    private IEnumerator AnimateBanner()
    {
        Vector2 startPos = initialPosition - new Vector2(0, slideDistance);
        Vector2 targetPos = initialPosition;

        // Slide & Fade In
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float ease = 1f - Mathf.Pow(1f - t, 3);

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, ease);
            bannerRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, ease);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        bannerRect.anchoredPosition = targetPos;

        yield return new WaitForSecondsRealtime(displayDuration);

        // Slide & Fade Out
        elapsed = 0f;
        Vector2 endPos = targetPos + new Vector2(0, slideDistance * 0.5f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            bannerRect.anchoredPosition = Vector2.Lerp(targetPos, endPos, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        bannerRect.anchoredPosition = initialPosition;
        displayRoutine = null;
    }
}