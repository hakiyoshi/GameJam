using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End_Branch : MonoBehaviour
{
    [Header("imageスプライトの表示順(上から)")]
    //後から増やすことも可能
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    [Header("imageスプライトの数")]
    public int image_Num;

    [Header("image1の分岐画像")]
    public Sprite Good_sprite;
    public Sprite Normal_sprite;
    public Sprite Bad_sprite;

    [Header("文字列の透明度を変化させるスピード0.0f～1.0f")]
    public float Speed;

    [Header("テキストを表示し終わってからシーンを移動するまでの時間")]
    public float Limit_time;

    //カラーチャンネル用の変数
    float alfa;     //透明度
    float red;      //赤
    float green;    //緑
    float blue;     //青
    //現在表示しているimage
    int Now_imageNo;
    //デスカウントをゲットする変数
    int Get_Count;
    //エンディングが全部見せ終わったフラグ
    bool Text_Flag;
    //現在の時間と制限時間
    float Now_time;

    //イメージを分岐する
    void Image_Branch()
    {
        //イメージをセットする
        if (Get_Count == 0)
        {
            image1.sprite = Good_sprite;
        }
        else if (Get_Count > 0 && Get_Count < 8)
        {
            image1.sprite = Normal_sprite;
        }
        else
        {
            image1.sprite = Bad_sprite;
        }
    }

    //変数の初期化
    void Init()
    {
        //テキストフラグを倒す
        Text_Flag = false;
        //時間の初期化宣言をする
        Now_time = 0.0f;
        //カラーチャンネルの変数の初期化
        alfa = 0.0f;
        red = 255.0f;
        green = 255.0f;
        blue = 255.0f;
        //現在表示しているimage番号
        Now_imageNo = 1;
    }

    //image
    void Letter_Display()
    {
        //imageスプライトの表示分だけswitch処理を増築する
        switch (Now_imageNo)
        {
        case 1:
            image1.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        case 2:
            image2.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        case 3:
            image3.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        case 4:
            image4.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            break;
        default:
            Text_Flag = true;
            break;
        }
        //alfaの変数の値によって処理を変更
        if(alfa >= 1.0f && Text_Flag == false)
        {
            alfa = 0.0f;
            Now_imageNo++;
        }
        else if(Text_Flag == false)
        {
            alfa += Speed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //デスカウントの値を入手する
        Get_Count = Ending_Manager.GetDead_Count();
        //イメージ分岐をする
        Image_Branch();
        //変数の初期化
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if(Text_Flag == false)
        {
            //文字を表示するための処理
            Letter_Display();
        }
        else
        {
            //直前のフレームから経過した時間を増やす
            Now_time += Time.deltaTime;

            if(Now_time >= Limit_time)
            {
                //タイトルに移動する
                Fade.FadeOut("TitleScene");
            }
        }

    }
}
