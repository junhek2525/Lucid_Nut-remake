using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerMove playerMove;  // PlayerMove ��ũ��Ʈ�� ����

    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;  // ����Ʈ�� ������ ���� ��ġ
    public Vector2 boxSize;
    public GameObject SlashEffect;    // �ֵθ��� ����Ʈ
    public GameObject StingEffect;    // ��� ����Ʈ

    public AudioClip attackSound;     // ���� ���� Ŭ�� �߰�
    private AudioSource audioSource;  // ����� �ҽ� �߰�

    private bool showGizmos = false;  // ����� ǥ�� ����
    private Vector2 gizmoCenter;      // ����� �߾� ��ġ
    private Vector2 gizmoSize;        // ����� ũ��
    private float gizmoDisplayTime = 0.5f; // ����� ǥ�õ� �ð�
    private float gizmoTimer = 0f;    // ����� Ÿ�̸�

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();  // PlayerMove ��ũ��Ʈ ��������
        audioSource = GetComponent<AudioSource>();  // ����� �ҽ� ������Ʈ ��������

        if (audioSource == null)
        {
            // ���� ����� �ҽ��� ������ �߰�
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        Vector2 len = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pos.transform.position;
        float z = Mathf.Atan2(len.y, len.x) * Mathf.Rad2Deg;

        pos.transform.rotation = Quaternion.Euler(0, 0, z);

        if (curTime <= 0)
        {
            // ���콺 ��Ŭ�� (�ֵθ��� ����)
            if (Input.GetMouseButtonDown(0) && !playerMove.isAttack)
            {
                int direction = len.x > 0 ? 1 : -1;  // ĳ���͸� �������� �������̸� 1, �����̸� -1

                PerformSlashAttack(direction);  // �ֵθ��� ���� ����
                curTime = coolTime;  // ��Ÿ�� ����
            }

            // ���콺 ��Ŭ�� (��� ����)
            if (Input.GetMouseButtonDown(1) && !playerMove.isAttack)
            {
                int direction = len.x > 0 ? 1 : -1;  // ĳ���͸� �������� �������̸� 1, �����̸� -1

                PerformStingAttack(direction);  // ��� ���� ����
                curTime = coolTime;  // ��Ÿ�� ����
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        // ����� Ÿ�̸� ������Ʈ
        if (gizmoTimer > 0)
        {
            gizmoTimer -= Time.deltaTime;
        }
        else
        {
            showGizmos = false;  // Ÿ�̸Ӱ� 0 ���ϰ� �Ǹ� ����� ǥ�ø� ��Ȱ��ȭ
        }
    }


    private void PerformSlashAttack(int direction)
    {
        playerMove.isAttack = true;

        playerMove.FlipAttack(direction);  // �÷��̾��� ������ ���� �������� ����

        GameObject SE = Instantiate(SlashEffect, transform.position, pos.transform.rotation,transform);  // �ֵθ��� ����Ʈ ����
        SE.gameObject.GetComponent<SpriteRenderer>().flipY = direction < 0 ? true : false;
        Destroy(SE, 0.3f);  // ª�� �ð� �� ����Ʈ ����

        audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���
    }

    private void PerformStingAttack(int direction)
    {
        playerMove.isAttack = true;

        playerMove.FlipAttack(direction);  // �÷��̾��� ������ ���� �������� ����

        GameObject SE = Instantiate(StingEffect, transform.position, pos.transform.rotation, transform);  // �ֵθ��� ����Ʈ ����
        SE.gameObject.GetComponent<SpriteRenderer>().flipY = direction < 0 ? true : false;
        Destroy(SE, 0.3f);  // ª�� �ð� �� ����Ʈ ����

        audioSource.PlayOneShot(attackSound);  // ������ �� ���� ���
    }
}
