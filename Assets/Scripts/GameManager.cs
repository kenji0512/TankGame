using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理用

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGamePaused = false;
    private List<TunkController> _players = new List<TunkController>(); // プレイヤーリストを追加
    private GameState currentState = GameState.Playing;

    public GameState CurrentState { get { return currentState; } }
    public delegate void GameEvent();
    public event GameEvent OnGameStarted;
    public event GameEvent OnGamePaused;
    public event GameEvent OnGameResumed;
    public event GameEvent OnGameOver;


    float _timer = 0.0f;

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
    public void HitStop(float time)
    {
        _timer = time;
        Time.timeScale = 0;
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

    public void RegisterPlayer(TunkController player)
    {
        _players.Add(player);
        Debug.Log($"Player {player.gameObject.name} joined the game. Total players: {_players.Count}");
    }

    public void RemovePlayer(TunkController player)
    {
        _players.Remove(player);
        Debug.Log($"Player {player.gameObject.name} left the game. Remaining players: {_players.Count}");

        // プレイヤーが1人だけの場合
        if (_players.Count <= 1)
        {
            Debug.Log("Only one player remaining. Transitioning to next scene...");
            TransitionToNextScene(); // 次のシーンに移行
        }
        Debug.Log($"Player {player.gameObject.name} left the game. Remaining players: {_players.Count}");
    }

    private void TransitionToNextScene()
    {
        Debug.Log("Transitioning to CrearScene...");
        if (_players.Count == 1) // 1人だけ残っているなら
        {
            Time.timeScale = 1f; // 時間を再開

            PlayerType winner = _players[0].playerType; // 残ったプレイヤーを勝者にする
            if (winner == PlayerType.Player1)
            {
                SceneManager.LoadScene("Player1WinScene"); // Player1が勝った場合
            }
            else if (winner == PlayerType.Player2)
            {
                SceneManager.LoadScene("Player2WinScene"); // Player2が勝った場合
            }
        }
    }
    public void CheckWinner(string winner)
    {
        if (winner == "TankBlue(Player)")
        {
            SceneManager.LoadScene("Player1WinScene");  // Player1 勝利画面へ
        }
        else if (winner == "TankRed(Enemy)")
        {
            SceneManager.LoadScene("Player2WinScene");  // Player2 勝利画面へ
        }
    }
}


///このゲームを面白くするタスク///
///1. ゲームプレイの多様性を追加
//単調さを防ぎ、リプレイ性を高めるための工夫：

//多彩な武器システム
//通常弾、ロケット弾以外にも、新たな武器（レーザー、○爆発弾、○誘導弾など）を追加し、プレイヤーが選択できるようにする。
//特殊スキルの導入
//一定時間無敵になるスキルや、高速移動スキルを追加。
//パワーアップアイテム
//フィールドにアイテムをランダム出現させ、取ると一時的に攻撃力や移動速度が上がる仕組みを導入。

//2.フィールドに変化を与える
//ゲーム環境がプレイヤーに影響を与える要素：

//破壊可能なオブジェクト
//壁だけでなく、建物や樹木なども破壊可能にし、戦略的な移動や射撃が求められる環境を構築。
//トラップの設置
//地雷やスパイクトラップなど、戦場に危険地帯を追加。
//ランダムイベント
//時間経過で障害物が増える、フィールドの一部が崩壊するなど、プレイヤーの動きに影響を与えるイベントを導入。

//3.対戦システムを工夫
//2人対戦をさらに盛り上げる要素：

//スコアシステム
//キル数や生存時間をスコア化し、ランキングを表示。
//タイムリミット付きモード
//制限時間内でどちらが多くのポイントを稼げるかを競う。
//バトルロイヤル形式
//マップを狭くして最後の1人になるまで戦う形式。

//追加アドバイス（お好みで）
//爆発時のノックバック処理があれば、Rigidbody.AddForce()で吹き飛び演出も可能。
//敵や壁に当たったときのSEも付けておくと、爽快感アップ。
//弾が敵や壁を貫通するかどうかのオプションも、bool _isPenetratingなどで管理できると良いですね。

//▶ チャレンジモード

//特定の条件（例: 特定の弾のみで倒す、時間内にゴールする）を達成するミッション。
//    4. スコアシステムの工夫
//▶ コンボシステム

//連続で命中させるとスコアが倍増する「コンボ」システムを導入。
//▶ ボーナススコア

//特定のターゲットを倒すことで得られるボーナスポイント。
//▶ スコアランキング

//オンラインランキングで他のプレイヤーと競い合えるようにする。

//5.AIプレイヤーやボスを追加
//プレイヤー同士だけでなく、AIを追加して挑戦要素を強化：

//AIタンクの追加
//プレイヤーを追いかけたり、逃げたりするAIタンクを配置。
//ボス戦の導入
//大型タンクボスを登場させ、協力して倒すモードを追加。

//7.成長要素を導入
//プレイヤーが続けたくなる仕組み：

//カスタマイズ可能なタンク
//タンクの見た目（色、形、エンブレム）をカスタマイズ可能に。
//レベルアップシステム
//プレイするたびにポイントを獲得し、スピードや攻撃力をアップグレード。
