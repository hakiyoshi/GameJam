using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //PlayerGimmick�X�N���v�g
    PlayerGimmick PG;

    //��A���E�̑ϐ��I�u�W�F�N�g(��)
    GameObject[] Cols;

    //�e�ϐ��̃X�v���C�g
    public Sprite NeedleSprite;
    public Sprite LavaSprite;
    public Sprite IceSprite;

    //Rigidbody2D
    Rigidbody2D rb;


    CapsuleCollider2D cc;

    //Gimmick�擾�p���C���[
    LayerMask Gimmick_Layer;

    //���S���̕\��ω��p
    Animator anim;

    //�ړ��\������p��bool
    bool bMove;

    bool bFall;

    bool bRight;
    bool bLeft;

    //�ړ��������ʗp
    [Header("�ړ����x")]
    [SerializeField] float MaxSpeed;//�ō����x
    float direction;

    [Header("�_�b�V�����ړ����x")]
    public float dashPower = 1.5f;

    [Header("�W�����v�̍���")]
    [SerializeField] float JumpPower;//�W�����v�̍���

    [Header("���� ����0.99")]
    [SerializeField] float Inertia;//����
    private float UseInertia;//�g������

    //�v���C���[�̊p�x�p
    float rotateZ;
    float rotateY;

    //�v���C���[�̉�]�p�x�v�Z�p
    float now_Rotate;

    //2�i�W�����v�J�E���g�p
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        PG = this.GetComponent<PlayerGimmick>();

        //�M�~�b�N�ϐ��I�u�W�F�N�g�p�z��
        Cols = new GameObject[3];
        //�E�̃M�~�b�N�ϐ��I�u�W�F�N�g�p
        Cols[0] = transform.Find("Right").gameObject;
        //��̃M�~�b�N�ϐ��I�u�W�F�N�g�p
        Cols[1] = transform.Find("Top").gameObject;
        //���̃M�~�b�N�ϐ��I�u�W�F�N�g�p
        Cols[2] = transform.Find("Left").gameObject;

        //�e�I�u�W�F�N�g�𓧖���(��)
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
            Cols[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        //Ray�Ŕ��肷�郌�C���[�ݒ�
        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        //�������Z�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody2D>();

        cc = GetComponent<CapsuleCollider2D>();

        //�A�j���[�^�[�擾
        anim = this.GetComponent<Animator>();

        //�ړ��\����bool������
        SetMove(true);

        bFall = false;

        bRight = false;

        bLeft = false;

        //�ړ������p���l������
        direction = 0f;

        //�p�x������
        rotateZ = 0f;
        rotateY = 0f;

        //��]�p�x������
        now_Rotate = 0f;

        //�W�����v�J�E���g������
        jumpCount = 2;

        //�����Z�b�g
        UseInertia = Inertia;
    }

    void Update()
    {
        if (!PG.GetHitMoveFlag())
        {
            //�����Ԃ������̕\��ύX
            anim.SetBool("isDeth", false);

            if (Time.timeScale != 0)
            {
                Jump();
                Collision();
                cc.enabled = true;
            }
        }
        else
        {
            //���񂾂Ƃ��̕\��ύX
            anim.SetBool("isDeth", true);
            cc.enabled = false;
            bRight = false;
            bLeft = false;
        }


    }

    void FixedUpdate()
    {
        if (!PG.GetHitMoveFlag())
        {
            //�ړ�
            Move();
        }

        //��]�������f 
        this.rb.transform.eulerAngles = new Vector3(0, rotateY, rotateZ);
            
    }

    //�L�[�{�[�h���͓��̏���
    void Move()
    {
        //�_�b�V������p
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MaxSpeed = 0.192f;
        }
        else
        {
            MaxSpeed = 0.15f;
        }

        //�L�[���͂ł̈ړ�����
        if (Input.GetKey(KeyCode.D))
        {
            direction = MaxSpeed;
            bRight = true; 
            bLeft = false;

            if (jumpCount == 2)
            {
                //�v���C���[�̍��E���]
                rotateY = 0;

                //�ϐ����ǂɖ�����Ȃ��悤�ɏC��
                for (int i = 0; i < Cols.Length; i++)
                {
                    Cols[i].transform.localPosition = new Vector3(Cols[i].transform.localPosition.x,
                                                                  Cols[i].transform.localPosition.y,
                                                                  -1f);
                }
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -MaxSpeed;
            bRight = false;
            bLeft = true;
            if (jumpCount == 2)
            {
                //�v���C���[�̍��E���]
                rotateY = 180;

                //�ϐ����ǂɖ�����Ȃ��悤�ɏC��
                for (int i = 0; i < Cols.Length; i++)
                {
                    Cols[i].transform.localPosition = new Vector3(Cols[i].transform.localPosition.x,
                                                                  Cols[i].transform.localPosition.y,
                                                                  3f);
                }
            }
        }
        else
        {
            bRight = false;
            bLeft = false;
        }

        //���E�ړ�
        rb.position += new Vector2(direction, 0.0f);

        //�^������
        direction *= UseInertia;
    }

    void Jump()
    {
        //�X�y�[�X�L�[���������Ƃ��̃W�����v����
        if (((Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return)) || (!Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.Return))) && jumpCount != 0)
        {
            AudioManager.PlayAudio("Jamp",false,false);

            //�������Z���Z�b�g
            rb.velocity = Vector2.zero;

            //�W�����v�J�E���g-1
            jumpCount -= 1;

            //�W�����v����
            rb.AddForce(Vector2.up * JumpPower);

            if (0 < this.transform.localScale.x)
            {
                //��]�����J�n
                StartCoroutine("Rotation", -90);
            }
            else
            {
                //��]�����J�n
                StartCoroutine("Rotation", 90);
            }
        }
    }

    //�W�����v���̉�]�̏���(�R���[�`��)
    IEnumerator Rotation(float angle)
    {
        //��]���x�C���^�[�o��������
        float time = 0.0f;

        //��]�p�x�v�Z
        now_Rotate += angle;

        //360�x�𒴂�����0�ɖ߂�
        if (now_Rotate < -359 || 359 < now_Rotate)
            now_Rotate = 0f;

        //��]�������s
        while (time <= 0.2f)
        {
            //���̊p�x����90�x��]
            rotateZ = Mathf.Lerp(now_Rotate - angle, now_Rotate, time / 0.1f);

            //�C���^�[�o�����Z
            time += Time.deltaTime;

            yield return 0;
        }
    }

    //Ray�����蔻��擾�p(�J����)&(�R���[�`��)
    void Collision()
    {
        //�����蔻��pRay
        RaycastHit2D[] hits = new RaycastHit2D[20];

        //�����x�N�g��
        Vector3[] Dire_Vec = { rb.transform.right, //�E
                               rb.transform.up,    //��
                              -rb.transform.right, //��
                              -rb.transform.up};   //�� 

        //ray�̎n�_
        Vector3 sta_Position = new Vector3(this.rb.transform.position.x
                                         , this.rb.transform.position.y);
        //ray�̏I�_�z��
        Vector3[] end_Position = new Vector3[20];

        float end_distance = 1.1f;

        //ray�̊e�I�_�ݒ�(�E)
        end_Position[0] = sta_Position + Dire_Vec[0] * end_distance;
        end_Position[1] = end_Position[0] + Dire_Vec[1] / 2.2f;
        end_Position[2] = sta_Position + Dire_Vec[0] + Dire_Vec[1] * 0.8f;
        end_Position[3] = end_Position[0] + Dire_Vec[3] / 2.2f;
        end_Position[4] = sta_Position + Dire_Vec[0] + Dire_Vec[3] * 0.8f;

        //ray�̊e�I�_�ݒ�(��)
        end_Position[5] = sta_Position + Dire_Vec[1] * end_distance * 1.2f;
        end_Position[6] = end_Position[5] + Dire_Vec[0] / 2.2f;
        end_Position[7] = sta_Position + Dire_Vec[1] + Dire_Vec[0] * 0.8f;
        end_Position[8] = end_Position[5] + Dire_Vec[2] / 2.2f;
        end_Position[9] = sta_Position + Dire_Vec[1] + Dire_Vec[2] * 0.8f;

        //ray�̊e�I�_�ݒ�(��)
        end_Position[10] = sta_Position + Dire_Vec[2] * end_distance;
        end_Position[11] = end_Position[10] + Dire_Vec[1] / 2.2f;
        end_Position[12] = sta_Position + Dire_Vec[2] + Dire_Vec[1] * 0.8f;
        end_Position[13] = end_Position[10] + Dire_Vec[3] / 2.2f;
        end_Position[14] = sta_Position + Dire_Vec[2] + Dire_Vec[3] * 0.8f;

        //ray�̊e�I�_�ݒ�(��)
        end_Position[15] = sta_Position + Dire_Vec[3] * end_distance;
        end_Position[16] = end_Position[15] + Dire_Vec[0] / 2.2f;
        end_Position[17] = sta_Position + Dire_Vec[3] + Dire_Vec[0] * 0.8f;
        end_Position[18] = end_Position[15] + Dire_Vec[2] / 2.2f;
        end_Position[19] = sta_Position + Dire_Vec[3] + Dire_Vec[2] * 0.8f;

        //ray�̊e�ݒ�(�㉺���E)
        for (int i = 0; i < hits.Length; i++)
            hits[i] = Physics2D.Linecast(sta_Position, end_Position[i], Gimmick_Layer);

        //�ύX�X�v���C�g�i�[�p
        Sprite change_Sprite = null;

        //�����蔻��m�F�p���[�v
        for (int i = 0; i < hits.Length; i++)
        {
            //i�Ԗڂ�Ray������������
            if (hits[i])
            {
                //�f�o�b�O��Line������p
                Debug.DrawLine(sta_Position, end_Position[i], Color.red);

                //�����ȊO�ɓ���������
                if (0 <= i && i <= 14)
                {
                    //�������������ɐF(�ϐ�)��\��
                    Cols[i / 5].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

                    //�j�M�~�b�N�ɓ���������
                    if (hits[i].collider.gameObject.tag == "Needle")
                    {
                        //�ύX�X�v���C�g��j�ɐݒ�
                        change_Sprite = NeedleSprite;
                    }
                    //�}�O�}�M�~�b�N�ɓ���������
                    else if (hits[i].collider.gameObject.tag == "Lava")
                    {
                        //�ύX�X�v���C�g���}�O�}�ɐݒ�
                        change_Sprite = LavaSprite;
                    }
                    //�X�M�~�b�N�ɓ���������
                    else if (hits[i].collider.gameObject.tag == "Ice" || hits[i].collider.gameObject.tag == "IceGround")
                    {
                        //�ύX�X�v���C�g��X�ɐݒ�
                        change_Sprite = IceSprite;
                    }
                    else if(hits[i].collider.gameObject.tag == "Fall")
                    {
                        Deth();
                        PG.HitGimmick(hits[i].collider);
                    }

                    //���������ʂ��M�~�b�N�ϐ��������Ă��Ȃ�������
                    if (Cols[i / 5].GetComponent<SpriteRenderer>().sprite != change_Sprite)
                    {
                        //�M�~�b�N�ɑΉ������ϐ���t�^
                        Cols[i / 5].GetComponent<SpriteRenderer>().sprite = change_Sprite;
                        Cols[i / 5].GetComponent<BoxCollider2D>().enabled = true;

                        //�M�~�b�N�Ƀq�b�g�������Ƃ�ʒm���ă`�F�b�N�|�C���g�ɖ߂�
                        PG.HitGimmick(hits[i].collider);

                        //�e�p�x���Z�b�g
                        now_Rotate = rotateZ = 0f;

                        break;
                    }
                        jumpCount = 1;
                }
                //�����ɓ���������
                else
                {
                    Deth();
                    PG.HitGimmick(hits[i].collider);
                }
            }
            else
            {
                    Debug.DrawLine(sta_Position, end_Position[i], Color.yellow);
            }
        }
    }

    void Deth()
    {
        //�S�Ɖu�폜����
        for (int j = 0; j < Cols.Length; j++)
        {
            //�S�Ă̖Ɖu�𓧖���
            Cols[j].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            Cols[j].GetComponent<BoxCollider2D>().enabled = false;
            //�M�~�b�N�ɑΉ������ϐ���t�^
            Cols[j].GetComponent<SpriteRenderer>().sprite = null;
        }
        //�e�p�x���Z�b�g
        now_Rotate = rotateZ = 0f;

        Ending_Manager.AddDead_Count();
    }

    //�������f�pbool�擾�p
    public bool GetMove()
    {
        return bMove;
    }
    //�������f�pbool�ݒ�p
    public void SetMove(bool set)
    {
        bMove = set;
    }

    public bool GetbRight()
    {
        return bRight;
    }

    public bool GetbLeft()
    {
        return bLeft;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //�X�̒n�ʂƂ̓����蔻��
        if (collision.gameObject.CompareTag("IceGround"))
        {
            //�����𑽂߂ɕt����
            UseInertia = 0.995f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //�n�ʂƂ̓����蔻��
        if (collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("IceGround"))
        {
           UseInertia = 0.0f;//����������
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //���Ƃ����Ƃ̓����蔻��
        if (collision.gameObject.CompareTag("Fall"))
        {
            bFall = true;
        }
        //�n�ʂƂ̓����蔻��
        if (collision.gameObject.CompareTag("Ground"))
        {
            //�W�����v�J�E���g���Z�b�g
            jumpCount = 2;
            UseInertia = Inertia;//������t����
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        //���Ƃ����Ƃ̓����蔻�胊�Z�b�g
        if (collision.gameObject.CompareTag("Fall"))
        {
            bFall = false;
        }

    }

}
