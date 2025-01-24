using UnityEngine;

public class HomingMissile : Bullet
{
    [SerializeField] private float homingSpeed = 5f; // �U�����x
    [SerializeField] private float rotationSpeed = 200f; // ��]���x
    private Transform target; // �^�[�Q�b�g�i�ǔ��Ώہj

    // ���������\�b�h
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

        // �^�[�Q�b�g��T���i��FPlayer�^�O�̃I�u�W�F�N�g��ǔ��j
        Transform targetObject = FindClosestTarget();
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            // �^�[�Q�b�g��������Ȃ��ꍇ�́A���i
            Debug.LogWarning("No target found, missile will move straight.");
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // �^�[�Q�b�g�������v�Z
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // ���݂̕������^�[�Q�b�g�����Ɍ����ď��X�ɉ�]
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget,
                                                         rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);

            // ��]��K�p
            transform.rotation = Quaternion.LookRotation(newDirection);

            // �O�i
            transform.position += transform.forward * homingSpeed * Time.deltaTime;
        }
        else
        {
            // �^�[�Q�b�g�����Ȃ��ꍇ�͒ʏ�̒��i
            base.Update();
        }
    }

    // �ł��߂��^�[�Q�b�g��T��
    public Transform FindClosestTarget()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Player");
        Transform closestTarget = null;
        float shortestDistance = Mathf.Infinity;
        float maxRange = 50f; // �ő�T���͈�

        foreach (GameObject potentialTarget in potentialTargets)
        {
            if (potentialTarget.transform == this.transform) continue; // �������g�͖���

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
