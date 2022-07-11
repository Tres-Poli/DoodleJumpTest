using UnityEngine;
using UnityEngine.UI;

public sealed class GameUI : UIPanelBase
{
    [SerializeField] Button _pauseBtn;
    [SerializeField] Text _scores;

    private void Awake()
    {
        BaseInit();
        _scores.text = 0.ToString();
    }

    private void Start()
    {
        _pauseBtn.onClick.AddListener(UIController.Instance.PauseGame);
        UIController.Instance.OnScoresUpdate += UpdateScores;
    }

    private void UpdateScores(int scores)
    {
        _scores.text = scores.ToString();
    }
}
