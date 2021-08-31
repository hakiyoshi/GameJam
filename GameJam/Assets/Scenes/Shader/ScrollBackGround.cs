using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{
    //カメラの取得用
    public GameObject Cam;

    //CinemaSceneカメラの取得用
    GameObject CinemaCam;

    //チェンジカメラクラス取得用
    ChangeCamera changeCamera;

    [Header("スクロール速度")]
    [SerializeField]float ScrollSpeed;

    [Header("スクロールさせる画像の数")]
    [SerializeField]int BackGroundCount;

    //背景の配列
    Renderer[] BackGround;

    //スクロールスピードに掛け合わせる値
    float Time;

    //背景の透明度を変える時に使う値
    float Alpha;

    [Header("ボス戦があるかどうか")]
    [SerializeField]bool bChange = false;

    [Header("透明化スピード")]
    [SerializeField]float Minus_Alpha = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //CinemaSceneカメラ初期化
        CinemaCam = Cam.transform.GetChild(0).gameObject;

        //ChangeCameraクラスを取得
        changeCamera = GameObject.Find("Main Camera").GetComponent<ChangeCamera>();

        //背景の初期化
        BackGround = new Renderer[BackGroundCount];
        for(int i = 0; i< BackGroundCount; i++)
        {
            BackGround[i] = this.transform.GetChild(i).gameObject.GetComponent<Renderer>();
            BackGround[i].material.SetFloat("_XSpeed", ScrollSpeed);
        }

        //スクロールスピードに掛け合わせる値の初期化
        Time = 0f;

        //透明度の初期化
        Alpha = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //敵追跡時に取得するCinemaSceneカメラを変更する
        if (changeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.DOLLY))
            CinemaCam = Cam.transform.GetChild(1).gameObject;
        else
            CinemaCam = Cam.transform.GetChild(0).gameObject;

        //CinemaSceneカメラが動いてるか判定
        if (0.05f < Mathf.Abs(CinemaCam.transform.localPosition.x))
        {
            if (0 < CinemaCam.transform.localPosition.x)
                Time++;
            else if (CinemaCam.transform.localPosition.x < 0)
                Time--;
        }

        //背景のシェーダーの移動値を変更する
        for (int i = 0; i < BackGroundCount; i++)
            BackGround[i].material.SetFloat("_XSpeed", ScrollSpeed * Time);

        //敵退出時に透明度を元に戻す
        if (changeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN) && Alpha < 1)
            Alpha += 0.01f;
        //敵出現時に透明度を変更する
        else if (!changeCamera.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN) && 0 < Alpha)
            Alpha -= 0.1f;

        //敵が出現するステージであればシェーダーに透明度を適応する
        if (bChange)
            BackGround[1].material.SetFloat("_Alpha",Alpha);
    }
}
