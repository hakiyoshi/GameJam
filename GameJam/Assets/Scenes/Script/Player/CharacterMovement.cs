using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //PlayerGimmick�X�N���v�g
    PlayerGimmick PG;

    //��A���E�̑ϐ��I�u�W�F�N�g(��)
    GameObject[] Cols;

    GameObject PreGimmick;

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

    //�ړ��������ʗp
    [Header("�ړ����x")]
    [SerializeField] float MaxSpeed;//�ō����x
    float init_Maxspeed;
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

    string DamageSound;

    bool bJump;

    RaycastHit2D[] hits;

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

        PreGimmick = null;

        //Ray�Ŕ��肷�郌�C���[�ݒ�
        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        //�������Z�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody2D>();

        cc = GetComponent<CapsuleCollider2D>();

        //�A�j���[�^�[�擾
        anim = this.GetComponent<Animator>();

        //�ړ������p���l������
        direction = 0f;

        init_Maxspeed = MaxSpeed;

        //�p�x������
        rotateZ = 0f;
        rotateY = 0f;

        //��]�p�x������
        now_Rotate = 0f;

        //�W�����v�J�E���g������
        jumpCount = 2;

        //�����Z�b�g
        UseInertia = Inertia;

        DamageSound = null;

        bJump = false;
    }

    void Update()
    {
        if (!PG.GetHitMoveFlag())
        {
            //�����Ԃ������̕\��ύX
            anim.SetBool("isDeth", false);

            if (Time.timeScale != 0)
            {
                Collision();
                Jump();
                cc.enabled = true;

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit)
                    {
                        bJump = false;
                        jumpCount = 1;
                        PreGimmick = hit.collider.gameObject;
                    }
                    else if(!bJump && PreGimmick != null && PreGimmick.layer == 6)
                    {
                        jumpCount = 2;
                    }
                }
            }
        }
        else
        {
            //���񂾂Ƃ��̕\��ύX
            anim.SetBool("isDeth", true);
            cc.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (!PG.GetHitMoveFlag())
        {
            if (Time.timeScale != 0)
            {
                //�ړ�
                Move();
            }
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
            MaxSpeed = init_Maxspeed * dashPower;
        }
        else
        {
            MaxSpeed = init_Maxspeed;
        }

        //�L�[���͂ł̈ړ�����
        if (Input.GetKey(KeyCode.D))
        {
            direction = MaxSpeed;

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

        //���E�ړ�
        rb.position += new Vector2(direction, 0.0f);

        //�^������
        direction *= UseInertia;
    }

    void Jump()
    {
        //�X�y�[�X�L�[���������Ƃ��̃W�����v����
        if (((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) && 0 < jumpCount)
        {
            bJump = true;
            //�W�����v�J�E���g-1
            jumpCount--;

            AudioManager.PlayAudio("Jamp", false, false);

            //�������Z���Z�b�g
            rb.velocity = Vector2.zero;

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
        hits = new RaycastHit2D[20];

        //�����x�N�g��
        Vector3[] Dire_Vec = { rb.transform.right, //�E
                               rb.transform.up,    //��
                              -rb.transform.right, //��
                              -rb.transform.up};   //�� 

        //ray�̎n�_
        Vector3 sta_Position = new Vector3(this.rb.transform.position.x + cc.offset.x
                                         , this.rb.transform.position.y + cc.offset.y);
        //ray�̏I�_�z��
        Vector3[] end_Position = new Vector3[20];

        float end_distance = 1.35f;

        //ray�̊e�I�_�ݒ�(�E)
        end_Position[0] = sta_Position + Dire_Vec[0] * end_distance;
        end_Position[1] = end_Position[0] + Dire_Vec[1] * 0.5f;
        end_Position[2] = end_Position[0] + Dire_Vec[1] * end_distance * 0.7f;
        end_Position[3] = end_Position[0] + Dire_Vec[3] * 0.5f;
        end_Position[4] = end_Position[0] + Dire_Vec[3] * end_distance * 0.7f;

        //ray�̊e�I�_�ݒ�(��)
        end_Position[5] = sta_Position + Dire_Vec[1] * end_distance * 1.25f;
        end_Position[6] = end_Position[5] + Dire_Vec[0] * 0.5f;
        end_Position[7] = end_Position[5] + Dire_Vec[0] * 1.2f;
        end_Position[8] = end_Position[5] + Dire_Vec[2] * 0.5f;
        end_Position[9] = end_Position[5] + Dire_Vec[2] * 1.2f;

        //ray�̊e�I�_�ݒ�(��)
        end_Position[10] = sta_Position + Dire_Vec[2] * end_distance;
        end_Position[11] = end_Position[10] + Dire_Vec[1] * 0.5f;
        end_Position[12] = end_Position[10] + Dire_Vec[1] * end_distance * 0.7f;
        end_Position[13] = end_Position[10] + Dire_Vec[3] * 0.5f;
        end_Position[14] = end_Position[10] + Dire_Vec[3] * end_distance * 0.7f;

        //ray�̊e�I�_�ݒ�(��)
        end_Position[15] = sta_Position + Dire_Vec[3] * end_distance * 1.25f;
        end_Position[16] = end_Position[15] + Dire_Vec[0] * 0.6f;
        end_Position[17] = sta_Position + Dire_Vec[3] + Dire_Vec[0];
        end_Position[18] = end_Position[15] + Dire_Vec[2] * 0.6f;
        end_Position[19] = sta_Position + Dire_Vec[3] + Dire_Vec[2];

        //ray�̊e�ݒ�(�㉺���E)
        for (int i = 0; i < hits.Length; i++)
            hits[i] = Physics2D.Linecast(sta_Position, end_Position[i], Gimmick_Layer);

        //�ύX�X�v���C�g�i�[�p
        Sprite change_Sprite = null;

        //�����蔻��m�F�p���[�v
        for (int i = hits.Length - 1; 0 <= i; i--)
        {
            //i�Ԗڂ�Ray������������
            if (hits[i])
            {
                //�f�o�b�O��Line������p
                Debug.DrawLine(sta_Position, end_Position[i], Color.red);

                if (hits[i].collider.gameObject.name == "DropLava(Clone)")
                {
                    Destroy(hits[i].collider.gameObject);
                }

                //�j�M�~�b�N�ɓ���������
                if (hits[i].collider.gameObject.tag == "Needle")
                {
                    //�ύX�X�v���C�g��j�ɐݒ�
                    change_Sprite = NeedleSprite;
                    DamageSound = "Damage_Needle";
                }
                //�}�O�}�M�~�b�N�ɓ���������
                else if (hits[i].collider.gameObject.tag == "Lava")
                {
                    Debug.Log(hits[i].collider.gameObject.name);

                    //�ύX�X�v���C�g���}�O�}�ɐݒ�
                    change_Sprite = LavaSprite;
                    DamageSound = "Damage_Lava";
                }
                //�X�M�~�b�N�ɓ���������
                else if (hits[i].collider.gameObject.tag == "Ice" || hits[i].collider.gameObject.tag == "IceGround")
                {
                    //�ύX�X�v���C�g��X�ɐݒ�
                    change_Sprite = IceSprite;
                    DamageSound = "Damage_Ice";
                }
                else if (hits[i].collider.gameObject.tag == "Fall")
                {
                    DamageSound = null;
                    Deth();
                    PG.HitGimmick(hits[i].collider);
                }
                //�����ȊO�ɓ���������
                if (0 <= i && i <= 14)
                {
                    //�������������ɐF(�ϐ�)��\��
                    Cols[i / 5].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

                    //���������ʂ��M�~�b�N�ϐ��������Ă��Ȃ�������
                    if (Cols[i / 5].GetComponent<SpriteRenderer>().sprite != change_Sprite)
                    {
                        //�M�~�b�N�ɑΉ������ϐ���t�^
                        Cols[i / 5].GetComponent<SpriteRenderer>().sprite = change_Sprite;
                        Cols[i / 5].GetComponent<BoxCollider2D>().enabled = true;

                        if (DamageSound != null)
                            AudioManager.PlayAudio(DamageSound, false, false);

                        //�M�~�b�N�Ƀq�b�g�������Ƃ�ʒm���ă`�F�b�N�|�C���g�ɖ߂�
                        PG.HitGimmick(hits[i].collider);

                        //�e�p�x���Z�b�g
                        now_Rotate = rotateZ = 0f;

                        Ending_Manager.AddDead_Count();

                        break;
                    }
                }
                //�����ɓ���������
                else
                {
                    Deth();
                    PG.HitGimmick(hits[i].collider);

                    if (DamageSound != null)
                        AudioManager.PlayAudio(DamageSound, false, false);

                    Ending_Manager.AddDead_Count();
                    break;
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
        PreGimmick = null;

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

    void OnTriggerStay2D(Collider2D collision)
    {
        //�n�ʂƂ̓����蔻��
        if (collision.gameObject.CompareTag("Ground"))
        {
            bJump = false;
            PreGimmick = null;
            //�W�����v�J�E���g���Z�b�g
            jumpCount = 2;
            UseInertia = Inertia;//������t����
        }
    }
}
