using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("ポーズ背景")]
    [SerializeField] GameObject Selects;
    [Header("ポーズ矢印")]
    [SerializeField] GameObject Arrow;
    [Header("ポーズ操作説明画像")]
    [SerializeField] GameObject Infomation;

    GameObject[] Select;

    int SelectCount;

    bool bPause;

    bool bInfomation;

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
        Infomation.SetActive(bPause);

        Fade.FadeIn();
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

        if(!bPause)
            Debug.Log(bPause);
    }

    void ChangeTimeScale()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bPause)
            {
                bPause = false;
                bInfomation = false;
            }
            else if (!bPause)
                bPause = true;

            if (bPause)
            {
                Selects.SetActive(bPause);
                Arrow.SetActive(bPause);
                ResetSelects();
            }
        }

        if (bPause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
       
        Selects.SetActive(bPause);
        Arrow.SetActive(bPause);
        Infomation.SetActive(bInfomation);
    }

    void DownSelect()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SelectCount++;

            if (SelectCount == Select.Length)
            {
                SelectCount = 0;
                Arrow.transform.localPosition = new Vector3(-500, 220, 0);
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
                SelectCount = Select.Length -1;
                Arrow.transform.localPosition = new Vector3(-500, -245, 0);
            }
            else
                Arrow.transform.localPosition += new Vector3(0, 155, 0);
        }
    }

    void NowSelect()
    {
        for (int i = 0; i < Select.Length; i++)
        {
            if (i == SelectCount)
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            else
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    void ResetSelects()
    {
        for (int i = 0; i < Select.Length; i++)
        {
            if (i == 0)
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            else
                Select[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }

        Arrow.transform.localPosition = new Vector3(-500,220,0);

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
                    bPause = false;
                    Fade.FadeOut(SceneManager.GetActiveScene().name);

                    break;
                case 2:
                    bPause = false;
                    Fade.FadeOut("TitleScene");
                    break;
                case 3:
                    bInfomation = !bInfomation;
                    Infomation.SetActive(bInfomation);
                    Selects.SetActive(!bInfomation);
                    Arrow.SetActive(!bInfomation);
                    break;
            }
        }
    }
}
