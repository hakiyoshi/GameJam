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

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        last = this.GetComponent<PlayerLastField>();
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


            rb.position = Move(StartPosi, EndPosi, (float)FlameCount / (float)MoveFlame);
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);

            FlameCount++;

            if (FlameCount == MoveFlame)
            {
                HitFlag = false;
                FlameCount = 0;
                rb.position = EndPosi;
            }
        }
    }

    //ギミックに当たった時に呼ばれる関数
    public void HitGimmick()
    {
        GimmickSet(last.LastPosition);
    }

    //ギミックに当たった時に呼ばれる関数
    public void HitGimmick(Collider2D collision)
    {
        RespawnPoint rp = collision.gameObject.GetComponent<RespawnPoint>();
        if (rp != null)
        {
            GimmickSet(rp.GetRespawnPoint());
        }
        else
        {
            GimmickSet(last.LastPosition);
        }
    }

    //ギミックに当たった時に呼ばれる関数
    public void HitGimmick(Collision2D collision)
    {
        RespawnPoint rp = collision.gameObject.GetComponent<RespawnPoint>();
        if (rp != null)
        {
            GimmickSet(rp.GetRespawnPoint());
        }
        else
        {
            GimmickSet(last.LastPosition);
        }
    }

    private void GimmickSet(Vector2 LastPosi)
    {
        EndPosi = LastPosi;
        StartPosi = rb.position;
        last.LastPosiFlag = false;
        HitFlag = true;
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
