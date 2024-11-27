using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    [SerializeField] private int hp = 0;
    [SerializeField] private int maxHp = 5000;

    public Image healthSlider;
    private Coroutine healthCoroutine;

    private void Start()
    {
        hp = maxHp;
        healthSlider.fillAmount = 1f; // �����̴��� 1�� max�� �����ϹǷ� 1�� �ʱ�ȭ
    }

    // �÷��̾ ������ �� ȣ��Ǵ� �Լ�
    public void TakeDamage(int damage)
    {
        // ���� ü���� ��� ����
        hp = Mathf.Clamp(hp - damage, 0, maxHp);

        // ���� ������ ü�� ���� �ڷ�ƾ�� ���� ���̸� ����
        if (healthCoroutine != null)
        {
            StopCoroutine(healthCoroutine);
        }

        // �����̴� ���Ҹ� �ε巴�� ó���ϴ� �ڷ�ƾ ����
        healthCoroutine = StartCoroutine(SmoothHealthDecrease(hp));
    }

    // �ε巴�� �����̴��� �����ϴ� �ڷ�ƾ
    private IEnumerator SmoothHealthDecrease(int targetHealth)
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // �����̴��� �����ϴµ� �ɸ��� �ð� (0.5��)

        float startingFill = healthSlider.fillAmount; // �����̴��� ���� fill ��
        float targetFill = (float)targetHealth / maxHp; // ��ǥ �����̴� ��

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // �����̴� ���� �ε巴�� �پ�鵵�� ������
            healthSlider.fillAmount = Mathf.Lerp(startingFill, targetFill, elapsedTime / duration);
            yield return null; // �� ������ ���
        }

        // �����̴� ���� �Ϸ� �� ��Ȯ�ϰ� Ÿ�� ü������ ������
        healthSlider.fillAmount = targetFill;
    }
}
