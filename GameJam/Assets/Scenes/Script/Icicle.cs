using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    [Header("キャラクターのオブジェクトを選択する")]
    public GameObject Charcter;
    //キャラクターのポジションを取得
    Vector3 CharPos;
    //つららの座標を取得
    Vector3 Ice;
    //ギミック自体の座標
    float IcePos;
    [Header("つららが反応する範囲")]
    public float Range;
    [Header("つららが落ちる速度0.0f～1.0f")]
    public float YSpeed;
    [Header("つららの加速度(1フレームごと)")]
    public float Boost;
    [Header("最大加速度")]
    public float Max_Boost;
    [Header("つららの復活する高さ(元あった座標から)")]
    public float YHight;
    [Header("つららが復活する速度0.0f～1.0f")]
    public float ReSpeed;
    //つららが落下できる状態かフラグを立てる
    //-2:移動なし -1:つらら生成 0:落下範囲の設定 1:つららの落下
    int Fall_Flag;
    //プレイヤーのデスのフラグをゲットする
    bool Re_Flag;
    //つららの速度の加速度
    float Accele;
    //PlayerGimmick のスクリプトをゲットする
    private PlayerGimmick PG;

    // Start is called before the first frame update
    void Start()
    {
        //キャラクターのオブジェクトのコンポーネントを取得する
        PG = Charcter.GetComponent<PlayerGimmick>();
        //つららの座標を取得
        Ice = this.transform.position;
        IcePos = Ice.y;
        //落下フラグを立てる
        Fall_Flag = 0;
        //加速度の値を初期化する
        Accele = 1.0f;
    }
    //落下の判定をする
    void Fall()
    {
        //キャラクターの座標を取得する
        CharPos = Charcter.transform.position;
        //x座標距離の絶対値を算出する
        float Dis = Mathf.Abs(Ice.x - CharPos.x);
        //キャラクターとつららの範囲を確認する
        if (Dis < Range || Fall_Flag == 1)
        {
            //つららを落とすフラグを1にする
            Fall_Flag = 1;
            //落下処理
            IcePos -= YSpeed * Accele;
            if(Accele < Max_Boost) { 
                Accele += Boost;
            }
            else
            {
                Accele = Max_Boost;
            }
        }
    }
    //つららが上から生えてくる処理
    void Re()
    {
        //つららが生えてくる速度
        IcePos -= ReSpeed;
        //つららが生えている初期の座標よりも低いか判断する
        if (IcePos < Ice.y)
        {
            //つららのy座標の変更
            IcePos = Ice.y;
            //つららのフラグを0にする
            Fall_Flag = 0;
        }
    }
    void Reset()
    {
        Fall_Flag = -2;
        IcePos = Ice.y + YHight;
        Accele = 1.0f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Re_Flag = PG.GetHitMoveFlag();
        //フラグを判別して処理をする
        if(Re_Flag == true)
        {
            Fall_Flag = -1;
        }
        if (Fall_Flag != -1 && Fall_Flag != -2)
        {
            //落下処理をする
            Fall();
        }
        else if (Fall_Flag == -1)
        {
            //落下後の復活判定
            Re();
        }
        //オブジェクトの座標を変更する
        transform.position = new Vector2(Ice.x, IcePos);
    }

    //コリジョンの判定する
    void OnTriggerEnter2D(Collider2D collider)
    {
        //つららを初期よりも少し大きめに設定する
        if (Fall_Flag == 1 || Fall_Flag == -1)
        {
            Reset();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           Reset();
        }
    }
}
