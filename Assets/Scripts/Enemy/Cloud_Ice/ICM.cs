using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICM : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer; // SpriteRenderer �߰�

    [Header("Move info")]
    private Rigidbody2D rigid;
    public bool move = true;
    public float moveSpeed = 3f;
    public int nextmove;

    [Header("Attack info")]
    public GameObject projectilePrefab; // �߻��� ������Ʈ�� ������
    public float projectileSpeed = 15f; // �߻� �ӵ�
    public Vector2 detectionSize1 = new Vector2(10f, 2f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset1 = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer1; // �÷��̾� ���̾�
    public bool attackC = true;

    [Header("Detection info")]
    public Vector2 detectionSize = new Vector2(22f, 2f); // �÷��̾� ���� ���� (�簢�� ũ��)
    public Vector2 detectionOffset = Vector2.zero; // �簢���� ��ġ ������
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public bool nmove = false;

    private Transform playerTransform;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
        think();
    }


    private void Update()
    {
        Attack();
        DetectAndMoveTowardsPlayer();
        if (move)
        {
            spriteRenderer.flipX = rigid.velocity.x > 0;
            moveSpeed = 3f;
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);
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
        // �÷��̾ ������ �簢�� ����
        Collider2D[] players = Physics2D.OverlapBoxAll((Vector2)transform.position + detectionOffset1, detectionSize1, 0f, playerLayer1);

        foreach (Collider2D collider in players)
        {
            if (collider.tag == "Player" && attackC)
            {
                attackC = false;
                StartCoroutine(FireProjectile(collider.transform.position));
                break; // ������ �÷��̾ ������ �� ���� �߻�
            }
        }
    }

    private IEnumerator FireProjectile(Vector2 targetPosition)
    {
        Vector2 directionToPlayer = (targetPosition - (Vector2)transform.position).normalized;
        nextmove = directionToPlayer.x > 0 ? 1 : -1;
        spriteRenderer.flipX = nextmove > 0;
        int a = 0;
        if (nextmove == 1)
        {
            a = 180;
        }

        // ��ų ��� �� �̵��� ���߰� �ִϸ��̼� ����
        nextmove = 0;
        nmove = false;
        moveSpeed = 0f;
        move = false;
        animator.SetBool("Attack", true);
        // �߻�ü ���� ��ġ
        Vector3 spawnPosition = transform.position + new Vector3(0, 2f, 0); // �������� ������ �߰�
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.transform.Rotate(0,a,0);
        // Ÿ�� ��ġ ����
        yield return new WaitForSeconds(1.2f); // �ִϸ��̼� ���
        
        if (projectile == null)
        {
            animator.SetBool("Attack", false);
            nmove = true;
            nextmove = Random.Range(-1, 2);
            attackC = true;
            move = true;
        }
        else 
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                targetPosition = (Vector2)players[0].transform.position;

                // �߻�ü�� Rigidbody2D ������Ʈ ��������
                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                if (projectileRb != null)
                {
                    
                    // ��ǥ ��ġ���� ���� ���
                    Vector2 direction = (targetPosition - (Vector2)spawnPosition).normalized;
                    projectileRb.velocity = direction * projectileSpeed;

                    // �߻�ü�� ȸ�� ���
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+180-a));
                }
            }

            animator.SetBool("Attack", false);

            if (projectile != null)
            {
                Destroy(projectile, 3f);
            }

            yield return new WaitForSeconds(1f);
            nmove = true;
            nextmove = Random.Range(-1, 2);
            attackC = true;
            move = true;
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