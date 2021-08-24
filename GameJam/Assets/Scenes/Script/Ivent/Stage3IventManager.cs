using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3IventManager : IventManager
{
    [Header("強制移動中で流すBGM")]
    [SerializeField] PlaySound ForceSound;

    [Header("強制移動終了後に流すBGM")]
    [SerializeField] PlaySound EndForceSound;

    private ChangeCamera change;

    private AudioController audiocon;//音コントローラー

    private void Start()
    {
        change = Camera.main.GetComponent<ChangeCamera>();

        StartCoroutine("ChackStartForceSound");
    }

    //強制移動チェック
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

    //強制移動終了チェック
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

    //曲再生
    AudioController PlayAudio(PlaySound sound)
    {
        return AudioManager.PlayAudio(sound.name, sound.loop, sound.fade);
    }
}
