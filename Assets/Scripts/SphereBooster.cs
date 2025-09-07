using UnityEngine;

public class SphereBooster : Bullet
{
    [SerializeField] private float _forceMagnitude = 10.0f; // 力の大きさ
    [SerializeField] private float _gravity = -9.81f; // 重力の強さ
    private float traveledDistance = 0f; // 累計移動距離
    private const float maxDistance = 30f; // 最大射程

    private Vector3 _velocity; // 初速度
    private Vector3 _gravityEffect; // 重力による影響
                                    //public PlayerType shooterType;

    // 発射位置を渡して初速度を設定
    public void Initialize(Vector3 initialDirection)
    {
        // 発射方向（shootPointの向いている方向）を取得して、初速度を設定
        _velocity = initialDirection.normalized * _forceMagnitude;

        // 重力の影響を設定 (Y軸方向にのみ影響)
        _gravityEffect = new Vector3(0, _gravity, 0);

    }

    private void Update()
    {
        _velocity += _gravityEffect * Time.deltaTime;
        Vector3 delta = _velocity * Time.deltaTime;
        transform.position += delta;

        traveledDistance += delta.magnitude;
        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject); // 射程を超えたら弾を破壊
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmoの色を設定
        Gizmos.color = Color.red;
        // 射出方向の矢印を描画
        Vector3 arrowEnd = transform.position + _velocity.normalized * _forceMagnitude;
        Gizmos.DrawLine(transform.position, arrowEnd); // 発射方向
        Gizmos.DrawSphere(arrowEnd, 0.1f); // 矢印の先端

        // 射程範囲を示す円を描画（必要に応じて範囲を設定可能）
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }
}
