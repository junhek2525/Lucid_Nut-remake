using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public string doorcolor;
    public Itmepuzzle Itmepuzzle;
    public GameObject doorObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� �浹�ߴ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            /*Itmepuzzle.doorcord(doorcolor);*/
            

            /* Debug.Log("�÷��̾�� �浹�߽��ϴ�!");*/
            // �� ������Ʈ�� ����
            /*Itmepuzzle Itmepuzzle = other.GetComponent<Itmepuzzle>();*/
            if (doorcolor == "red" && Itmepuzzle.redkey > 0)
            {
                Itmepuzzle.redkey = Itmepuzzle.redkey - 1;
                Itmepuzzle.doorcord();
                Destroy(gameObject);
            }
            if (doorcolor == "orange" && Itmepuzzle.orangekey > 0)
            {
                Itmepuzzle.orangekey = Itmepuzzle.orangekey - 1;
                Itmepuzzle.doorcord();

                Destroy(gameObject);

            }
            if (doorcolor == "yellow" && Itmepuzzle.yellowkey > 0)
            {
                Itmepuzzle.yellowkey = Itmepuzzle.yellowkey - 1;
                Itmepuzzle.doorcord();

                Destroy(gameObject);

            }
            if (doorcolor == "Lightgreen" && Itmepuzzle.Lightgreenkey > 0)
            {
                Itmepuzzle.Lightgreenkey = Itmepuzzle.Lightgreenkey - 1;
                Itmepuzzle.doorcord();

                Destroy(gameObject);

            }
            if (doorcolor == "blue" && Itmepuzzle.bluekey > 0)
            {
                Itmepuzzle.bluekey = Itmepuzzle.bluekey - 1;
                Itmepuzzle.doorcord();

                Destroy(gameObject);

            }
            if (doorcolor == "purple" && Itmepuzzle.purplekey > 0)
            {
                Itmepuzzle.purplekey = Itmepuzzle.purplekey - 1;
                Itmepuzzle.doorcord();

                Destroy(gameObject);

            }
            
            /* Debug.Log("�÷��̾�� �浹�߽��ϴ�!");*/
            // �� ������Ʈ�� ����


        }
    }
    
           /* public void dooropen()
            {
               Destroy(gameObject);
            }*/














        // Update is called once per frame
        void Update()
    {
        
    }
}
