using UnityEngine;

sealed class GameController : MonoBehaviour, IUICommunication
{
    public static GameController Instance { get; private set; }

    [SerializeField] float _offsetYFromPivotToRemove;
    [SerializeField] float _offsetYFromPlayerToGenerate;
    [SerializeField] float _lvlGoal;
    [SerializeField] Transform _gamePivot;

    [SerializeField] CameraFollower _cameraFollower;
    [SerializeField] PlayerController _playerController;
    [SerializeField] PlatformContainer _platformContainer;

    private IUICommunication _uiController;

    public float OffsetYFromPivotToRemove => _offsetYFromPivotToRemove;
    public Transform GamePivot => _gamePivot;
    public float LvlGoal => _lvlGoal;

    private void Awake()
    {
        Instance = this;

        _playerController.Hide(true);
        SetGameScriptsEnabled(false);
    }

    private void Start()
    {
        _uiController = UIController.Instance;
        _playerController.GameOver += GameOver;
    }

    private void SetGameScriptsEnabled(bool enabled)
    {
        _playerController.enabled = enabled;
        _cameraFollower.enabled = enabled;
        _platformContainer.enabled = enabled;
    }

    public void StartGame()
    {
        UpdateScores(0);
        SetGameScriptsEnabled(true);
        _playerController.Hide(false);
        _playerController.Pause(false);
        _platformContainer.InitPlatformGeneration();
    }

    public void PauseGame()
    {
        _playerController.Pause(true);
        SetGameScriptsEnabled(false);
    }

    public void ResumeGame()
    {
        _playerController.Pause(false);
        SetGameScriptsEnabled(true);
    }

    public void ToMenu()
    {
        _playerController.Stop();
        _cameraFollower.Stop();
        _platformContainer.Stop();

        SetGameScriptsEnabled(false);
    }

    public void RestartGame()
    {
        UpdateScores(0);
        _cameraFollower.Restart();
        _playerController.Restart();
        _platformContainer.Restart();

        StartGame();
    }

    public void GameOver(bool win)
    {
        _uiController.GameOver(win);
    }

    public void UpdateScores(int scores)
    {
        _uiController.UpdateScores(scores);
    }
}
