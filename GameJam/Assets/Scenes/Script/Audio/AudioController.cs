using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource setaudio = null;


    private bool fadein;
    public bool FadeIn { get { return fadein; } }

    private bool fadeout;
    public bool FadeOut { get { return fadeout; } }


    private float fadespeed;
    private float loopstart;
    private float loopend;

    public enum AudioState
    {
        PLAY,//�Đ���
        STOP,//�X�g�b�v��
        PAUSE,//�|�[�Y��
    }

    private AudioState audiostate;
    public bool GetIfAudioState(AudioState ifstate) { return audiostate == ifstate ? true : false; }//���݂̏�Ԕ�r


#if UNITY_EDITOR
    //�Đ��ʒu���m�F����悤�ϐ�
    [Header("�Đ��ʒu")]
    public float Time;//�����������Ⴞ�߂�

    [Header("�Đ��ʒu�ύX�@�b�P��")]
    public float SetTime = 0.0f;//���Ԃ������I�ɏ�����������
#endif

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //�Đ��ʒu�m�F
        Time = setaudio.time;

        //�Đ��ʒu�ύX
        if (SetTime != 0.0f)
        {
            setaudio.time = SetTime;
            SetTime = 0.0f;
        }
#endif

        if (setaudio.loop && loopend != 0.0f)//���[�v��ԂŏI��肪�ݒ肳��Ă���ꍇ
        {
            if (setaudio.time >= loopend)//�Đ��ʒu���w�肵���ꏊ�ɂ��ǂ蒅������
            {
                setaudio.time = loopstart;
            }
        }
        
        if (setaudio.clip.length <= setaudio.time)
        {
            Stop();
        }
    }

    private void FixedUpdate()
    {
        FadeProcess();
    }

    private void FadeProcess()
    {
        if (fadein)
        {
            setaudio.volume += fadespeed;

            if (setaudio.volume >= 1.0f)//���ʍő�ɂȂ�����
            {
                setaudio.volume = 1.0f;
                fadein = false;
            }
        }
        else if (fadeout)
        {
            setaudio.volume -= fadespeed;

            if (setaudio.volume <= 0.0f)//���ʂ��O�ɂȂ�����
            {
                switch (audiostate)//�t�F�[�h�A�E�g��̏���
                {
                    case AudioState.STOP:
                        Stop();
                        break;
                    case AudioState.PAUSE:
                        Pause();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void PlayAudio(AudioClip audioClip, bool fadein, bool loop, int fadeflame = 60, float audioloopstart = 0.0f, float audioloopend = 0.0f)
    {
        setaudio = this.GetComponent<AudioSource>();//���Đ��@�\�ǉ�
        setaudio.playOnAwake = false;//�ŏ�����Đ����Ȃ��悤�ɂ���
        setaudio.clip = audioClip;//�����f�[�^���Z�b�g
        setaudio.loop = loop;//���[�v����
        loopstart = audioloopstart;
        loopend = audioloopend;
        audiostate = AudioState.PLAY;//��ԃZ�b�g


        if (fadein)//�t�F�[�h����
        {
            SetFadeIn(fadeflame);
        }
        else
        {
            this.fadein = false;
            fadeout = false;
            setaudio.volume = 1.0f;//����
            fadespeed = 0.0f;
        }
        //�Đ�
        setaudio.Play();
    }

    //�t�F�[�h�C���Z�b�g
    private void SetFadeIn(int fadeflame)
    {
        fadein = true;
        fadeout = false;
        setaudio.volume = 0.0f;//����
        fadespeed = 1.0f / (float)fadeflame;
    }

    //�t�F�[�h�A�E�g�Z�b�g
    private void SetFadeOut(int fadeflame)
    {
        fadeout = true;
        fadein = false;

        fadespeed = 1.0f / (float)fadeflame;
    }

    //���̃t�F�[�h�A�E�g
    public void FadeOutStart(int fadeflame = 60)
    {
        SetFadeOut(fadeflame);
        audiostate = AudioState.STOP;
    }

    //�ꎞ��~
    public void Pause()
    {
        setaudio.Pause();
        audiostate = AudioState.PAUSE;
    }

    //�ꎞ��~����
    public void UnPause()
    {
        setaudio.UnPause();
        audiostate = AudioState.PLAY;
    }

    //�t�F�[�h���Ȃ���|�[�Y����
    public void FadeInPause(int fadeflame = 60)
    {
        SetFadeIn(fadeflame);
        audiostate = AudioState.STOP;
    }

    //�t�F�[�h���Ȃ���ꎞ��~��������
    public void FadeInUnPause(int fadeflame = 60)
    {
        setaudio.UnPause();
        SetFadeOut(fadeflame);
        audiostate = AudioState.PLAY;
    }

    //�X�g�b�v
    //���Ӂ@�I�u�W�F�N�g�������̂ł܂��ŏ�����Đ�����ۂ�AudioManager����
    //�@�@�@���̊֐���ǂ񂾂�I�u�W�F�N�g��ێ����Ă�ϐ��͍폜���邱��
    public void Stop()
    {
        setaudio.Stop();
        this.gameObject.SetActive(false);
    }

    //�Ȃ̍Ō�܂ōĐ�
    public void EndStop()
    {
        setaudio.loop = false;
    }
}
