using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    [SerializeField] float _laserRange = 30f;   // レーザーの射程距離
    [SerializeField] int _damage = 3;           // レーザーのダメージ
    [SerializeField] LineRenderer _lineRenderer; // レーザーの視覚エフェクト用
    [SerializeField] float _laserDuration = 0.2f; // レーザーの表示時間

    void Start()
    {
        FireLaser();
        Destroy(gameObject, _laserDuration); // 指定時間後に削除
    }

    void FireLaser()
    {
        Vector3 start = transform.position;
        Vector3 direction = transform.forward;
        Vector3 end = start + direction * _laserRange;

        // Raycastで敵を判定
        if (Physics.Raycast(start, direction, out RaycastHit hit, _laserRange))
        {
            // ヒットしたオブジェクトが 相手 ならダメージを与える
            TunkController hitplayer = hit.collider.GetComponent<TunkController>();
            if (hitplayer != null)
            {
                hitplayer.TakeDamage();
                end = hit.point; // レーザーの終点を敵に変更
            }
        }

        // LineRenderer でレーザーエフェクトを描画
        if (_lineRenderer != null)
        {
            _lineRenderer.SetPosition(0, start); // 開始地点
            _lineRenderer.SetPosition(1, end);   // 終了地点
        }
    }
}
