using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("�|�[�Y�w�i")]
    [SerializeField] GameObject Selects;
    [Header("�|�[�Y���")]
    [SerializeField] GameObject Arrow;
    [Header("�|�[�Y�����т����摜")]
    [SerializeField] GameObject Infomation;

    //�Z���N�g�{�^���i�[�p�z��
    GameObject[] Select;

    //���݂̃Z���N�g�{�^���̔ԍ�
    int SelectNumber;

    //�|�[�Y���Ă��邩�ǂ�����bool
    bool bPause;

    //�����т�����I�����Ă��邩�ǂ�����bool
    bool bInfomation;

    bool bChangeScene;

    // Start is called before the first frame update
    void Start()
    {
        //�Z���N�g�{�^���̔z�񏉊���
        Select = new GameObject[4];

        //�e�Z���N�g�{�^���ݒ�
        for (int i = 0; i < Select.Length; i++)
        {
            Select[i] = Selects.transform.GetChild(i).gameObject;
        }

        //ResetSelects���\�b�h�Ăяo��
        ResetSelects();

        //�|�[�Y��ʂ��J���Ȃ��悤�ɂ���
        bPause = false;

        //�e�I�u�W�F�N�g���\���ɂ���
        Selects.SetActive(bPause);
        Arrow.SetActive(bPause);
        Infomation.SetActive(bPause);

        bChangeScene = false;

        Fade.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeTimeScale���\�b�h�Ăяo��
        ChangeTimeScale();

        //�|�[�Y���Ȃ���s����
        if (bPause)
        {
            DownSelect();

            UpSelect();

            NowSelect();

            PushSelect();
        }

    }

    //���Ԃ��~�߂郁�\�b�h
    void ChangeTimeScale()
    {
        //�G�X�P�[�v�L�[�������ꂽ�����肷��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //�|�[�Y��ʂ��J���Ă�����|�[�Y��ʂ����
            if (bPause)
            {
                bPause = false;
                bInfomation = false;
                Selects.SetActive(bPause);
                Arrow.SetActive(bPause);
                ResetSelects();
            }
            //�|�[�Y��ʂ����Ă�����|�[�Y��ʂ��J��
            else if (!bPause)
            {
                bPause = true;
            }
        }

        //�|�[�Y���Ȃ玞�Ԃ��~�߂�
        if (bPause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        //�e�I�u�W�F�N�g�̕\���ݒ�
        Selects.SetActive(bPause);
        Arrow.SetActive(bPause);
        Infomation.SetActive(bInfomation);
    }


    void DownSelect()
    {
        //S�L�[�������ꂽ�����肷��
        if (Input.GetKeyDown(KeyCode.S))
        {
            //�Z���N�g�{�^���̔ԍ���+1����
            SelectNumber++;

            AudioManager.PlayAudio("TitleMoveCursor", false, false);

            //��ԉ��̃{�^����S�L�[�����������ԏ�̃{�^���Ɉړ�������
            if (SelectNumber == Select.Length)
            {
                SelectNumber = 0;
                Arrow.transform.localPosition = new Vector3(-500, 220, 0);
            }
            //�������Ɉړ�������
            else
            {
                Arrow.transform.localPosition += new Vector3(0, -155, 0);
            }
        }
    }

    void UpSelect()
    {
        //W�L�[�������ꂽ�����肷��
        if (Input.GetKeyDown(KeyCode.W))
        {
            //�Z���N�g�{�^���̔ԍ���-1����
            SelectNumber--;

            AudioManager.PlayAudio("TitleMoveCursor", false, false);

            //��ԏ�̃{�^����W�L�[�����������ԉ��̃{�^���Ɉړ�������
            if (SelectNumber == -1)
            {
                SelectNumber = Select.Length - 1;
                Arrow.transform.localPosition = new Vector3(-500, -245, 0);
            }
            //������Ɉړ�������
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
