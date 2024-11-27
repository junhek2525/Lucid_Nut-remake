using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossmove : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform�� �����մϴ�.
    public float speed = 2f; // ���� �̵��ϴ� �ӵ��Դϴ�.
    public float stoppingDistance = 1f; // �÷��̾���� ���ߴ� �Ÿ��Դϴ�.
    public bool stop = false;
    public float delaymove = 0f;
    // Start is called before the first frame update
    /*private Transform player; // �÷��̾��� Ʈ������*/

    void Start()
    {
        // �±װ� "Player"�� ��ü�� ã���ϴ�.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // �÷��̾� ��ü�� �����ϴ��� Ȯ���ϰ� Ʈ�������� �����մϴ�.
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    public void stopdirector(float delay)
    {
        delaymove = delay;
        StartCoroutine(stopon());
    }

    public void speeddirector()
    {
        StartCoroutine(speeddown());
    }
    IEnumerator speeddown()
    {
        // ������� 50%���� ����� ��
        speed = 1f;
        yield return new WaitForSeconds(5f);
        speed = 2f;
    }
        IEnumerator stopon()
    {
        stop = true;
        Debug.Log("����");
        yield return new WaitForSeconds(delaymove);
        Debug.Log("�ȸ���");
        stop = false;
    }


    void Update()
    {
       
       if(stop == false)
        {
            // �÷��̾�� �� ������ ������ ����մϴ�.
            Vector2 direction = player.position - transform.position;
            direction.Normalize();

            // ���� �÷��̾� �������� �̵���ŵ�ϴ�.
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
       /* if(stop == true)
        {
            Debug.Log("wkrehd");
        }*/
        
    }




    /*void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, player.position);

        // �÷��̾ Ư�� �Ÿ����� �ָ� ���� �� ����
        if (distance > stoppingDistance)
        {
            // �÷��̾� �������� �̵�
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // �÷��̾ �ٶ󺸵��� ȸ��
            transform.LookAt(player);
        }
    }*/
}
