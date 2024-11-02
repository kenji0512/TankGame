using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���Ǘ��p

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGamePaused = false;
    private List<PlayerController> _players = new List<PlayerController>(); // �v���C���[���X�g��ǉ�
    private GameState currentState = GameState.Playing;

    public GameState CurrentState { get { return currentState; } }
    public delegate void GameEvent();
    public event GameEvent OnGameStarted;
    public event GameEvent OnGamePaused;
    public event GameEvent OnGameResumed;
    public event GameEvent OnGameOver;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        isGamePaused = false;
        Time.timeScale = 1f;
        OnGameStarted?.Invoke();
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
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
        Debug.Log("Game Paused!");
    }

    public void ResumeGame()
    {
        if (currentState != GameState.Paused) return;
        currentState = GameState.Playing;
        isGamePaused = false;
        Time.timeScale = 1f;
        OnGameResumed?.Invoke();
        Debug.Log("Game Resumed!");
    }

    public void EndGame()
    {
        isGamePaused = true;
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
        Debug.Log("Game Over!");
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    public void RegisterPlayer(PlayerController player)
    {
        _players.Add(player);
        Debug.Log($"Player {player.gameObject.name} joined the game. Total players: {_players.Count}");
    }

    public void RemovePlayer(PlayerController player)
    {
        _players.Remove(player);
        Debug.Log($"Player {player.gameObject.name} left the game. Remaining players: {_players.Count}");

        // �v���C���[��1�l�����̏ꍇ
        if (_players.Count <= 1)
        {
            Debug.Log("Only one player remaining. Transitioning to next scene...");
            TransitionToNextScene(); // ���̃V�[���Ɉڍs
        }
        Debug.Log($"Player {player.gameObject.name} left the game. Remaining players: {_players.Count}");
    }

    private void TransitionToNextScene()
    {
        Debug.Log("Transitioning to CrearScene...");

        // �����ŃV�[�����ڍs����i��: "CrearScene"�Ƃ������O�̃V�[���ցj
        SceneManager.LoadScene("CrearScene");
    }
}
