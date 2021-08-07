using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [Header("スタートのゲームオブジェクト")]
    public GameObject start;
    [Header("ゴールのゲームオブジェクト")]
    public GameObject end;
    [Header("移動量の調整0.0～1.0fの間に値")]
    public float MoveSpeed;
    //x座標の移動量
    float Movex;
    //y座標の移動量
    float Movey;
    //x座標のポジション
    float Posx;
    //y座標のポジション
    float Posy;
    //移動しきったか判定する変数0.0f~1.0f
    float Norm;
    //初期化判定
    bool Init_Flag;
    //スタートのオブジェクトの座標を取得する
    Vector3 PosS;
    //エンドのオブジェクトの座標を取得する
    Vector3 PosE;
    
    // Start is called before the first frame update
    void Start()
    {
        //ゲームオブジェクトのコンポーネントを取得する
        GameObject start = GetComponent<GameObject>();
        GameObject end = GetComponent<GameObject>();
        //初期化フラグを倒す
        Init_Flag = false;
        //移動判別変数の初期化
        Norm = 0.5f;
    }

    void TurnMove()
    {
        //移動量を反転する
        Movex = -Movex;
        Movey = -Movey;
        //デバッグ用
        Debug.Log("向きが変わる");
        Norm = 0.0f;
    }

    void Init()
    {
        //取得したオブジェクトの座標を取得
        PosS = start.transform.position;
        PosE = end.transform.position;
        //移動量を算出する
        Movex = (PosE.x - PosS.x) * MoveSpeed;
        Movey = (PosE.y - PosS.y) * MoveSpeed;
        //初期化フラグを立てる
        Init_Flag = true;
        //初期位置を設定する
        Posx = PosS.x + (PosE.x - PosS.x) / 2;
        Posy = PosS.y + (PosE.y - PosS.y) / 2;
        //スタートとゴールの間の座標
        transform.position = new Vector2(Posx, Posy);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( Init_Flag == false)
        {
            Init();
        }
        //座標のポジションを加算する
        Posx += Movex;
        Posy += Movey;
        Norm += MoveSpeed;
        //取得したオブジェクトの範囲を越えた場合の条件文
        if(Norm > 1.0f)
        {
            TurnMove();
        }
        //座標を変更する
        transform.position = new Vector2(Posx, Posy);
    }
}
