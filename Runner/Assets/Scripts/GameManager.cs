
using UnityEngine;

public static class GameManager
{
    public delegate void GameEvent();

    public static event GameEvent GameStarted;
    public static event GameEvent GameOver;
    public static event GameEvent GamePaused;
    public static event GameEvent GamePlayed;
    
    public static bool IsGamePaused;

    public static void OnGameStarted()
    {
        if (GameStarted != null) 
            GameStarted();
    }

    public static void OnGameOver()
    {
        if (GameOver != null) 
            GameOver();
    }

    public static void OnGamePaused()
    {
        if (GamePaused != null) 
            GamePaused();
    }

    public static void OnGamePlayed()
    {
        if (GamePlayed != null) 
            GamePlayed();
    }
}
