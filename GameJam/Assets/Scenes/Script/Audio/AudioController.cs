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


#if UNITY_EDITOR
    public float Time;
    public float SetTime = 0.0f;
#endif

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        Time = setaudio.time;

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

        //���������I������ꍇ�X�g�b�v����������
        if (!setaudio.isPlaying)
        {
            Stop();
        }
    }

    private void FixedUpdate()
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
                Stop();
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

        

        if (fadein)//�t�F�[�h����
        {
            this.fadein = true;
            fadeout = false;
            setaudio.volume = 0.0f;//����
            fadespeed = 1.0f / (float)fadeflame;
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

    //���̃t�F�[�h�A�E�g
    public void FadeOutStart(int fadeflame = 60)
    {
        fadeout = true;
        fadein = false;

        fadespeed = 1.0f / (float)fadeflame;
    }

    //�ꎞ��~
    public void Pause()
    {
        setaudio.Pause();
    }

    //�ꎞ��~����
    public void UnPause()
    {
        setaudio.UnPause();
    }

    //�X�g�b�v
    //���Ӂ@�I�u�W�F�N�g�������̂ł܂��ŏ�����Đ�����ۂ�AudioManager����
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
