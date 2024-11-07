using UnityEngine;

public class SphereBooster : Bullet
{
    [SerializeField] private float _forceMagnitude = 10.0f; // 力の大きさ
    [SerializeField] private float _gravity = -9.81f; // 重力の強さ

    private Vector3 _velocity; // 初速度
    private Vector3 _gravityEffect; // 重力による影響

    //void Start()
    //{
    //    Transform shootPoint = FindObjectOfType<BulletShoot>()._shootpointR;
    //    base.Start();
    //    // 初速度を設定（力の大きさと方向から計算）
    //    _velocity = shootPoint.forward.normalized * _forceMagnitude;

    //    // 重力の影響を設定 (Y軸方向にのみ影響)
    //    _gravityEffect = new Vector3(0, _gravity, 0);
    //}
    // 発射位置を渡して初速度を設定
    public void Initialize(Vector3 initialDirection, PlayerType shooter)
    {
        base.Start();
        // 発射方向（shootPointの向いている方向）を取得して、初速度を設定
        _velocity = initialDirection.normalized * _forceMagnitude;

        // 重力の影響を設定 (Y軸方向にのみ影響)
        _gravityEffect = new Vector3(0, _gravity, 0);
    }

    protected override void Update()
    {
        // 重力の影響を加える
        _velocity += _gravityEffect * Time.deltaTime;

        // フレームごとに位置を更新
        transform.position += _velocity * Time.deltaTime;
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
