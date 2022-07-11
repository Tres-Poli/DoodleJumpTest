public interface IUICommunication
{
    public void StartGame();
    public void PauseGame();
    public void ToMenu();
    public void RestartGame();
    public void ResumeGame();
    public void GameOver(bool win);
    public void UpdateScores(int scores);
}
