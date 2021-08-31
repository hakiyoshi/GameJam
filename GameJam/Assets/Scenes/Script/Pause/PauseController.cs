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
    [Header("ポーズあそびかた画像")]
    [SerializeField] GameObject Infomation;

    //セレクトボタン格納用配列
    GameObject[] Select;

    //現在のセレクトボタンの番号
    int SelectNumber;

    //ポーズしているかどうかのbool
    bool bPause;

    //あそびかたを選択しているかどうかのbool
    bool bInfomation;

    bool bChangeScene;

    // Start is called before the first frame update
    void Start()
    {
        //セレクトボタンの配列初期化
        Select = new GameObject[4];

        //各セレクトボタン設定
        for (int i = 0; i < Select.Length; i++)
        {
            Select[i] = Selects.transform.GetChild(i).gameObject;
        }

        //ResetSelectsメソッド呼び出し
        ResetSelects();

        //ポーズ画面を開かないようにする
        bPause = false;

        //各オブジェクトを非表示にする
        Selects.SetActive(bPause);
        Arrow.SetActive(bPause);
        Infomation.SetActive(bPause);

        bChangeScene = false;

        Fade.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeTimeScaleメソッド呼び出し
        ChangeTimeScale();

        //ポーズ中なら実行する
        if (bPause)
        {
            DownSelect();

            UpSelect();

            NowSelect();

            PushSelect();
        }

    }

    //時間を止めるメソッド
    void ChangeTimeScale()
    {
        //エスケープキーが押されたか判定する
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ポーズ画面が開いていたらポーズ画面を閉じる
            if (bPause)
            {
                bPause = false;
                bInfomation = false;
                Selects.SetActive(bPause);
                Arrow.SetActive(bPause);
                ResetSelects();
            }
            //ポーズ画面が閉じていたらポーズ画面を開く
            else if (!bPause)
            {
                bPause = true;
            }
        }

        //ポーズ中なら時間を止める
        if (bPause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        //各オブジェクトの表示設定
        Selects.SetActive(bPause);
        Arrow.SetActive(bPause);
        Infomation.SetActive(bInfomation);
    }


    void DownSelect()
    {
        //Sキーが押されたか判定する
        if (Input.GetKeyDown(KeyCode.S))
        {
            //セレクトボタンの番号を+1する
            SelectNumber++;

            AudioManager.PlayAudio("TitleMoveCursor", false, false);

            //一番下のボタンでSキーを押したら一番上のボタンに移動させる
            if (SelectNumber == Select.Length)
            {
                SelectNumber = 0;
                Arrow.transform.localPosition = new Vector3(-500, 220, 0);
            }
            //矢印を下に移動させる
            else
            {
                Arrow.transform.localPosition += new Vector3(0, -155, 0);
            }
        }
    }

    void UpSelect()
    {
        //Wキーが押されたか判定する
        if (Input.GetKeyDown(KeyCode.W))
        {
            //セレクトボタンの番号を-1する
            SelectNumber--;

            AudioManager.PlayAudio("TitleMoveCursor", false, false);

            //一番上のボタンでWキーを押したら一番下のボタンに移動させる
            if (SelectNumber == -1)
            {
                SelectNumber = Select.Length - 1;
                Arrow.transform.localPosition = new Vector3(-500, -245, 0);
            }
            //矢印を上に移動させる
            else
            {
                Arrow.transform.localPosition += new Vector3(0, 155, 0);
            }
        }
    }

    void NowSelect()
    {
        for (int i = 0; i < Select.Length; i++)
        {
            if (i == SelectNumber)
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

        Arrow.transform.localPosition = new Vector3(-500, 220, 0);

        SelectNumber = 0;
    }

    void PushSelect()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.PlayAudio("TitleDecision", false, false);

            switch (SelectNumber)
            {
                case 0:
                    bPause = false;
                    break;
                case 1:
                    bPause = false;
                    Fade.FadeOut(SceneManager.GetActiveScene().name);
                    bChangeScene = true;
                    break;
                case 2:
                    bPause = false;
                    Fade.FadeOut("TitleScene");
                    bChangeScene = true;
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

    public bool GetbChangeScene()
    {
        return bChangeScene;
    }
}
