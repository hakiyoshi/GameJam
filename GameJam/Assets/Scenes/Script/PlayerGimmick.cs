using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimmick : MonoBehaviour
{

    //�ړ�
    private Vector2 EndPosi;//���B�n�_
    private Vector2 StartPosi;//�J�n�n�_

    [SerializeField] int MoveFlame;//�ړ��t���[��
    private int FlameCount = 0;


    private bool HitFlag = false;

    //�K�v�Ȃ���
    private Rigidbody2D rb;//����
    private PlayerLastField last;//�Ō�̒��n�_

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

    //�M�~�b�N�ɓ����������ɌĂ΂��֐�
    public void HitGimmick()
    {
        GimmickSet(last.LastPosition);
    }

    //�M�~�b�N�ɓ����������ɌĂ΂��֐�
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

    //�M�~�b�N�ɓ����������ɌĂ΂��֐�
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
    
    //�q�b�g�����ێw�肵���ꏊ�Ɉړ��������m�F����֐�
    //true �ړ���
    //false �ړ����ĂȂ�
    public bool GetHitMoveFlag()
    {
        return HitFlag;
    }

    //�ړ��֐�
    //Time��0.0f~1.0f
    //Start�n�_
    //End�I�_�@
    private Vector2 Move(Vector2 Start, Vector2 End, float Time)
    {
        return Start + ((End - Start) * Time);
    }
}
