using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void OnHit()
    {
        StartCoroutine(HitEffect());
    }

    System.Collections.IEnumerator HitEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
}