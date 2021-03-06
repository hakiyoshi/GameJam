using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimmick : MonoBehaviour
{

    //移動
    private Vector2 EndPosi;//到達地点
    private Vector2 StartPosi;//開始地点

    [SerializeField] int MoveFlame;//移動フレーム
    private int FlameCount = 0;


    private bool HitFlag = false;

    //必要なもの
    private Rigidbody2D rb;//物理
    private PlayerLastField last;//最後の着地点

    private BoxCollider2D box;

    private ChangeCamera change;//強制移動制御

    private bool restartdolly = false;

    //ディレイ
    int Delay = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();//重力
        last = this.GetComponent<PlayerLastField>();//プレイヤーが最後まで立っていた地面
        box = this.GetComponent<BoxCollider2D>();//箱当たり判定
        change = Camera.main.GetComponent<ChangeCamera>();//カメラチェンジスクリプト
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (HitFlag)
        {
#if UNITY_EDITOR
            Debug.DrawLine(EndPosi + new Vector2(-1.0f, 0.0f), EndPosi + new Vector2(1.0f, 0.0f), Color.red);
            Debug.DrawLine(EndPosi + new Vector2(0.0f, -1.0f), EndPosi + new Vector2(0.0f, 1.0f), Color.red);
#endif
            if (Delay > 0)//ディレイが0より大きい場合
            {
                Delay--;
                return;
            }

            
            rb.position = Move(StartPosi, EndPosi, (float)FlameCount / (float)MoveFlame);
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);

            FlameCount++;

            if (FlameCount == MoveFlame)
            {
                HitFlag = false;
                FlameCount = 0;
                rb.position = EndPosi;

                if(restartdolly)
                    change.StartDollyCart();

                restartdolly = false;
            }
        }
    }

    //ギミックに当たった時に呼ばれる関数
    public void HitGimmick()
    {
        GimmickSet(last.LastPosition);
    }

    //ギミックに当たった時に呼ばれる関数
    public void HitGimmick(Collider2D collider, int delay = 0)
    {
        SetHitGimmick(collider.gameObject.GetComponent<RespawnPoint>(), delay);
    }

    //ギミックに当たった時に呼ばれる関数
    public void HitGimmick(Collision2D collision, int delay = 0)
    {
        SetHitGimmick(collision.gameObject.GetComponent<RespawnPoint>(), delay);
    }

    private void SetHitGimmick(RespawnPoint rp, int delay)
    {
        if (rp != null)
        {
            Transform vec = rp.GetRespawnPoint();
            if (vec == null)
            {
                GimmickSet(last.LastPosition);
            }
            else
            {
                GimmickSet(vec.position);
            }
        }
        else
        {
            GimmickSet(last.LastPosition);
        }

        Delay = delay;
    }

    private void GimmickSet(Vector2 LastPosi)
    {
        EndPosi = LastPosi;
        StartPosi = rb.position;
        last.LastPosiFlag = false;
        HitFlag = true;

        if (change.IfCameraFlag(ChangeCamera.CAMERAFLAG.DOLLY))
        {
            change.StopDollyCart();
            change.ResetDollyCart();
            restartdolly = true;
        }
    }
    
    //ヒットした際指定した場所に移動中かを確認する関数
    //true 移動中
    //false 移動してない
    public bool GetHitMoveFlag()
    {
        return HitFlag;
    }

    //移動関数
    //Timeは0.0f~1.0f
    //Start始点
    //End終点　
    private Vector2 Move(Vector2 Start, Vector2 End, float Time)
    {
        return Start + ((End - Start) * Time);
    }
}
