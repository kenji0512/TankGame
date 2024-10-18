using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public int hp_max = 3;
    public GameObject[] mesh_damage;

    private int hp = 3;

    private void Awake()
    {
        hp = hp_max;
    }

    public void Damage()
    {
        DoDamage();
    }

    private void DoDamage()
    {
        hp -= 1;
        UpdateMesh();
        if (hp <= 0)
            Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void UpdateMesh()
    {
        int damage = hp_max - hp;
        if (damage >= 0 && damage < mesh_damage.Length)
        {
            GameObject valid = mesh_damage[damage];
            foreach (GameObject msh in mesh_damage)
            {
                bool active = (msh == valid);
                if (active != msh.activeSelf)
                    msh.SetActive(active);
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Damage();
        }
    }
}
