using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject Selects;
    [SerializeField] GameObject Arrow;

    GameObject[] Select;

    int SelectCount;

    bool bPause;

    // Start is called before the first frame update
    void Start()
    {
        Select = new GameObject[4];

        for(int i = 0; i < Select.Length; i++)
        {
            Select[i] = Selects.transform.GetChild(i).gameObject;
        }

        ResetSelects();

        bPause = false;

        Selects.SetActive(bPause);
        Arrow.SetActive(bPause);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTimeScale();

        if (bPause)
        {
            DownSelect();

            UpSelect();

            NowSelect();

            PushSelect();
        }
        Debug.Log(SelectCount);
    }

    void ChangeTimeScale()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bPause = !bPause;
            
            if(bPause)
                ResetSelects();
        }

        if (bPause)
        {
            Time.timeScale = 0f;
        }
        else
            Time.timeScale = 1f;

        Selects.SetActive(bPause);
        Arrow.SetActive(bPause);
    }

    void DownSelect()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SelectCount++;

            if (SelectCount == Select.Length)
            {
                SelectCount = 0;
                Arrow.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
                Arrow.transform.localPosition += new Vector3(0, -155, 0);
        }
    }

    void UpSelect()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SelectCount--;

            if (SelectCount == -1)
            {
                SelectCount = Select.Length;
                Arrow.transform.localPosition = new Vector3(0, -465, 0);
            }
            else
                Arrow.transform.localPosition += new Vector3(0, 155, 0);
        }
    }

    void NowSelect()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == SelectCount)
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            else
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    void ResetSelects()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            else
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }

        Arrow.transform.localPosition = Vector3.zero;

        SelectCount = 0;
    }

    void PushSelect()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (SelectCount)
            {
                case 0:
                    bPause = false;
                    break;
                case 1:
                    Fade.FadeOut(SceneManager.GetActiveScene().name);
                    break;
                case 2:
                    Fade.FadeOut("TitleScene");
                    break;
            }
        }
    }
}
