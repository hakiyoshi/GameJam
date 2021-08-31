using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoController : MonoBehaviour
{
    //トレーラーの入ったオブジェクトを取得する
    public GameObject Video;

    //再生中か判定する用のbool
    bool bPlay;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        bPlay = false;
        
        //初期化時に非表示にする
        Video.SetActive(bPlay);
    }

    // Update is called once per frame
    void Update()
    {
        //Ctl + Vキーの同時押しで再生開始
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V) && !bPlay)
        {
            //全ての音を消す
            AudioManager.AllStopAudio();
            bPlay = true;
        }
        //どこかのキーを押せば通常画面に戻る
        else if (Input.anyKeyDown && !Input.GetKey(KeyCode.Space) && bPlay )
        {
            //タイトルの音を出す
            AudioManager.PlayAudio("Title", false, true);
            bPlay = false;
        }

        //bPlayの値によって再生する
        Video.SetActive(bPlay);
    }
}
