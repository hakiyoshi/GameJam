using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End_Branch : MonoBehaviour
{
    Image image;

    [Header("エンディング分岐のスプライト")]
    public Sprite Good_sprite;
    public Sprite Normal_sprite;
    public Sprite Bad_sprite;

    //デスカウントをゲットする変数
    int Get_Count;
    //エンディングが全部見せ終わったフラグ
    bool Text_Flag;
    //現在の時間と制限時間
    float Now_time;
    [Header("テキストを表示し終わってからシーンを移動するまでの時間")]
    public float Limit_time;

    //イメージを分岐する
    void Image_Branch()
    {
        //イメージをセットする
        if (Get_Count == 0)
        {
            image.sprite = Good_sprite;
        }
        else if (Get_Count > 0 && Get_Count < 7)
        {
            image.sprite = Normal_sprite;
        }
        else
        {
            image.sprite = Bad_sprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        //デスカウントの値を入手する
        Get_Count = Ending_Manager.GetDead_Count();
        //イメージ分岐をする
        Image_Branch();
        //テキストフラグを倒す
        Text_Flag = false;
        //時間の初期化宣言をする
        Now_time = 0.0f;
    }

    

    // Update is called once per frame
    void Update()
    {   
        if(Text_Flag == false)
        {
            //文字を表示するための処理

        }
        else
        {
            //直前のフレームから経過した時間を増やす
            Now_time += Time.deltaTime;

            if(Now_time >= Limit_time)
            {
                //タイトルに移動する
                Fade.FadeOut("Title");
            }
        }

    }
}
