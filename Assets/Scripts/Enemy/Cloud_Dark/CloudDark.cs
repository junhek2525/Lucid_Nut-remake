using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloudDark : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer �߰�

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Dash info")]
    public bool isDashing = false; // �뽬 �� ����
    public bool isPreparingToDash = false; // �뽬 �غ� �� ����
    private Vector2 dashTarget;
    [SerializeField] private float dashSpeed = 15f;
    public Vector2 detectionSize1 = new Vector2(5f, 2f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset1 = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer1; // �÷��̾� ���̾�
    public GameObject dashEffect;
    public GameObject projectilePrefab; // �߻�ü ������
    public GameObject projectilePrefab2; // �߻�ü ������
    public int dashCount = 0; // �뽬 Ƚ��
    private bool hasFiredProjectile = false; // �߻�ü �߻� ����

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(15f, 5f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public bool nmove = true;

    private Transform playerTransform;
    private Vector2 dashStartPosition; // �뽬 ���� ��ġ
    [SerializeField] private float dashDistance = 10f; // �뽬 �Ÿ�

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        think();
    }

    private void Update()
    {
        DetectAndMoveTowardsPlayer();
        Dashcoll();
        if (move && !isDashing) // �뽬 ���� �ƴ� ���� �̵�
        {
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
        // �÷��̾ ������ �簢�� ����
        Collider2D[] player = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset, detectionSize, 0f, playerLayer);

        foreach (Collider2D collider in player)
        {
            if (collider.tag == "Player")
            {
                playerTransform = collider.transform;
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                nextmove = directionToPlayer.x > 0 ? 1 : -1;
            }
        }
    }

    void Dashcoll()
    {
        // �÷��̾ ������ �簢�� ����
        Collider2D[] player1 = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in player1)
        {
            if (collider.tag == "Player" && !isPreparingToDash)
            {
                dashStartPosition = transform.position; // �뽬 ���� ��ġ ����
                dashTarget = collider.transform.position; // �뽬 ��ǥ ����
                dashTarget.y = transform.position.y; // y�� �ڱ�ɷ� ����

                if (dashCount < 3 && !isDashing)
                {
                    StartCoroutine(PrepareAndDash(dashTarget));
                    dashCount++;
                }
                else if (!hasFiredProjectile) // �߻�ü�� ���� �߻���� �ʾ�����
                {
                    isDashing = true;
                    StartCoroutine(LaunchProjectile(dashTarget));
                    dashCount = 0; // �뽬 Ƚ�� �ʱ�ȭ
                    hasFiredProjectile = true; // �߻�ü �߻� ���� ����
                }
            }
        }
    }

    private IEnumerator PrepareAndDash(Vector2 playerPosition)
    {
        move = false;
        nmove = false;
        isPreparingToDash = true; // �뽬 �غ� ������ ����
        isDashing = true; // �뽬 ����
        animator.SetBool("Attack", true);

        // �÷��̾� ���� ���� ���
        Vector2 directionToPlayer = (playerPosition - (Vector2)transform.position).normalized;
        dashTarget = (Vector2)transform.position + directionToPlayer * dashDistance; // �뽬 ��ǥ ����

        // ��������Ʈ ���� ����
        spriteRenderer.flipX = directionToPlayer.x > 0;

        yield return new WaitForSeconds(0.5f); // �غ� �ð� ���� ���

        GameObject explosion = Instantiate(dashEffect, this.transform.position, Quaternion.identity);
        Destroy(explosion, 0.5f);

        // �뽬 �� ��ǥ ��ġ�� �̵�
        while (Vector2.Distance(transform.position, dashTarget) > 0.2f)
        {
            // �뽬 ��ǥ �������� �̵�
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            // �뽬 �߿��� ��������Ʈ ���� ����
            Vector2 direction = (dashTarget - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = direction.x > 0;

            yield return null; // �����Ӹ��� ���
        }

        // �뽬 �Ϸ� �� ���� ����
        animator.SetBool("Attack", false);
        isDashing = false; // �뽬 ����
        yield return new WaitForSeconds(1f); // ��Ÿ�� ���
        move = true;
        nmove = true;
        isPreparingToDash = false; // �뽬 �غ� �Ϸ�
        hasFiredProjectile = false; // ���� �߻縦 ���� �߻� ���� �ʱ�ȭ
    }

    private IEnumerator LaunchProjectile(Vector2 targetPosition)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)this.transform.position).normalized;
        spriteRenderer.flipX = directionToPlayer.x > 0;

        move = false;
        nmove = false;

        if (projectilePrefab != null)
        {
            animator.SetBool("Attack2", true);
            yield return new WaitForSeconds(1.3f);

            Debug.Log(this.transform.position);
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector2 shootPos = this.transform.position;
            shootPos.y = player.transform.position.y;
            GameObject projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb != null)
            {
                // �߻�ü�� ���� �߻�   
                Vector2 launchDirection = new Vector2(0, 1); // ���� ����
                float launchSpeed = 30f; // �߻� �ӵ�

                projectileRb.velocity = launchDirection * launchSpeed;
                animator.SetBool("Attack2", false);

                // 2�ʰ� X�ุ ����ٴϱ�
                float timeElapsed = 0f;
                float fixedHeight = 20f+this.transform.position.y; // ���� ��

                while (timeElapsed < 1f)
                {
                    if (projectile.transform.position.y >= fixedHeight)
                    {
                        // �߻�ü�� Y�� ��ġ�� ����
                        Vector2 currentPosition = projectile.transform.position;
                        currentPosition.y = fixedHeight;
                        projectile.transform.position = currentPosition;
                    }

                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                Vector2 playerpos = player.transform.position;
                Vector2 currpos = projectile.transform.position;
                currpos.x = playerpos.x;
                projectile.transform.position = currpos;

                // �߻�ü�� ������ ������ ���
                while (projectileRb.velocity.y > 0)
                {
                    yield return null;
                }

                // �߻�ü ���� ��, ���� ������Ʈ�� �����Ͽ� �Ʒ��� ����߸���
                int numberOfObjects = 8; // ����߸� ������Ʈ ����
                for (int i = 0; i < numberOfObjects; i++)
                {
                    Vector2 spawnPosition = new Vector2(projectile.transform.position.x, projectile.transform.position.y);
                    GameObject fallingObject = Instantiate(projectilePrefab2, spawnPosition, Quaternion.identity);
                    Rigidbody2D fallingObjectRb = fallingObject.GetComponent<Rigidbody2D>();

                    if (fallingObjectRb != null)
                    {
                        Vector2 fallingDirection = new Vector2(Random.Range(-3f, 3f), -1); // �ణ�� �¿� ���� �������� ����߸���
                        fallingObjectRb.velocity = fallingDirection.normalized * Random.Range(2f, 5f); // �������� �ӵ� ����
                    }
                    Destroy(fallingObject,4f);
                }

                Destroy(projectile); // ���� �߻�ü ����

                yield return new WaitForSeconds(1f); // �뽬 ���� ���
                isDashing = false;
                move = true;
                nmove = true;
                hasFiredProjectile = false;
            }
        }
    }


    // �ð������� ���� ���� ǥ�ÿ� (�����)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset, new Vector3(detectionSize.x, detectionSize.y, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)detectionOffset1, new Vector3(detectionSize1.x, detectionSize1.y, 0));
    }
}
