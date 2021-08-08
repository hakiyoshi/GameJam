using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Header("�ύX��V�[����")]
    public string SceneName;

    [Header("�V�[���ύX����")]
    public float IntervalTime;


    void Start()
    {
    }

    public void Onclick()
    {
        StartCoroutine("Click", IntervalTime);
    }

    IEnumerator Click(float interval)
    {
        float time = 0.0f;

        while (time <= interval)
        {
            time += Time.deltaTime;
            yield return 0;
        }
        SceneManager.LoadScene(SceneName);
    }
}
