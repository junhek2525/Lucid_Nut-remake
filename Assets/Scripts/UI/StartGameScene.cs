using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(2);
    }
}
