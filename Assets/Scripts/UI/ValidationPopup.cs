using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class ValidationPopup : MonoBehaviour, ICanvasGroupUI
{
    [SerializeField] Text _message;
    [SerializeField] Button _yesBtn;
    [SerializeField] Button _noBtn;

    private CanvasGroup _canvasGroup;

    public event Action<bool> ValidationResult;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _yesBtn.onClick.AddListener(() => Validate(true));
        _noBtn.onClick.AddListener(() => Validate(false));
    }

    private void Validate(bool result)
    {
        ValidationResult?.Invoke(result);
    }

    public void InitPopup(string message)
    {
        Hide(false);
        _message.text = message;
    }

    public void Hide(bool hide)
    {
        _canvasGroup.alpha = hide ? 0f : 1f;
        _canvasGroup.interactable = !hide;
        _canvasGroup.blocksRaycasts = !hide;
    }
}
