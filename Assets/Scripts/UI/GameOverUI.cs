using UnityEngine;
using UnityEngine.UI;

public sealed class GameOverUI : UIPanelBase
{
    [SerializeField] Text _message;
    [SerializeField] Button _menuBtn;
    [SerializeField] Button _restartBtn;

    [SerializeField] string _winMessage;
    [SerializeField] string _loseMessage;

    private void Awake()
    {
        BaseInit();
    }

    private void Start()
    {
        _menuBtn.onClick.AddListener(UIController.Instance.ToMenu);
        _restartBtn.onClick.AddListener(UIController.Instance.RestartGame);

        UIController.Instance.OnGameOver += UpdateMessage;
    }

    private void UpdateMessage(bool win)
    {
        _message.text = win ? _winMessage : _loseMessage;
    }
}
