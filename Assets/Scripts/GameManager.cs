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
        // �V���O���g���p�^�[���̎���
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���̃C���X�^���X�����݂���ꍇ�͔j��
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���ԂŃI�u�W�F�N�g���ێ�
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        isGamePaused = false;
        Time.timeScale = 1f; // �Q�[�����ĊJ
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
        Time.timeScale = 0f; // �Q�[�����|�[�Y
        OnGamePaused.Invoke();
        Debug.Log("Game Paused!");
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        isGamePaused = false;
        Time.timeScale = 1f; // �Q�[�����ĊJ   �Q�[�����̂��ׂĂ̓����𐧌䂵�Ă��܂�
        Debug.Log("Game Resumed!");
    }

    public void EndGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // �Q�[�����~
        Debug.Log("Game Over!");
        // �Q�[���I���̏�����ǉ��i�Ⴆ�΃��U���g��ʂւ̑J�ڂȂǁj
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
