using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartFade : MonoBehaviour
{
    public Image image; // ����Ƽ���� ������ Image ������Ʈ

    private void Update()
    {
        Color color = image.color; // ���� �̹����� ������ ������
        color.a -= (Time.deltaTime/2);
        image.color = color; // �̹����� ���� ����� ������ �ٽ� �Ҵ�
    }
}
