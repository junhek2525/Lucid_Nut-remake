using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 2f;  // ���������� �� �ɸ��� �ð�
    private Renderer objectRenderer;
    private Color originalColor;
    private float fadeTimer;

    void Start()
    {
        // Renderer ������Ʈ�� ��������
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            // ���� ��Ƽ������ ���� ��������
            originalColor = objectRenderer.material.color;
        }
        else
        {
            Debug.LogError("Renderer component not found on the object.");
        }
    }

    void Update()
    {
        if (objectRenderer != null)
        {
            // Ÿ�̸� ����
            fadeTimer += Time.deltaTime;

            // ���� ���
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            // ��Ƽ������ ���� ������Ʈ
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            objectRenderer.material.color = newColor;

            // ������ ���������� ������Ʈ�� ����
           /* if (alpha <= 0f)
            {
                Destroy(gameObject); // ������Ʈ ����
            }*/
        }
    }

    public void StartFading()
    {
        // ����ȭ ����
        fadeTimer = 0f;
    }
}
