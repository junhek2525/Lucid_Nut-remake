
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class coldwave : MonoBehaviour
{
    public GameObject coldPrefab;
    public GameObject redPrefab;
    bool on = true; //�ð� ���� ����
    float cooldownTime = 5; //���� ��ȯ �ð�
    float Duration = 7; //���� �庮 ���ӽð�
    public float Warning_Time = 1f;
    float time; //�ð�
    public Transform player;
    public PlayerMove PlayerMove;

    private Renderer attention;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        on = true;

       
        
    }
    // Update is called once per frame



    public void coldwaveshot()
    {
        StartCoroutine(Objecton()); //���� �庮 ����
    }




    void Update()
    {
    }
    /* if (on == true) //�ð�
     {
         time += Time.deltaTime;
     }

     if (time >= cooldownTime) //��Ÿ���� ����(�ð��� ��Ÿ�Ӻ��� ������)
     {
         on = false; //�ð� ����
         time = 0;  //�ð� �ʱ�ȭ*/




    /*}*/

    

    IEnumerator Objecton()
        {

        
        GameObject attention = Instantiate(redPrefab);
        attention.transform.position = player.position;
        yield return new WaitForSeconds(2f);
        GameObject coldwave = Instantiate(coldPrefab); //������Ʈ ��ȯ
            coldwave.transform.position = attention.transform.position; //�÷��̾� ��ġ�� �̵�
        Destroy(attention);
        yield return new WaitForSeconds(Duration); //���ӽð��� ������
            Destroy(coldwave); //������Ʈ ����
                on = true; //�ð� �簡��
            
        }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            /*PlayerMove.PlayerMove(); 20 - (20 * 12 / 100) = 17.6;*/


        }
       
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
        }

    }

}
