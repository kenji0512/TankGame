using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGamePaused = false;

    List<Enemy> _enemies = new List<Enemy>();

    private GameState currentState = GameState.Playing;

    public GameState CurrentStateget { get { return currentState; } }
    public delegate void GameEvent();
    public event GameEvent OnGameStarted;
    public event GameEvent OnGamePaused;
    public event GameEvent OnGameResumed;
    public event GameEvent OnGameOver;
    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 他のインスタンスが存在する場合は破棄
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン間でオブジェクトを維持
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        isGamePaused = false;
        Time.timeScale = 1f; // ゲームを再開
        OnGameStarted.Invoke();
        Debug.Log("Game Started!");
    }

    public void PauseGame()
    {
        if (currentState == GameState.Paused) 
        {
            Debug.LogWarning("Game is already paused!");
            return;
        }
        currentState = GameState.Paused;
        isGamePaused = true;
        Time.timeScale = 0f; // ゲームをポーズ
        OnGamePaused.Invoke();
        Debug.Log("Game Paused!");
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        isGamePaused = false;
        Time.timeScale = 1f; // ゲームを再開   ゲーム内のすべての動きを制御しています
        Debug.Log("Game Resumed!");
    }

    public void EndGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // ゲームを停止
        Debug.Log("Game Over!");
        // ゲーム終了の処理を追加（例えばリザルト画面への遷移など）
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    public void Register(Enemy e)
    {
        _enemies.Add(e);
    }
    public void Remove(Enemy e)
    {
        _enemies.Remove(e);
    }
}
