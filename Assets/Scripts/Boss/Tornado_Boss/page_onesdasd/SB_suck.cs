using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_suck : MonoBehaviour
{
    public float attractionSpeed = 5f; // ���Ƶ��̴� �ӵ�

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Wall �±׸� ���� ������Ʈ�� �ݶ��̴��� ������ ��
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject); // ���� ������Ʈ �ı�
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Player �±׸� ���� ������Ʈ�� �ݶ��̴� �ȿ� ���� ��
        if (other.CompareTag("Player"))
        {
            // Player ������Ʈ�� ���� ������Ʈ�� ���Ƶ���
            Vector3 direction = transform.position - other.transform.position;
            other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, attractionSpeed * Time.deltaTime);
        }
    }
}