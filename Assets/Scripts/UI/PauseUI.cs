using UnityEngine;
using UnityEngine.UI;

public sealed class PauseUI : UIPanelBase
{
    [SerializeField] Button _menuBtn;
    [SerializeField] Button _restartBtn;
    [SerializeField] Button _returnBtn;

    [SerializeField] string _toMenuMessage;
    [SerializeField] string _restartMessage;

    private void Awake()
    {
        BaseInit();
    }

    private void Start()
    {
        _menuBtn.onClick.AddListener(() => UIController.Instance.ValidateClick(UIController.Instance.ToMenu, _toMenuMessage));
        _restartBtn.onClick.AddListener(() => UIController.Instance.ValidateClick(UIController.Instance.RestartGame, _restartMessage));
        _returnBtn.onClick.AddListener(UIController.Instance.ResumeGame);
    }
}
