public enum PlayerType
{
    Player1, Player2, Player3, Player4, Player5, Player6,
}
public enum GameState
{
    Playing,
    Paused,
    GameOver
}
[System.Flags]
public enum State
{
    Idle = 0,
    Moving = 1 << 0,
    Shooting = 1 << 1,
    Dead = 1 << 2
}

//ŽžŠÔ‚É‚Ü‚Â‚í‚éŒ^‚Ífloat‚¶‚á‚È‚­‚Ädoble‚É‚·‚é‚Ì‚ª‚¢‚¢