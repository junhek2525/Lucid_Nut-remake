using System.Collections;
using UnityEngine;

public class dash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float cooldownTime = 1f;

    public bool DS = false; // �뽬 Ȱ��ȭ ���θ� �����ϴ� ����

    public bool isDashing = false; // �뽬 �� ����
    public bool isPreparingToDash = false; // �뽬 �غ� �� ����

    private Vector2 dashTarget; // �뽬 ��ǥ ��ġ

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TryStartDash(other.transform.position);
        }
    }

    // �뽬 ����
    public void TryStartDash(Vector2 targetPosition)
    {
        if (DS && !isPreparingToDash)
        {
            StartCoroutine(PrepareAndDash(targetPosition));
            DS = false; // �뽬�� ������ �� DS�� false�� ����
        }
    }

    private IEnumerator PrepareAndDash(Vector2 targetPosition)
    {
        isPreparingToDash = true; // �뽬 �غ� ������ ����
        yield return new WaitForSeconds(0.5f); // �غ� �ð� ���� ���

        dashTarget = targetPosition; // �뽬 ��ǥ ����
        isPreparingToDash = false; // �뽬 �غ� �Ϸ�
        isDashing = true; // �뽬 ����

        // �뽬 �� ��ǥ ��ġ�� �̵�
        while (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null; // �����Ӹ��� ���
        }

        isDashing = false; // �뽬 ����
        yield return new WaitForSeconds(cooldownTime); // ��Ÿ�� ���
    }

}
