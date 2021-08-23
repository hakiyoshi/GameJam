using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
    [Header("MainVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera maincamera = null;//バーチャルカメラ

    [Header("DollyCartVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera dollycart = null;//強制移動カメラ

    [Header("TargetGroupVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera targetcamera = null;//ターゲットカメラ

    private OriginalDollyCart cart = null;
    private CinemachinePathBase path = null;
    private float speed;

    public enum CAMERAFLAG
    {
        MAIN,//メインカメラ
        DOLLY,//強制移動
        TARGET,//ターゲット
    }
    private CAMERAFLAG moveflag = CAMERAFLAG.MAIN;//移動フラグ

    private Transform player = null;

    // Start is called before the first frame update
    void Start()
    {
        maincamera.Priority = 10;

        //強制移動が設定されてない場合
        if (dollycart != null && dollycart.Follow != null)
        {
            cart = dollycart.Follow.GetComponent<OriginalDollyCart>();//強制移動カート取得
            path = cart.m_Path;//パス取得
            speed = cart.m_Speed;//スピードを避難
            cart.m_Speed = 0.0f;//スピードを0に設定

            player = GameObject.Find("Player").transform;//プレイヤーの座標取得
            ChangeMain();

            StartCoroutine(CheckDollyCart());//強制移動をするか制御する
        }
            
    }

    //強制移動開始するかチャック
    private IEnumerator CheckDollyCart()
    {
        while (true)
        {
            if (IfCameraFlag(CAMERAFLAG.MAIN) && dollycart.Follow.position.x - this.transform.position.x < 0)//スタート地点に近づいたら強制移動開始
            {
                moveflag = CAMERAFLAG.TARGET;
                ChangeTargetGroup();

                StartCoroutine("CheckEndDollyCart");//チェック
                yield break;
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    //強制移動が終了するかチェック
    private IEnumerator CheckEndDollyCart()
    {
        while (true)
        {
            //最終地点にたどり着いてプレイヤーの位置がカメラより右に行ったらメインカメラに移動
            if (IfCameraFlag(CAMERAFLAG.DOLLY) && cart.m_Position >= path.PathLength - 1.0f)
            {
                ChangeMain();
                cart.m_Speed = 0.0f;
                yield break;//コルーチンを閉じる
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //メインカメラに変更
    void ChangeMain()
    {
        cart.m_Speed = 0.0f;
        maincamera.Priority = 10;
        dollycart.Priority = 0;
        targetcamera.Priority = 0;
        moveflag = CAMERAFLAG.MAIN;
    }

    //強制移動カメラに変更
    void ChangeDollyCart()
    {
        cart.m_Speed = speed;
        maincamera.Priority = 0;
        dollycart.Priority = 10;
        targetcamera.Priority = 0;
        moveflag = CAMERAFLAG.DOLLY;
    }

    void ChangeTargetGroup()
    {
        cart.m_Speed = 0.0f;
        maincamera.Priority = 0;
        dollycart.Priority = 0;
        targetcamera.Priority = 10;
        moveflag = CAMERAFLAG.TARGET;
    }

    //比較カメラフラグ
    public bool IfCameraFlag(CAMERAFLAG flag)
    {
        return moveflag == flag ? true : false;
    }

    public void StartDollyCart()
    {
        ChangeDollyCart();
    }

    public void StartMain()
    {
        ChangeMain();
    }

    public void ResetDollyCart()
    {
        cart.m_Position = 0.0f;
    }

    public void StopDollyCart()
    {
        cart.m_Speed = 0.0f;
    }
}
