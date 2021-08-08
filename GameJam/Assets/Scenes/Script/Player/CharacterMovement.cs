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

    //Gimmick�擾�p���C���[
    LayerMask Gimmick_Layer;

    //���S���̕\��ω��p
    Animator anim;

    //��������p��bool
    bool bDeth;

    //�j�M�~�b�N�pbool
    bool bNeedle;
    //�}�O�}�M�~�b�N�pbool
    bool bLava;

    //�ړ��������ʗp
    [Header("�ړ����x")]
    [SerializeField] float MaxSpeed;//�ō����x
    float direction;

    [Header("�W�����v�̍���")]
    [SerializeField] float JumpPower;//�W�����v�̍���

    [Header("���� ����0.99")]
    [SerializeField] float Inertia;//����

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
        }

        //Ray�Ŕ��肷�郌�C���[�ݒ�
        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        //�������Z�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody2D>();

        //�A�j���[�^�[�擾
        anim = this.GetComponent<Animator>();

        //��������p��bool������
        SetDeth(false);

        //�e�M�~�b�N����pbool������
        bNeedle = false;
        bLava = false;

        //�ړ������p���l������
        direction = 0f;

        //�p�x������
        rotateZ = 0f;
        rotateY = 0f;

        //��]�p�x������
        now_Rotate = 0f;

        //�W�����v�J�E���g������
        jumpCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PG.GetHitMoveFlag())
        {
            anim.SetBool("isDeth", false);
            Move();
            StartCoroutine("Collision");
        }
        else
        {
            anim.SetBool("isDeth", true);
        }

        //��]�������f 
        this.rb.transform.eulerAngles = new Vector3(0, rotateY, rotateZ);
    }

    //�L�[�{�[�h���͓��̏���
    void Move()
    {
        //�X�y�[�X�L�[���������Ƃ��̃W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount != 0)
        {
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

        //�L�[���͂ł̈ړ�����
        if (Input.GetKey(KeyCode.D))
        {
            direction = MaxSpeed;
            
            if(jumpCount == 2)
                rotateY = 0;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -MaxSpeed;

            if (jumpCount == 2)
                rotateY = 180;
        }


        //���E�ړ�
        rb.position += new Vector2(direction, 0.0f);

        //�^������
        direction *= Inertia;
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
    IEnumerator Collision()
    {
        //�����蔻��pRay
        RaycastHit2D[] hits = new RaycastHit2D[12];
        //ray�̒���
        float end_distance = 1.4f;
        //�����x�N�g��
        Vector3[] Dire_Vec = { rb.transform.right * end_distance,       //�E
                                    rb.transform.up * end_distance,     //��
                                    -rb.transform.right * end_distance, //��
                                    -rb.transform.up * end_distance};   //�� 
        //ray�̎n�_
        Vector3 sta_Position = new Vector3(this.rb.transform.position.x + this.GetComponent<BoxCollider2D>().offset.x
                                         , this.rb.transform.position.y + this.GetComponent<BoxCollider2D>().offset.y);
        //ray�̏I�_�z��
        Vector3[] end_Position = new Vector3[12];

        //ray�̊e�I�_�ݒ�(�㉺���E)
        end_Position[0] = sta_Position + Dire_Vec[0];
        end_Position[1] = end_Position[0] + Dire_Vec[1] / 2f;
        end_Position[2] = end_Position[0] + Dire_Vec[3] / 2f;
        end_Position[3] = sta_Position + Dire_Vec[1];
        end_Position[4] = end_Position[3] + Dire_Vec[0] / 2f;
        end_Position[5] = end_Position[3] + Dire_Vec[2] / 2f;
        end_Position[6] = sta_Position + Dire_Vec[2];
        end_Position[7] = end_Position[6] + Dire_Vec[1] / 2f;
        end_Position[8] = end_Position[6] + Dire_Vec[3] / 2f;
        end_Position[9] = sta_Position + Dire_Vec[3];
        end_Position[10]= end_Position[9] + Dire_Vec[0] / 2f;
        end_Position[11]= end_Position[9] + Dire_Vec[2] / 2f;


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
                if (0 <= i && i <= 8)
                {
                    //�������������ɐF(�ϐ�)��\��
                    Cols[i / 3].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

                    //�j�M�~�b�N�ɓ���������
                    if (hits[i].collider.gameObject.tag == "Needle" && bNeedle)
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
                    else if (hits[i].collider.gameObject.tag == "Ice")
                    {
                        //�ύX�X�v���C�g��X�ɐݒ�
                        change_Sprite = IceSprite;
                    }

                    //���������ʂ��M�~�b�N�ϐ��������Ă��Ȃ�������
                    if (Cols[i / 3].GetComponent<SpriteRenderer>().sprite != change_Sprite)
                    {
                        //�M�~�b�N�ɑΉ������ϐ���t�^
                        Cols[i / 3].GetComponent<SpriteRenderer>().sprite = change_Sprite;

                        //�M�~�b�N�Ƀq�b�g�������Ƃ�ʒm���ă`�F�b�N�|�C���g�ɖ߂�
                        PG.HitGimmick(hits[i].collider);

                        yield return new WaitForSeconds(0.2f);

                        //�e�p�x���Z�b�g
                        now_Rotate = rotateZ = 0f;
                    }
                        break;
                }
                //�����ɓ���������
                else
                {
                    //�S�Ɖu�폜����
                    for (int j = 0; j < Cols.Length; j++)
                    {
                        //�S�Ă̖Ɖu�𓧖���
                        Cols[j].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
                        //�M�~�b�N�ɑΉ������ϐ���t�^
                        Cols[j].GetComponent<SpriteRenderer>().sprite = null;
                    }

                    PG.HitGimmick(hits[i].collider);
                    yield return new WaitForSeconds(0.2f);
                    //�e�p�x���Z�b�g
                    now_Rotate = rotateZ = 0f;
                }

            }
            else
            {
                //�f�o�b�O��Line������p
                Debug.DrawLine(sta_Position, end_Position[i], Color.blue);
            }
        }
    }

    //�������f�pbool�擾�p
    public bool GetDeth()
    {
        return bDeth;
    }
    //�������f�pbool�ݒ�p
    public void SetDeth(bool set)
    {
        bDeth = set;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //�n�ʂƂ̓����蔻��
        if (collision.gameObject.CompareTag("Ground"))
        {
            //�W�����v�J�E���g���Z�b�g
            jumpCount = 2;
        }
    }
        //�����蔻��擾�p(��)
        /*void OnCollisionEnter2D(Collision2D collision)
        {
            //�n�ʂƂ̓����蔻��
            if (collision.gameObject.CompareTag("Ground"))
            {
                //�W�����v�J�E���g���Z�b�g
                jumpCount = 2;
            }

            //�}�O�}�Ƃ̓����蔻��
            if (collision.gameObject.CompareTag("Lava"))
            {
                bLava = true;
            }
            //�X�Ƃ̓����蔻��
            else if (collision.gameObject.CompareTag("Ice"))
            {
                bIce = true;
            }
        }
        */

        void OnTriggerEnter2D(Collider2D collision)
        {
            //�j�Ƃ̓����蔻��
            if (collision.gameObject.CompareTag("Needle"))
            {
                bNeedle = true;
            }

            //�}�O�}�Ƃ̓����蔻��
            if (collision.gameObject.CompareTag("Lava"))
            {
                bLava = true;
            }
    }
}
