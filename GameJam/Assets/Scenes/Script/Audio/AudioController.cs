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
        PLAY,//再生中
        STOP,//ストップ中
        PAUSE,//ポーズ中
    }

    private AudioState audiostate;
    public bool GetIfAudioState(AudioState ifstate) { return audiostate == ifstate ? true : false; }//現在の状態比較


#if UNITY_EDITOR
    //再生位置を確認するよう変数
    [Header("再生位置")]
    public float Time;//書き換えちゃだめよ

    [Header("再生位置変更　秒単位")]
    public float SetTime = 0.0f;//時間を強制的に書き換えるやつ
#endif

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //再生位置確認
        Time = setaudio.time;

        //再生位置変更
        if (SetTime != 0.0f)
        {
            setaudio.time = SetTime;
            SetTime = 0.0f;
        }
#endif

        if (setaudio.loop && loopend != 0.0f)//ループ状態で終わりが設定されている場合
        {
            if (setaudio.time >= loopend)//再生位置が指定した場所にたどり着いたら
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

            if (setaudio.volume >= 1.0f)//音量最大になったら
            {
                setaudio.volume = 1.0f;
                fadein = false;
            }
        }
        else if (fadeout)
        {
            setaudio.volume -= fadespeed;

            if (setaudio.volume <= 0.0f)//音量が０になったら
            {
                switch (audiostate)//フェードアウト後の処理
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
        setaudio = this.GetComponent<AudioSource>();//音再生機能追加
        setaudio.playOnAwake = false;//最初から再生しないようにする
        setaudio.clip = audioClip;//音声データをセット
        setaudio.loop = loop;//ループ処理
        loopstart = audioloopstart;
        loopend = audioloopend;
        audiostate = AudioState.PLAY;//状態セット


        if (fadein)//フェード分岐
        {
            SetFadeIn(fadeflame);
        }
        else
        {
            this.fadein = false;
            fadeout = false;
            setaudio.volume = 1.0f;//音量
            fadespeed = 0.0f;
        }
        //再生
        setaudio.Play();
    }

    //フェードインセット
    private void SetFadeIn(int fadeflame)
    {
        fadein = true;
        fadeout = false;
        setaudio.volume = 0.0f;//音量
        fadespeed = 1.0f / (float)fadeflame;
    }

    //フェードアウトセット
    private void SetFadeOut(int fadeflame)
    {
        fadeout = true;
        fadein = false;

        fadespeed = 1.0f / (float)fadeflame;
    }

    //音のフェードアウト
    public void FadeOutStart(int fadeflame = 60)
    {
        SetFadeOut(fadeflame);
        audiostate = AudioState.STOP;
    }

    //一時停止
    public void Pause()
    {
        setaudio.Pause();
        audiostate = AudioState.PAUSE;
    }

    //一時停止解除
    public void UnPause()
    {
        setaudio.UnPause();
        audiostate = AudioState.PLAY;
    }

    //フェードしながらポーズする
    public void FadeInPause(int fadeflame = 60)
    {
        SetFadeIn(fadeflame);
        audiostate = AudioState.STOP;
    }

    //フェードしながら一時停止解除する
    public void FadeInUnPause(int fadeflame = 60)
    {
        setaudio.UnPause();
        SetFadeOut(fadeflame);
        audiostate = AudioState.PLAY;
    }

    //ストップ
    //注意　オブジェクト事消すのでまた最初から再生する際はAudioManagerから
    //　　　この関数を読んだらオブジェクトを保持してる変数は削除すること
    public void Stop()
    {
        setaudio.Stop();
        this.gameObject.SetActive(false);
    }

    //曲の最後まで再生
    public void EndStop()
    {
        setaudio.loop = false;
    }
}
