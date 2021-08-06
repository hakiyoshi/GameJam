using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [Header("スタートのゲームオブジェクト")]
    public GameObject start;
    [Header("ゴールのゲームオブジェクト")]
    public GameObject end;
    [Header("移動量の調整")]
    public float MoveSpeed;
    //x座標の移動量
    float Movex;
    //y座標の移動量
    float Movey;
    //x座標のポジション
    float Posx;
    //y座標のポジション
    float Posy;
    //スタートのオブジェクトの座標を取得する
    Vector3 PosS;
    //エンドのオブジェクトの座標を取得する
    Vector3 PosE;
    //初期化判定

    // Start is called before the first frame update
    void Start()
    {
        //ゲームオブジェクトのコンポーネントを取得する
        GameObject start = GetComponent<GameObject>();
        GameObject end = GetComponent<GameObject>();
        //取得したオブジェクトの座標を取得
        PosS = start.transform.position;
        PosE = end.transform.position;
        //移動量を算出する
        Movex = (PosE.x - PosS.x) / MoveSpeed;
        Movey = (PosE.y - PosS.y) / MoveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //座標のポジションを加算する
        Posx += Movex;
        Posy += Movey;
        //取得したオブジェクトの範囲を越えた場合の条件文
        if(PosS.x - Posx >= 0 && PosS.y - Posy >= 0)
        {
            //移動量を反転する
            Movex = -Movex;
            Movey = -Movey;
        }
        else if(PosE.x - Posx >= 0 && PosE.y - Posy >= 0)
        {
            //移動量を反転する
            Movex = -Movex;
            Movey = -Movey;
        }
        //座標を変更する
        transform.position = new Vector2(Posx, Posy);
    }
}
