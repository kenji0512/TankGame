using UnityEngine;

public class HomingMissile : Bullet
{
    [SerializeField] private float homingSpeed = 5f; // 誘導速度
    [SerializeField] private float rotationSpeed = 200f; // 回転速度
    private Transform target; // ターゲット（追尾対象）

    // 初期化メソッド
    public void Initialize(Transform target, PlayerType shooterType)
    {
        if (target == null)
        {
            Debug.LogError("Target is null in Initialize method.");
        }
        this.target = target;
        this.shooterType = shooterType;
    }
    protected override void Start()
    {
        base.Start();

        // ターゲットを探す（例：Playerタグのオブジェクトを追尾）
        Transform targetObject = FindClosestTarget();
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            // ターゲットが見つからない場合は、直進
            Debug.LogWarning("No target found, missile will move straight.");
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // ターゲット方向を計算
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // 現在の方向をターゲット方向に向けて徐々に回転
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget,
                                                         rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);

            // 回転を適用
            transform.rotation = Quaternion.LookRotation(newDirection);

            // 前進
            transform.position += transform.forward * homingSpeed * Time.deltaTime;
        }
        else
        {
            // ターゲットがいない場合は通常の直進
            base.Update();
        }
    }

    // 最も近いターゲットを探す
    public Transform FindClosestTarget()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Player");
        Transform closestTarget = null;
        float shortestDistance = Mathf.Infinity;
        float maxRange = 50f; // 最大探索範囲

        foreach (GameObject potentialTarget in potentialTargets)
        {
            if (potentialTarget.transform == this.transform) continue; // 自分自身は無視

            float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
            if (distance < shortestDistance && distance <= maxRange)
            {
                shortestDistance = distance;
                closestTarget = potentialTarget.transform;
            }
        }

        return closestTarget;
    }

    protected override void HandleCharacterCollision(TunkController hitPlayer)
    {
        base.HandleCharacterCollision(hitPlayer);
        Debug.Log("Homing bullet hit the target!");
    }

    protected override void HandleWallCollision(BreakableWall breakableWall)
    {
        base.HandleWallCollision(breakableWall);
        Debug.Log("Homing bullet collided with a wall!");
    }
}
