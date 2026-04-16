using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [Header("Fill Settings")]
    [SerializeField] private Image fillImage;

    [Header("Color Transition")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    public void SetProgress(float progress)
    {
        if (fillImage == null) return;
        progress = Mathf.Clamp01(progress);
        fillImage.fillAmount = progress;
        fillImage.color = Color.Lerp(startColor, endColor, progress);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ResetBar()
    {
        SetProgress(0f);
    }
}