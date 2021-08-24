using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2IventManager : IventManager
{
    [Header("開始時に流すBGM情報")]
    [SerializeField] PlaySound StartSound;

    [Header("強制スクロール開始時に流すBGM情報")]
    [SerializeField] PlaySound ForcedSound;
    [SerializeField] ChangeCamera ChangeCamera;

    //現在再生中の音データ
    private AudioController audiodata = null;


    //フラグ処理
    private enum STAGEFLAG
    {
        START,//開始
        FORCED,//強制移動
        AFTERFORCED,//強制移動後
    }

    //サウンドフラグ
    private STAGEFLAG soundflag = STAGEFLAG.START;
    private bool GetIfSoundFlag(STAGEFLAG flag) { return soundflag == flag ? true : false; }

    // Start is called before the first frame update
    void Start()
    {
        //始まって開幕再生する
        audiodata = PlayAudio(StartSound);

        //分岐処理開始
        StartCoroutine("ChackForced");
    }

    //強制移動開始しているか
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

    //強制移動後か
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

    //クリアしたか
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
