using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
    [Header("DollyTrack")]
    [SerializeField] CinemachineSmoothPath path = null;

    [Header("MainVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera maincamera = null;//バーチャルカメラ

    [Header("DollyCartVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera dollycart = null;//強制移動カメラ

    private CinemachineDollyCart cart;
    private float speed;

    private bool moveflag = true;//移動フラグ

    // Start is called before the first frame update
    void Start()
    {
        maincamera.Priority = 10;

        //強制移動が設定されてない場合
        if (dollycart != null && dollycart.Follow != null)
        {
            cart = dollycart.Follow.GetComponent<CinemachineDollyCart>();
            speed = cart.m_Speed;//スピードを避難
            cart.m_Speed = 0.0f;//スピードを0に設定
            ChangeMain();

            StartCoroutine(CheckDollyCart());//強制移動をするか制御する
        }
            
    }

    private IEnumerator CheckDollyCart()
    {
        while (true)
        {
            if (moveflag && path.m_Waypoints[0].position.x - this.transform.position.x < 0)//スタート地点に近づいたら強制移動開始
            {
                moveflag = false;
                ChangeDollyCart();
            }
            else if (!moveflag && Vector3.Distance(path.m_Waypoints[path.m_Waypoints.Length - 1].position, this.transform.position) <= 10.1f)//最終地点にたどり着いたらプレイヤー移動に変わる
            {
                ChangeMain();
                cart.m_Speed = 0.0f;
                yield break;//コルーチンを閉じる
            }
            
            yield return new WaitForSeconds(1.0f / 30.0f);
        }
    }

    //メインカメラに変更
    void ChangeMain()
    {
        maincamera.Priority = 10;
        dollycart.Priority = 0;
        cart.m_Speed = 0.0f;
    }


    //強制移動カメラに変更
    void ChangeDollyCart()
    {
        maincamera.Priority = 0;
        dollycart.Priority = 10;
        cart.m_Speed = speed;
    }
}
