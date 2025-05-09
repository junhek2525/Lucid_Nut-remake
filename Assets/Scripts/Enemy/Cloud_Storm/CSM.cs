using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CSM : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer 추가

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Attack info")]
    public GameObject projectilePrefab; // 발사할 오브젝트의 프리팹
    public float projectileSpeed = 15f; // 발사 속도
    public Vector2 detectionSize1 = new Vector2(10f, 2f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset1 = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer1; // 플레이어 레이어
    public bool attackC = true;

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(22f, 2f); // 플레이어 감지 범위 (사각형 크기)
    public Vector2 detectionOffset = Vector2.zero; // 사각형의 위치 오프셋
    public LayerMask playerLayer; // 플레이어 레이어
    public bool nmove = true;

    [Header("Special Attack info")]
    public GameObject lightningPrefab; // 벼락의 프리팹
    public float lightningRadius = 5f; // 벼락의 반경
    private int skillCount = 0; // 스킬 사용 카운트

    private Transform playerTransform;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        think();
    }

    private void Update()
    {
        Attack();
        DetectAndMoveTowardsPlayer();
        if (move)
        {
            moveSpeed = 3f;
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
            spriteRenderer.flipX = rigid.velocity.x > 0;
        }
    }

    void think()
    {
        if (nmove)
        {
            nextmove = Random.Range(-1, 2);
        }
        float nextTimeThink = Random.Range(2f, 4f);
        Invoke("think", nextTimeThink);
    }

    void DetectAndMoveTowardsPlayer()
    {
        // 플레이어를 감지할 사각형 영역
        Collider2D[] player = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset, detectionSize, 0f, playerLayer);

        foreach (Collider2D collider in player)
        {
            if (collider.tag == "Player" && attackC)
            {
                playerTransform = collider.transform;
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                nextmove = directionToPlayer.x > 0 ? 1 : -1;
            }
        }
    }

    void Attack()
    {
        // 플레이어를 감지할 사각형 영역
        Collider2D[] players = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in players)
        {
            if (collider.tag == "Player" && attackC)
            {
                attackC = false;
                skillCount++; // 스킬 사용 카운트 증가
                if (skillCount >= 3)
                {
                    skillCount = 0; // 카운트 초기화
                    StartCoroutine(FireLightning(collider.transform.position));
                }
                else
                {
                    StartCoroutine(FireProjectile(collider.transform.position));
                }
                break; // 감지된 플레이어가 있으면 한 번만 발사
            }
        }
    }

    private IEnumerator FireProjectile(Vector2 targetPosition)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        nextmove = directionToPlayer.x > 0 ? 1 : -1;
        spriteRenderer.flipX = nextmove > 0;    

        // 스킬 사용 중 이동을 멈추고 애니메이션 시작
        nextmove = 0;
        nmove = false;
        move = false;
        moveSpeed = 0f;
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);

        // 발사체 생성 및 방향 설정
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        targetPosition = (Vector2)players[0].transform.position;

        // 발사체의 Rigidbody2D 컴포넌트 가져오기
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            // 목표 위치로의 방향 계산
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            projectileRb.velocity = direction * projectileSpeed;

            // 발사체의 회전 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 180, angle + 90)); // 90도 추가
        }

        animator.SetBool("Attack", false);

        Destroy(projectile, 3f);

        yield return new WaitForSeconds(1f);
        nmove = true;
        nextmove = Random.Range(-1, 2);
        move = true;
        attackC = true;
    }

    private IEnumerator FireLightning(Vector2 targetPosition)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        nextmove = directionToPlayer.x > 0 ? 1 : -1;
        spriteRenderer.flipX = nextmove > 0;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 lightningpos = transform.position;

        lightningpos.x = player.transform.position.x;
        lightningpos.y = lightningpos.y + 20;

        // 스킬 사용 중 이동을 멈추고 애니메이션 시작
        nextmove = 0;
        nmove = false;
        move = false;
        moveSpeed = 0f;
        animator.SetBool("Attack2", true);
        
        yield return new WaitForSeconds(0.6f);

        // 벼락 생성
        GameObject lightning = Instantiate(lightningPrefab, lightningpos, Quaternion.identity);
        Rigidbody2D lightningRb = lightning.GetComponent<Rigidbody2D>();
        lightningRb.velocity = new Vector2(0,-1) * 30f;

        // 벼락의 영향 반경 내 플레이어에게 데미지를 입히는 로직 추가 가능

        animator.SetBool("Attack2", false);

        Destroy(lightning, 2f);

        yield return new WaitForSeconds(1f);
        nmove = true;
        nextmove = Random.Range(-1, 2);
        move = true;
        attackC = true;
    }

    // 시각적으로 감지 범위 표시용 (디버그)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset, new Vector3(detectionSize.x, detectionSize.y, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset1, new Vector3(detectionSize1.x, detectionSize1.y, 0));
    }
}
