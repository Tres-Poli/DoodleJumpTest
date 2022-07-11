using UnityEngine;
using UnityEngine.UI;

public sealed class MenuUI : UIPanelBase
{
    [SerializeField] Button _startGameButton;

    private void Awake()
    {
        BaseInit();
    }

    private void Start()
    {
        _startGameButton.onClick.AddListener(UIController.Instance.StartGame);
    }
} 
