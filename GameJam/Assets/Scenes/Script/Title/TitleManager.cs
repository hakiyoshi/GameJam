using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [Header("�X�C�b�`�I�u�W�F�N�g")]
    [Header("���߂���A��������A�����т����A�I���̏��œ����")]
    [SerializeField] GameObject[] Switch;

    [Header("�A�C�R���I�u�W�F�N�g")]
    [Header("���߂���A��������A�����т����A�I���̏��œ����")]
    [SerializeField] GameObject[] Icon;

    //�I����
    public enum SWITCH
    {
        NEWPLAY = 0,//�͂��߂���
        //CONTINUE,//�Â�����
        HOWTOPLAY,//�����т���
        EXIT,//�I��
        MAX,
    }

    //�I�����Ă���R�}���h
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

        if (nextselect != nowselect)//���ƍ����Ⴄ�ꍇ
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

    //��i��
    void NextSwicth()
    {
        //�ő�̏ꍇ
        if (nowselect == SWITCH.EXIT)
            return;

        nextselect = nowselect + 1;
    }

    //��߂�
    void ReturnSwicth()
    {
        if (nowselect == SWITCH.NEWPLAY)
            return;

        nextselect = nowselect - 1;
    }

    //���̑I�������̑I���ɂ���
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

    //�͂��߂���ɃA�C�R����ύX
    void ChangeNewPlayIcon()
    {
        //�I��
        Switch[0].SetActive(true);
        Switch[1].SetActive(false);
        Switch[2].SetActive(false);
        //Switch[3].SetActive(false);

        Switch[0].GetComponent<SelectAnimation>().Open(0.1f);
        Switch[1].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[2].GetComponent<SelectAnimation>().Close(0.1f);

        //�A�C�R��
        for (int i = 0; i < Icon.Length; i++)
        {
            Icon[i].SetActive(!Switch[i].activeSelf);
        }
    }

    ////�Â�����ɃA�C�R���ύX
    //void ChangeContinueIcon()
    //{
    //    //�I��
    //    Switch[0].SetActive(false);
    //    Switch[1].SetActive(true);
    //    Switch[2].SetActive(false);
    //    //Switch[3].SetActive(false);

    //    //�A�C�R��
    //    for (int i = 0; i < Icon.Length; i++)
    //    {
    //        Icon[i].SetActive(!Switch[i].activeSelf);
    //    }
    //}

    //�����т����ɃA�C�R���ύX
    void ChangeHowToPlayIcon()
    {
        //�I��
        Switch[0].SetActive(false);
        Switch[1].SetActive(true);
        Switch[2].SetActive(false);
        //Switch[3].SetActive(false);

        Switch[0].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[1].GetComponent<SelectAnimation>().Open(0.1f);
        Switch[2].GetComponent<SelectAnimation>().Close(0.1f);

        //�A�C�R��
        for (int i = 0; i < Icon.Length; i++)
        {
            Icon[i].SetActive(!Switch[i].activeSelf);
        }
    }

    //�Ƃ���ɃA�C�R���ύX
    void ChangeExitIcon()
    {
        //�I��
        Switch[0].SetActive(false);
        Switch[1].SetActive(false);
        Switch[2].SetActive(true);
        //Switch[3].SetActive(true);

        Switch[0].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[1].GetComponent<SelectAnimation>().Close(0.1f);
        Switch[2].GetComponent<SelectAnimation>().Open(0.1f);

        //�A�C�R��
        for (int i = 0; i < Icon.Length; i++)
        {
            Icon[i].SetActive(!Switch[i].activeSelf);
        }
    }

    //��r
    public bool IfNowSelectSwicth(SWITCH select)
    {
        return nowselect == select ? true : false;
    }

    //�Q�b�^�[
    private SWITCH GetNowSelect() { return nowselect; }
}
