using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ==========================================
    // [이동 관련 설정]
    // ==========================================
    [Header("Movement")]
    [Tooltip("캐릭터의 이동 속도입니다.")]
    public float moveSpeed = 5f;

    // 물리를 이용한 이동 처리를 위해 컴포넌트를 저장할 변수 (2D 강체)
    private Rigidbody2D rb;

    // 키보드(WASD, 방향키) 입력 값을 저장할 변수 (X축, Y축 좌표)
    private Vector2 moveInput;


    // ==========================================
    // [상태 및 애니메이션 설정]
    // ==========================================
    [Header("State & Animation")]
    [Tooltip("현재 캐릭터의 행동 상태를 나타냅니다. (기본값: Idle)")]
    public PlayerState currentState = PlayerState.Idle;

    // 애니메이션 재생을 제어하기 위해 컴포넌트를 저장할 변수
    private Animator anim;

    // 무기의 설정
    [Header("Equipped Weapon")]
    [Tooltip("현재 장착 중인 무기 데이터 에셋을 여기에 넣으세요.")]
    public WeaponData currentWeapon; // 이 줄이 반드시 있어야 합니다!

    // ==========================================
    // [공격 판정 관련 설정]
    // ==========================================
    [Header("Attack Settings")]
    [Tooltip("공격 범위의 중심이 될 오브젝트입니다. (캐릭터 앞 배치)")]
    public Transform attackPoint;

    [Tooltip("공격이 닿는 반지름 범위(원 크기)입니다.")]
    public float attackRange = 0.5f;

    [Tooltip("공격 판정을 적용할 적들의 레이어(Layer)입니다.")]
    public LayerMask enemyLayers;


    // ==========================================
    // [초기화 함수] 게임이 시작될 때 단 1번만 실행됨
    // ==========================================
    void Start()
    {
        // 내 게임 오브젝트에 붙어있는 Rigidbody2D 컴포넌트를 찾아 상자에 넣어둡니다.
        rb = GetComponent<Rigidbody2D>();

        // 내 게임 오브젝트에 붙어있는 Animator 컴포넌트를 찾아 상자에 넣어둡니다.
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. 상태에 따른 입력 처리
        HandleInput();

        // 2. 애니메이션 파라미터 동기화
        UpdateAnimation();

        // 수정 전
        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.Attack)
        {
            PerformAttack();
        }

        // 수정 후
        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.Attack)
        {
            PerformAttack(); // 무기 데이터에 정의된 값을 스스로 가져다 씁니다.
        }
    }

    void HandleInput()
    {
        // 공격 중이 아닐 때만 이동 입력을 받습니다.
        if (currentState != PlayerState.Attack)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;
        }
        else
        {
            // 공격 중에는 이동 입력을 0으로 만들어 멈추게 합니다.
            moveInput = Vector2.zero;
        }

        // --- 공격 입력 (추가된 부분) ---
        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.Attack)
        {
            PerformAttack();
        }

        // --- 가드 입력 (추가된 부분) ---
        if (Input.GetMouseButton(1))
        {
            currentState = PlayerState.Guard;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            currentState = PlayerState.Idle;
        }
    }

    void PerformAttack() // 이제 외부에서 레벨을 안 받아도 무기 데이터에서 가져옵니다.
    {
        // [추가] 만약 무기가 장착되지 않았다면 공격을 수행하지 않음 (방어 코드)
        if (currentWeapon == null)
        {
            Debug.LogWarning("무기가 장착되지 않았습니다!");
            return;
        }

        // 1. 상태 설정 및 애니메이션 실행
        // 이제 level 대신 currentWeapon.attackLevel을 사용합니다.
        currentState = PlayerState.Attack;
        anim.SetInteger("attackLevel", currentWeapon.attackLevel);
        anim.SetTrigger("doAttack");

        // 2. 실제 판정 로직
        // 이제 attackRange 대신 currentWeapon.attackRange를 사용합니다.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, currentWeapon.attackRange, enemyLayers);

        // 3. 판정된 결과 수행
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " 히트! 무기: " + currentWeapon.weaponName + " / 레벨: " + currentWeapon.attackLevel);

            if (enemy.GetComponent<EnemyTest>() != null)
            {
                enemy.GetComponent<EnemyTest>().OnHit();
            }
        }

        // 4. 상태 초기화 (쿨타임도 무기 데이터를 따를 수 있습니다)
        Invoke("ResetState", currentWeapon.attackCooldown);
    }

    void ResetState()
    {
        if (currentState != PlayerState.Guard)
            currentState = PlayerState.Idle;
    }

    void UpdateAnimation()
    {
        if (anim == null) return;
        anim.SetBool("isMoving", moveInput.magnitude > 0);
        anim.SetBool("isGuarding", currentState == PlayerState.Guard);
    }

    void FixedUpdate()
    {
        // 가드 중에는 이동 속도를 0으로, 아니면 moveInput에 따라 이동
        if (currentState == PlayerState.Guard)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = moveInput * moveSpeed;
        }
    }

    // 에디터에서 범위를 보기 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}