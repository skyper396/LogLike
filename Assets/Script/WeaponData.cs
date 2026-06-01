using UnityEngine;

// 프로젝트 창에서 마우스 우클릭으로 무기 파일을 만들 수 있게 해줍니다.
// 이 줄이 메뉴를 만드는 핵심입니다. 오타가 없는지 꼭 확인하세요!
[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;    // 무기 이름
    public float attackRange;    // 이 무기의 사거리
    public int attackLevel;      // 이 무기의 기본 공격 레벨
    public float attackCooldown; // 다음 공격까지의 대기 시간
}