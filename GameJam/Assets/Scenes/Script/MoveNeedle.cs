using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNeedle : MonoBehaviour
{
    [Header("棘が向いている角度")]
    public float AngleZ;
    [Header("移動距離")]
    public float Dis;
    [Header("移動するスピード0.0f ~ 1.0f")]
    public float Speed;
    //カウント1までに
    float Count;
    //移動量
    Vector3 MoveSpeed;
    //クォータニオン
    Quaternion Quo;

    // Start is called before the first frame update
    void Start()
    {
        //クォータニオンの角度を設定する
        Quo = Quaternion.Euler(0.0f, 0.0f, AngleZ);
        //オブジェクトの角度を変更する
        this.transform.rotation = Quo;
        //カウントを初期化
        Count = 0.0f;
        //スピードの変更
        MoveSpeed = gameObject.transform.rotation * new Vector3(0.0f, Dis * Speed, 0.0f);
    }
  
    // Update is called once per frame
    void FixedUpdate()
    {
        //カウントをスピード分増加させる
        Count += Speed;
        //現在の座標から移動量を足す
        this.transform.position += MoveSpeed;
        //もしも、カウントが1になったら
        if(Count >= 1.0f)
        {
            //カウントを初期化
            Count = 0.0f;
            //移動量を反転させる
            MoveSpeed = -MoveSpeed;
        }
    }
}
