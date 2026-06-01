using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    // [이 줄이 추가되어야 합니다!]
    // public으로 선언해야 인스펙터 창에 드래그할 수 있는 빈 칸이 생깁니다.
    public SpriteRenderer spriteRenderer;

    public void OnHit()
    {
        // 맞았을 때 실행할 로직 (예: 빨간색으로 변경)
        StartCoroutine(HitEffect());
    }

    System.Collections.IEnumerator HitEffect()
    {
        // spriteRenderer가 연결되어 있다면 색을 빨갛게 바꿉니다.
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
}