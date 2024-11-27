using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public Image Panel;
    public AudioSource audioSource; // ����� �ҽ� �߰�
    float time = 0f;
    public float Fade_time = 1f;
    public float PitchFade_time = 1f; // ��ġ ���̵� �ð� �߰�

    public void StartPrologue()
    {
        StartCoroutine(FadeFlow());
    }

    public void EndGame()
    {
        Application.Quit();
    }

    IEnumerator FadeFlow()
    {
        Color alpha = Panel.color;

        // ���̵� �� �� ��ġ ���Ҹ� ���ÿ� ����
        time = 0f;
        Panel.gameObject.SetActive(true);
        float pitchTime = 0f;

        while (alpha.a < 1f || audioSource.volume > 0f)
        {
            time += Time.deltaTime / Fade_time;
            pitchTime += Time.deltaTime / PitchFade_time;

            // �г��� ���İ��� ���� ������Ŵ (���̵� ��)
            if (alpha.a < 1f)
            {
                alpha.a = Mathf.Lerp(0, 1, time);
                Panel.color = alpha;
            }

            // ����� ��ġ�� ������ ���� ���ҽ�Ŵ
            if (audioSource.volume > 0f)
            {
                audioSource.volume = Mathf.Lerp(1f, 0f, pitchTime);
            }

            yield return null;
        }

        // ���̵�� ��ġ ���Ұ� ��� �Ϸ�� �� �� ��ȯ
        SceneManager.LoadScene(1);
        yield return null;
    }
}
