using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBulletManager : MonoBehaviour
{
    [Header("弾プレハブ")]
    [SerializeField] GameObject BulletPrefab;

    [Header("砲台リロードフレーム数")]
    [SerializeField] int ReloadFlame = 20;

    //弾情報
    private GameObject[] bullet;

    //プレイヤー座標
    private Transform player;

    //弾発射クールタイム
    public const int COOLTIME = 1;//一回撃ったらCOOLTIME回数分撃たない
    private int CountCoolTime = 0;//クールタイムカウント用

    // Start is called before the first frame update
    void Start()
    {
        //弾を取得
        bullet = new GameObject[this.transform.childCount];
        for (int i = 0; i < bullet.Length; i++)
        {
            bullet[i] = this.transform.GetChild(i).gameObject;
        }

        //プレイヤー座標受け取る
        player = GameObject.Find("Player").transform;
    }

    public GameObject CreateBullet(GameObject obje)
    {
        GameObject newobj = Instantiate(BulletPrefab);
        newobj.transform.position = obje.transform.position;
        newobj.transform.rotation = obje.transform.rotation;
        return newobj;
    }

    //プレイヤーへの方向ベクトル　正規化済
    public Vector3 CalPlayerVec(Vector3 Position)
    {
        return Vector3.Normalize(Quaternion.AngleAxis(30.0f, new Vector3(0.0f, 0.0f, 1.0f)) * new Vector3(1.0f, 0.0f, 0.0f));
    }

    //生成時間総フレーム数取得
    public int GetReloadFlame()
    {
        return ReloadFlame;
    }

    //指定したタイムを加算
    public void AddCoolTime(int addtime)
    {
        CountCoolTime += addtime;
    }

    //ゲッター
    public int GetCoolTime()
    {
        return CountCoolTime;
    }

    //セッター
    public void SetCoolTime(int settime)
    {
        CountCoolTime = settime;
    }

    //指定したCoolTime以上になっていたらtrue
    public bool GetIfCoolTime(int CoolTime = COOLTIME)
    {
        return CountCoolTime >= CoolTime ? true : false;
    }
}
