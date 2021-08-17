using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusButtery : MonoBehaviour
{
    //弾マネージャー
    private VirusBulletManager bulletmana;

    //スプライトレンダラー
    private SpriteRenderer sr;

    //弾が込められているか
    private bool beginbullet = true;

    //フレームカウント
    private int FlameCount = 0;

    //角度の範囲誤差
    private const float ERRORRANGE = 1.0f;

    //弾の初期位置距離
    private const float STARTRANGE = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        //弾マネージャー
        bulletmana = this.transform.parent.GetComponent<VirusBulletManager>();

        //スプライトレンダラー
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //弾が発射していない状態でプレイヤーの位置と自分の向きのなす角が範囲内の時
        if (beginbullet && Mathf.Abs(CalPlayerWithThisAngle()) <= ERRORRANGE)//範囲内の時
        {
            ShotBullet();
        }
    }

    private void FixedUpdate()
    {
        //弾が込められてない場合
        if (!beginbullet)
        {
            //到達地点計算
            Vector3 end = (this.transform.localRotation * new Vector3(1.0f, 0.0f, 0.0f)) * STARTRANGE;
            this.transform.localPosition = Move(Vector3.zero, end, (float)FlameCount / (float)bulletmana.GetReloadFlame());

            FlameCount++;

            if (FlameCount == bulletmana.GetReloadFlame())
            {
                ReloadBullet(end);
            }
        }
    }

    //弾発射
    void ShotBullet()
    {
        bulletmana.CreateBullet(this.gameObject);
        beginbullet = false;
        this.transform.localPosition = Vector3.zero;
    }

    //リロード
    void ReloadBullet(Vector3 end)
    {
        beginbullet = true;
        FlameCount = 0;
        this.transform.localPosition = end;
    }

    //プレイヤーと自分のベクトルのなす角を求める
    float CalPlayerWithThisAngle()
    {
        return Vector3.Angle(
            GetForward(this.transform.rotation),
            bulletmana.CalPlayerVec(this.transform.position));
    }

    //正面ベクトル
    public Vector3 GetForward(Quaternion Rotation)
    {
        return Rotation * new Vector3(1.0f, 0.0f, 0.0f);
    }

    //移動
    Vector3 Move(Vector3 start, Vector3 end, float time)
    {
        return start + ((end - start) * time);
    }
}
