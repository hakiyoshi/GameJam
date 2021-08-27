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

    private BoxCollider2D box;

    private ChangeCamera change;//�����ړ�����

    private bool restartdolly = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();//�d��
        last = this.GetComponent<PlayerLastField>();//�v���C���[���Ō�܂ŗ����Ă����n��
        box = this.GetComponent<BoxCollider2D>();//�������蔻��
        change = Camera.main.GetComponent<ChangeCamera>();//�J�����`�F���W�X�N���v�g
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

                if(restartdolly)
                    change.StartDollyCart();

                restartdolly = false;
            }
        }
    }

    //�M�~�b�N�ɓ����������ɌĂ΂��֐�
    public void HitGimmick()
    {
        GimmickSet(last.LastPosition);
    }

    //�M�~�b�N�ɓ����������ɌĂ΂��֐�
    public void HitGimmick(Collider2D collider)
    {
        SetHitGimmick(collider.gameObject.GetComponent<RespawnPoint>());
    }

    //�M�~�b�N�ɓ����������ɌĂ΂��֐�
    public void HitGimmick(Collision2D collision)
    {
        SetHitGimmick(collision.gameObject.GetComponent<RespawnPoint>());
    }

    private void SetHitGimmick(RespawnPoint rp)
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
