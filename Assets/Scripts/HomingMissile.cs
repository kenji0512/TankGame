using UnityEngine;

public class HomingMissile : Bullet
{
    [SerializeField] private float _homingSpeed = 5f; // 誘導速度
    [SerializeField] private float _rotationSpeed = 200f; // 回転速度
    private Transform _target; // ターゲット（追尾対象）
    [SerializeField] float maxRange = 50f; // 最大探索範囲


    // 初期化メソッド
    public void Initialize(PlayerType shooterType)
    {
        this.shooterType = shooterType;
    }
    protected override void Start()
    {
        base.Start();
        // 発射時の向きを補正（Z軸がねじれないようにする）
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0; // 上下方向のズレをなくす
        transform.forward = forwardDirection;

        // ターゲットを探す（例：Playerタグのオブジェクトを追尾）
        Transform targetObject = FindClosestTarget();
        if (targetObject != null)
        {
            _target = targetObject.transform;
        }
        else
        {
            // ターゲットが見つからない場合は、直進
            Debug.LogWarning("No target found, missile will move straight.");
        }
    }

    private void Update()
    {
        if (_target == null || Vector3.Distance(transform.position, _target.position) > maxRange)
        {
            // ターゲットがない、またはターゲットが一定距離以上離れている場合、再検索
            _target = FindClosestTarget();
        }
        if (_target != null)
        {
            // ターゲット方向を計算
            Vector3 directionToTarget = (_target.position - transform.position).normalized;

            // Z軸の回転を無視するようにする
            directionToTarget.y = 0; // 高さ方向の回転を無視
            // 目標方向を向ける（y軸を固定）
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // 回転を適用
            //transform.rotation = Quaternion.LookRotation(newDirection);
            // 徐々に回転
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            // 前進
            transform.position += transform.forward * _homingSpeed * Time.deltaTime;
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

        foreach (GameObject potentialTarget in potentialTargets)
        {
            if (potentialTarget.transform == this.transform) continue; // 自分自身は無視

            TunkController controller = potentialTarget.GetComponent<TunkController>();
            if (controller == null) continue; 

            float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
            if (potentialTarget.GetComponent<TunkController>().playerType == shooterType)
            {
                continue;
            }
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
        // Destroy をやめて、プールに戻す
        myPool.Catch("HomingMissile", this.gameObject);
    }
}
