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

    // Update is called once per frame
    void Update()
    {

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
        return Vector3.Normalize(player.position - Position);
    }

    //生成時間総フレーム数取得
    public int GetReloadFlame()
    {
        return ReloadFlame;
    }

}
