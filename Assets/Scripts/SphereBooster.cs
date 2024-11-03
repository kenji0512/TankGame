using UnityEngine;

public class SphereBooster : MonoBehaviour
{
    [SerializeField] private float _forceMagnitude = 10.0f; // 力の大きさ
    [SerializeField] private Vector3 _initialDirection = new Vector3(1.0f, 1.0f, 0f); // 初期射出方向
    [SerializeField] private float _gravity = -9.81f; // 重力の強さ

    private Vector3 _velocity; // 初速度
    private Vector3 _gravityEffect; // 重力による影響

    void Start()
    {
        // 初速度を設定（力の大きさと方向から計算）
        _velocity = _initialDirection.normalized * _forceMagnitude;

        // 重力の影響を設定 (Y軸方向にのみ影響)
        _gravityEffect = new Vector3(0, _gravity, 0);
    }

    void Update()
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
        Vector3 arrowEnd = transform.position + _initialDirection.normalized * _forceMagnitude;
        Gizmos.DrawLine(transform.position, arrowEnd); // 発射方向
        Gizmos.DrawSphere(arrowEnd, 0.1f); // 矢印の先端

        // 射程範囲を示す円を描画（必要に応じて範囲を設定可能）
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }
}
