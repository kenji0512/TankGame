public enum PlayerType
{
    Player1, Player2, Player3, Player4, Player5, Player6,
}
public enum GameState
{
    Ready,     // �J�E���g�_�E����
    Playing,    //�o�g����
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
public enum BGMType
{
    Title,
    Battle,
    Result
}

//���Ԃɂ܂��^��float����Ȃ���doble�ɂ���̂�����