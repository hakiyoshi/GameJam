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

    //次のシーンの名前を保存する
    string Next_Stage;

    // Start is called before the first frame update
    void Start()
    {
       
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
            
            break;
        }
        //フェードして次のステージへ
        Fade.FadeOut(Next_Stage);
    }
    
    //ゴールオブジェクトに接触したか判定
    void OnTriggerEnter2D(Collider2D collider)
    {
        //接触したオブジェクトがプレイヤーだったら
        if(collider.tag == "Player")
        {
            //ゴールSE再生(名前追加予定
            //AudioManager.PlayAudio("",false,false);
            //ステージの移動
            Scene_Move();
        }
    }

}
