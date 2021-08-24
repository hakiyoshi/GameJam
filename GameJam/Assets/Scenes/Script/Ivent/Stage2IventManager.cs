using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2IventManager : IventManager
{
    [Header("�J�n���ɗ���BGM���")]
    [SerializeField] PlaySound StartSound;

    [Header("�����X�N���[���J�n���ɗ���BGM���")]
    [SerializeField] PlaySound ForcedSound;
    [SerializeField] ChangeCamera ChangeCamera;

    //���ݍĐ����̉��f�[�^
    private AudioController audiodata = null;


    //�t���O����
    private enum STAGEFLAG
    {
        START,//�J�n
        FORCED,//�����ړ�
        AFTERFORCED,//�����ړ���
    }

    //�T�E���h�t���O
    private STAGEFLAG soundflag = STAGEFLAG.START;
    private bool GetIfSoundFlag(STAGEFLAG flag) { return soundflag == flag ? true : false; }

    // Start is called before the first frame update
    void Start()
    {
        //�n�܂��ĊJ���Đ�����
        audiodata = PlayAudio(StartSound);

        //���򏈗��J�n
        StartCoroutine("ChackForced");
    }

    //�����ړ��J�n���Ă��邩
    IEnumerator ChackForced()
    {
        while (true)
        {
            if (ChangeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.DOLLY))
            {
                audiodata.FadeOutStart();
                audiodata = PlayAudio(ForcedSound);
                StartCoroutine("ChackAfterForced");
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //�����ړ��ォ
    IEnumerator ChackAfterForced()
    {
        while (true)
        {
            if (ChangeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN))
            {
                audiodata.FadeOutStart();
                audiodata = PlayAudio(StartSound);
                StartCoroutine("ChackClear");
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //�N���A������
    IEnumerator ChackClear()
    {
        while (true)
        {
            if (true)
            {
                yield break;
            }

            //yield return new WaitForFixedUpdate();
        }
    }
}
