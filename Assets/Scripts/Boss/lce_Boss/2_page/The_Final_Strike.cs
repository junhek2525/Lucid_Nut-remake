using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class The_Final_Strike : MonoBehaviour
{
    private Transform player;
    public float speed = 45f;
    float speedtime;
    float speedtimemax = 0.8f;
    float delay = 3f;
    public float Duration = 2f;
    public float Durationtime = 0f;
    public float cooltimetest = 7f;

    float timetest;
    private GameObject lceBoss;
    private Vector2 direction;



    int i;

    // Start is called before the first frame update
    void Start()
    {
        
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // �÷��̾� ��ü�� �����ϴ��� Ȯ���ϰ� Ʈ�������� �����մϴ�.
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        /*StartCoroutine();*/
        on();
    }



    void Update()
    {
    }

    /*IEnumerator*/
    void on()
    {
        /*timetest += Time.deltaTime;
        if (timetest >= cooltimetest)
        {*/

        /*for (int i = 0; i <= 3 ; i++)
        {*/

        transform.position = player.position;
        int random = Random.Range(1, 3);
        /*float xx = 
        float yy = Random.Range(1, 16);*/
        if (random == 1)
        {
            transform.position = new Vector2(player.position.x - Random.Range(-10, 10), player.position.y - 15);
            Debug.Log("1");
        }
        else if (random == 2)
        {
            transform.position = new Vector2(player.position.x - Random.Range(-10, 10), player.position.y + 15);
            Debug.Log("2");
        }
        else if (random == 3)
        {
            Debug.Log("3");
        }



        // �÷��̾ �ٶ󺸵��� ȸ��
        LookAtPlayer();

        // �÷��̾ �ִ� �������� �̵�
        /*StartCoroutine(MoveInDirection());*/

        speedtime = speedtimemax;
        StartCoroutine(move());
                /*}*/


            /*timetest = 0f;*/  //��Ÿ�� �ʱ�ȭ
        }


    IEnumerator move()
    {
            while (speedtime > 0f)
            {
                speedtime -= Time.deltaTime;
                Debug.Log("time");

                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return null;
                }

            
        }
            
                
            
           
        
           
        
            
            

        
    

    void LookAtPlayer()
    {
        // �÷��̾�� ������Ʈ ������ ������ ���
        Vector2 direction = player.position - transform.position;

        // ȸ�� ������ ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������Ʈ�� Z�� ȸ���� �����Ͽ� �÷��̾ �ٶ󺸰� ����ϴ�.
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
    
/*
    // Update is called once per frame
    void Update()
    {
        timetest += Time.deltaTime;
        if (timetest >= cooltimetest)
        {
            timetest = 0f;

            *//*for (int a = 0; a <= 4; a++)
            {*//*
                Vector2 direction = player.position - transform.position;

                transform.position = player.position;
                float xx = Random.Range(1, 16);
                float yy = Random.Range(1, 16);
                transform.position = new Vector2(-30, 0);
                direction.Normalize();
                StartCoroutine(dush());
                IEnumerator dush()
                {
                    for (int i = 0; i <= 100; i++)
                    {

                        transform.Translate(direction * speed * Time.deltaTime);
                        yield return new WaitForSeconds(delay);
                    }
                }


                
            }




            // ���� �÷��̾� �������� �̵���ŵ�ϴ�.
           *//* transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            transform.Translate(0, speed * Time.deltaTime, 0); //��帧 �̵�*//*

            Durationtime += Time.deltaTime;
            if (Durationtime >= Duration) // ���ӽð��� ������
            {

            }
        }


        // �÷��̾�� �� ������ ������ ����մϴ�.




*/
    


/*}*/


/*   public Transform player;  // �÷��̾��� Transform�� ����
   public float speed = 5f;  // �̵� �ӵ�
   public float chaseSpeed = 8f;  // ���� �ӵ�
   public float minDistance = 2f;  // �÷��̾� ��ó�� �̵��� �Ÿ�
   public float maxDistance = 5f;  // �÷��̾� ��ó�� �̵��� �ִ� �Ÿ�

   private Vector2 targetPosition;
   private bool isChasingPlayer = false;

   void Start()
   {
       // �÷��̾� ��ó�� ������ ��ġ ����
       SetRandomPositionNearPlayer();
   }

   void Update()
   {
       if (!isChasingPlayer)
       {
           // ������ ��ġ�� �̵�
           MoveToTarget();
       }
       else
       {
           // �÷��̾ ���� ����
           ChasePlayer();
       }
   }

   void SetRandomPositionNearPlayer()
   {
       // �÷��̾� ��ó�� ������ ��ġ�� ����
       Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);
       targetPosition = (Vector2)player.position + randomOffset;
   }

   void MoveToTarget()
   {
       // ��ǥ ��ġ�� �̵�
       transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

       // ��ǥ ��ġ�� �����ϸ� �÷��̾ �ѱ� ����
       if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
       {
           isChasingPlayer = true;
       }
   }

   void ChasePlayer()
   {
       // �÷��̾ ���� ����
       Vector2 direction = (player.position - transform.position).normalized;
       transform.Translate(direction * chaseSpeed * Time.deltaTime);
   }
}
*/