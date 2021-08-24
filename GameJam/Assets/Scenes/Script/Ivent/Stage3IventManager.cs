using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3IventManager : IventManager
{
    [Header("�����ړ����ŗ���BGM")]
    [SerializeField] PlaySound ForceSound;

    [Header("�����ړ��I����ɗ���BGM")]
    [SerializeField] PlaySound EndForceSound;

    private ChangeCamera change;

    private AudioController audiocon;//���R���g���[���[

    private void Start()
    {
        change = Camera.main.GetComponent<ChangeCamera>();

        StartCoroutine("ChackStartForceSound");
    }

    //�����ړ��`�F�b�N
    IEnumerator ChackStartForceSound()
    {
        while (true)
        {
            if (change.IfCameraFlag(ChangeCamera.CAMERAFLAG.DOLLY))
            {
                audiocon = PlayAudio(ForceSound);
                StartCoroutine("ChackEndForceSound");
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
        
    }

    //�����ړ��I���`�F�b�N
    IEnumerator ChackEndForceSound()
    {
        while (true)
        {
            if (change.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN))
            {
                audiocon.FadeOutStart();
                audiocon = PlayAudio(EndForceSound);
                StartCoroutine("ChackClear");
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ChackClear()
    {
        yield break;
    }

    //�ȍĐ�
    AudioController PlayAudio(PlaySound sound)
    {
        return AudioManager.PlayAudio(sound.name, sound.loop, sound.fade);
    }
}
