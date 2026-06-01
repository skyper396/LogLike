using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHp = 100;

    [SerializeField]
    private int currentHp;

    private void Start()
    {
        currentHp = maxHp;

        Debug.Log($"플레이어 체력 : {currentHp}");
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp < 0)
        {
            currentHp = 0;
        }

        Debug.Log(
            $"플레이어 피해 {damage} / 남은 체력 {currentHp}"
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
    }
}