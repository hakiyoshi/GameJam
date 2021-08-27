using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [Header("スイッチオブジェクト")]
    [Header("初めから、続きから、あそびかた、終了の順で入れる")]
    [SerializeField] GameObject[] Switch;

    [Header("アイコンオブジェクト")]
    [Header("初めから、続きから、あそびかた、終了の順で入れる")]
    [SerializeField] GameObject[] Icon;

    //選択肢
    public enum SWITCH
    {
        NEWPLAY = 0,//はじめから
        //CONTINUE,//つづきから
        HOWTOPLAY,//あそびかた
        EXIT,//終了
        MAX,
    }

    //選択しているコマンド
    [SerializeField] SWITCH StartSelect = SWITCH.NEWPLAY;
    private SWITCH nowselect;
    private SWITCH nextselect;

    private bool select = false;

    private bool inputflag = true;
    private TitleCommand command;

    private void Start()
    {
        nowselect = StartSelect;
        nextselect = StartSelect;

        command = this.GetComponent<TitleCommand>();

        ChangeIcon(nowselect);
    }

    private void Update()
    {
        if (select)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                command.CloseHowToPlayCommand();
                select = false;
                inputflag = true;
            }
            return;
        }

        if (inputflag)
        {
            InputProcess();
        }

        if (nextselect != nowselect)//次と今が違う場合
        {
            ChangeSelect();
            ChangeIcon(nowselect);
        }
    }

    void InputProcess()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            ReturnSwicth();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            NextSwicth();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            command.ExeCommand(nowselect);
            inputflag = false;
            select = true;
        }
    }

    //一個進む
    void NextSwicth()
    {
        //最大の場合
        if (nowselect == SWITCH.EXIT)
            return;

        nextselect = nowselect + 1;
    }

    //一個戻る
    void ReturnSwicth()
    {
        if (nowselect == SWITCH.NEWPLAY)
            return;

        nextselect = nowselect - 1;
    }

    //次の選択を今の選択にする
    void ChangeSelect()
    {
        nowselect = nextselect;
    }

    void ChangeIcon(SWITCH change)
    {
        switch (change)
        {
            case SWITCH.NEWPLAY:
                ChangeNewPlayIcon();
                break;
            //case SWITCH.CONTINUE:
                //ChangeContinueIcon();
                //break;
            case SWITCH.HOWTOPLAY:
                ChangeHowToPlayIcon();
                break;
            case SWITCH.EXIT:
                ChangeExitIcon();
                break;
            default:
                break;
        }
    }

    //はじめからにアイコンを変更
    void ChangeNewPlayIcon()
    {
        //選択
        Switch[0].SetActive(true);
        Switch[1].SetActive(false);
        Switch[2].SetActive(false);
        //Switch[3].SetActive(false);

        Switch[0].GetComponent<SelectAnimation>().Open(0.1f);
        Switch[1].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[2].GetComponent<SelectAnimation>().Close(0.1f);

        //アイコン
        for (int i = 0; i < Icon.Length; i++)
        {
            Icon[i].SetActive(!Switch[i].activeSelf);
        }
    }

    ////つづきからにアイコン変更
    //void ChangeContinueIcon()
    //{
    //    //選択
    //    Switch[0].SetActive(false);
    //    Switch[1].SetActive(true);
    //    Switch[2].SetActive(false);
    //    //Switch[3].SetActive(false);

    //    //アイコン
    //    for (int i = 0; i < Icon.Length; i++)
    //    {
    //        Icon[i].SetActive(!Switch[i].activeSelf);
    //    }
    //}

    //あそびかたにアイコン変更
    void ChangeHowToPlayIcon()
    {
        //選択
        Switch[0].SetActive(false);
        Switch[1].SetActive(true);
        Switch[2].SetActive(false);
        //Switch[3].SetActive(false);

        Switch[0].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[1].GetComponent<SelectAnimation>().Open(0.1f);
        Switch[2].GetComponent<SelectAnimation>().Close(0.1f);

        //アイコン
        for (int i = 0; i < Icon.Length; i++)
        {
            Icon[i].SetActive(!Switch[i].activeSelf);
        }
    }

    //とじるにアイコン変更
    void ChangeExitIcon()
    {
        //選択
        Switch[0].SetActive(false);
        Switch[1].SetActive(false);
        Switch[2].SetActive(true);
        //Switch[3].SetActive(true);

        Switch[0].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[1].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[2].GetComponent<SelectAnimation>().Open(0.1f);

        //アイコン
        for (int i = 0; i < Icon.Length; i++)
        {
            Icon[i].SetActive(!Switch[i].activeSelf);
        }
    }

    //比較
    public bool IfNowSelectSwicth(SWITCH select)
    {
        return nowselect == select ? true : false;
    }

    //ゲッター
    private SWITCH GetNowSelect() { return nowselect; }
}
