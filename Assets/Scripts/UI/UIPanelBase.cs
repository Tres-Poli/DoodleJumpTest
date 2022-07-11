using UnityEngine;

public abstract class UIPanelBase : MonoBehaviour, ICanvasGroupUI
{
    protected CanvasGroup _canvasGroup;

    public void BaseInit()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Hide(bool hide)
    {
        _canvasGroup.alpha = hide ? 0f : 1f;
        _canvasGroup.interactable = !hide;
        _canvasGroup.blocksRaycasts = !hide;
    }
}
