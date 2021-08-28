using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Check : MonoBehaviour
{
    //ステージの選択する
    public enum Game_Stage
    {
        Stage1,     //ステージ１
        Stage2,     //ステージ２
        Stage3      //ステージ３
    };

    [Header("ゴールを置くステージ選択")]
    public Game_Stage Stage;
    [Header("ゴールして動かすアニメーションのオブジェクト")]
    public GameObject friend;
    [Header("シーン移動の遅延")]
    public float Scene_delay;
    [Header("サウンドの遅延")]
    public float Sound_delay;

    //次のシーンの名前を保存する
    string Next_Stage;
    
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
       anim = friend.GetComponent<Animator>();
    }

    void Update()
    {
    
    }

    //ステージ(シーン)を移動する
    void Scene_Move()
    {
        //フェード,シーン移動
        switch (Stage)
        {
        //ステージ１の場合
        case Game_Stage.Stage1:
            //ステージ２に移動(ステージ名追加
            Next_Stage = "stage2";
            break;
            //ステージ２の場合
        case Game_Stage.Stage2:
            //ステージ３に移動(ステージ名追加
            Next_Stage = "stage3";
            break;
        //ステージ３の場合
        case Game_Stage.Stage3:
            //移動(移動先の名前追加予定
            Next_Stage = "Ending";
            break;
        }
        //フェードして次のステージへ
        Fade.FadeOut(Next_Stage);
    }

    void Goal_SE()
    {
        //ゴールSE再生
        AudioManager.PlayAudio("Goal", false, false);
    }
    
    //ゴールオブジェクトに接触したか判定
    void OnTriggerEnter2D(Collider2D collider)
    {
        //接触したオブジェクトがプレイヤーだったら
        if(collider.tag == "Player")
        {
            //ゲージを開けるSE再生
            AudioManager.PlayAudio("GateOpen",false,false);
            //アニメーションを再生する(triggerをonにする
            anim.SetTrigger("Goal");
            Invoke("Goal_SE",Sound_delay);
            //ステージの移動
            Invoke("Scene_Move",Scene_delay);
        }
    }

}
