using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IUICommunication
{
    public static UIController Instance { get; set; }

    [SerializeField] UIPanelBase _menuPanel;
    [SerializeField] UIPanelBase _gamePanel;
    [SerializeField] UIPanelBase _pausePanel;
    [SerializeField] UIPanelBase _gameOverPanel;

    [SerializeField] ValidationPopup _validationPopup;

    private Dictionary<UIPanel, ICanvasGroupUI> _uiPanels;
    private Action _currValidationAction;

    private IUICommunication _gameController;

    public event Action<bool> OnGameOver;
    public event Action<int> OnScoresUpdate;

    private void Awake()
    {
        Instance = this;

        _uiPanels = new Dictionary<UIPanel, ICanvasGroupUI>();
        _uiPanels.Add(UIPanel.Menu, _menuPanel);
        _uiPanels.Add(UIPanel.Game, _gamePanel);
        _uiPanels.Add(UIPanel.Pause, _pausePanel);
        _uiPanels.Add(UIPanel.GameOver, _gameOverPanel);
    }

    private void Start()
    {
        _validationPopup.Hide(true);
        _gameController = GameController.Instance;
        SwitchUI(UIPanel.Menu);
    }

    public void SwitchUI(UIPanel panel)
    {
        foreach (var p in _uiPanels)
        {
            p.Value.Hide(true);
        }

        _uiPanels[panel].Hide(false);
    }

    public void StartGame()
    {
        SwitchUI(UIPanel.Game);
        _gameController.StartGame();
    }

    public void PauseGame()
    {
        SwitchUI(UIPanel.Pause);
        _gameController.PauseGame();
    }

    public void ToMenu()
    {
        SwitchUI(UIPanel.Menu);
        _gameController.ToMenu();
    }

    public void RestartGame()
    {
        SwitchUI(UIPanel.Game);
        _gameController.RestartGame();
    }

    public void ResumeGame()
    {
        SwitchUI(UIPanel.Game);
        _gameController.ResumeGame();
    }

    public void GameOver(bool win)
    {
        SwitchUI(UIPanel.GameOver);
        _gameController.PauseGame();
        OnGameOver?.Invoke(win);
    }

    public void ValidateClick(Action action, string message)
    {
        _validationPopup.gameObject.SetActive(true);
        _currValidationAction = action;
        _validationPopup.InitPopup(message);
        _validationPopup.ValidationResult += Validate;
    }

    private void Validate(bool result)
    {
        _validationPopup.ValidationResult -= Validate;
        _validationPopup.gameObject.SetActive(false);
        _currValidationAction?.Invoke();
        _validationPopup.Hide(true);
    }
    public void UpdateScores(int scores)
    {
        OnScoresUpdate?.Invoke(scores);
    }
}

public enum UIPanel
{
    Menu = 0, 
    Game = 1,
    Pause = 2,
    GameOver = 3,
}
