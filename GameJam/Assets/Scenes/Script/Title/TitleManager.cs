using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [Header("�X�C�b�`�I�u�W�F�N�g")]
    [Header("���߂���A��������A�����т����A�I���̏��œ����")]
    [SerializeField] GameObject[] Switch;

    //�I����
    public enum SWITCH
    {
        START,//�͂��߂���
        CONTINUE,//�Â�����
        HOWTOPLAY,//�����т���
        EXIT,//�I��
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

    //��i��
    void NextSwicth()
    {
        nextselect = nowselect++;
    }

    //��߂�
    void ReturnSwicth()
    {
        nextselect = nowselect++;
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

    //�͂��߂���ɃA�C�R����ύX
    void ChangeStartIcon()
    {
        Switch[0].SetActive(true);
        Switch[1].SetActive(false);
        Switch[2].SetActive(false);
        Switch[3].SetActive(false);
    }

    //�Â�����ɃA�C�R���ύX
    void ChangeContinueIcon()
    {
        Switch[0].SetActive(false);
        Switch[1].SetActive(true);
        Switch[2].SetActive(false);
        Switch[3].SetActive(false);
    }

    //�����т����ɃA�C�R���ύX
    void ChangeHowToPlayIcon()
    {
        Switch[0].SetActive(false);
        Switch[1].SetActive(false);
        Switch[2].SetActive(true);
        Switch[3].SetActive(false);
    }

    //�Ƃ���ɃA�C�R���ύX
    void ChangeExitIcon()
    {
        Switch[0].SetActive(false);
        Switch[1].SetActive(false);
        Switch[2].SetActive(false);
        Switch[3].SetActive(true);
    }

    //��r
    public bool IfNowSelectSwicth(SWITCH select)
    {
        return nowselect == select ? true : false;
    }
}
