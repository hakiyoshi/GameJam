using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Header("変更先シーン名")]
    public string SceneName;

    [Header("シーン変更時間")]
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
