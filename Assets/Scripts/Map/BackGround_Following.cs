using UnityEngine;

public class BackGround_Following : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // ����� ������� �ӵ�

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPosition = player.transform.position;
        Vector3 backgroundPosition = this.transform.position;

        // x��� y�� ��� �ε巴�� ��������� ������Ʈ
        backgroundPosition.x = Mathf.Lerp(backgroundPosition.x, playerPosition.x, smoothSpeed * Time.deltaTime);
        backgroundPosition.y = Mathf.Lerp(backgroundPosition.y, playerPosition.y, smoothSpeed * Time.deltaTime);

        backgroundPosition.z = 2f; // z�� ����
        this.transform.position = backgroundPosition;
    }
}
