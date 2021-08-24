using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [Header("スイッチオブジェクト")]
    [Header("初めから、続きから、あそびかた、終了の順で入れる")]
    [SerializeField] GameObject[] Switch;

    //選択肢
    public enum SWITCH
    {
        START,//はじめから
        CONTINUE,//つづきから
        HOWTOPLAY,//あそびかた
        EXIT,//終了
    }

    [SerializeField] SWITCH StartSelect = SWITCH.START;
    private SWITCH nowselect;
    private SWITCH nextselect;

    private void Start()
    {
        nowselect = StartSelect;
        nextselect = StartSelect;
    }

    private void Update()
    {
        InputProcess();
    }

    void InputProcess()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

        }
    }

    //一個進む
    void NextSwicth()
    {
        nextselect = nowselect++;
    }

    //一個戻る
    void ReturnSwicth()
    {
        nextselect = nowselect++;
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
            case SWITCH.START:

                break;
            case SWITCH.CONTINUE:
                break;
            case SWITCH.HOWTOPLAY:
                break;
            case SWITCH.EXIT:
                break;
            default:
                break;
        }
    }

    //はじめからにアイコンを変更
    void ChangeStartIcon()
    {
        Switch[0].SetActive(true);
        Switch[1].SetActive(false);
        Switch[2].SetActive(false);
        Switch[3].SetActive(false);
    }

    //つづきからにアイコン変更
    void ChangeContinueIcon()
    {
        Switch[0].SetActive(false);
        Switch[1].SetActive(true);
        Switch[2].SetActive(false);
        Switch[3].SetActive(false);
    }

    //あそびかたにアイコン変更
    void ChangeHowToPlayIcon()
    {
        Switch[0].SetActive(false);
        Switch[1].SetActive(false);
        Switch[2].SetActive(true);
        Switch[3].SetActive(false);
    }

    //とじるにアイコン変更
    void ChangeExitIcon()
    {
        Switch[0].SetActive(false);
        Switch[1].SetActive(false);
        Switch[2].SetActive(false);
        Switch[3].SetActive(true);
    }

    //比較
    public bool IfNowSelectSwicth(SWITCH select)
    {
        return nowselect == select ? true : false;
    }
}
