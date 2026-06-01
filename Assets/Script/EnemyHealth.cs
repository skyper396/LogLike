using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHp = 10;

    [SerializeField]
    private int currentHp;

    private void Start()
    {
        currentHp = maxHp;
        Debug.Log($"{gameObject.name} 체력 : {currentHp}");
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        Debug.Log(
            $"{gameObject.name} 피해 {damage} / 남은 체력 {currentHp}"
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} 사망");

        Destroy(gameObject);
    }
}