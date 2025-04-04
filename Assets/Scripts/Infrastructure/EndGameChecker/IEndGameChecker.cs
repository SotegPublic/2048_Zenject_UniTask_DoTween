using System;

public interface IEndGameChecker
{
    public void GameOverCheck();
    public Action OnGameOver { get; set; }
}
