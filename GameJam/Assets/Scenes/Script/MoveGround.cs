using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [Header("�X�^�[�g�̃Q�[���I�u�W�F�N�g")]
    public GameObject start;
    [Header("�S�[���̃Q�[���I�u�W�F�N�g")]
    public GameObject end;
    [Header("�v���C���[�I�u�W�F�N�g")]
    public GameObject pl;
    [Header("�ړ��ʂ̒���0.0�`1.0f�̊Ԃɒl")]
    public float MoveSpeed;
    //x���W�̈ړ���
    float Movex;
    //y���W�̈ړ���
    float Movey;
    //x���W�̃|�W�V����
    float Posx;
    //y���W�̃|�W�V����
    float Posy;
    //�ړ��������������肷��ϐ�0.0f~1.0f
    float Norm;
    //����������
    bool Init_Flag;
    //�X�^�[�g�̃I�u�W�F�N�g�̍��W���擾����
    Vector3 PosS;
    //�G���h�̃I�u�W�F�N�g�̍��W���擾����
    Vector3 PosE;
    //�v���C���[������Ă��邩����
    bool Player_Flag;
    //���b�W�g�{�f�B��錾����
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        //�Q�[���I�u�W�F�N�g�̃R���|�[�l���g���擾����
        GameObject start = GetComponent<GameObject>();
        GameObject end = GetComponent<GameObject>();
        rb = pl.GetComponent<Rigidbody2D>();
        //�������t���O��|��
        Init_Flag = false;
        Player_Flag = false;
        //�ړ����ʕϐ��̏�����
        Norm = 0.5f;
    }
    //�ړ��̌�����ύX����
    void TurnMove()
    {
        //�ړ��ʂ𔽓]����
        Movex = -Movex;
        Movey = -Movey;
        //�f�o�b�O�p
        Debug.Log("�������ς��");
        Norm = 0.0f;
    }
    //������
    void Init()
    {
        //�擾�����I�u�W�F�N�g�̍��W���擾
        PosS = start.transform.position;
        PosE = end.transform.position;
        //�ړ��ʂ��Z�o����
        Movex = (PosE.x - PosS.x) * MoveSpeed;
        Movey = (PosE.y - PosS.y) * MoveSpeed;
        //�������t���O�𗧂Ă�
        Init_Flag = true;
        //�����ʒu��ݒ肷��
        Posx = PosS.x + (PosE.x - PosS.x) / 2;
        Posy = PosS.y + (PosE.y - PosS.y) / 2;
        //�X�^�[�g�ƃS�[���̊Ԃ̍��W
        transform.position = new Vector2(Posx, Posy);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( Init_Flag == false)
        {
            Init();
        }
        //���W�̃|�W�V���������Z����
        Posx += Movex;
        Posy += Movey;
        Norm += MoveSpeed;
        //���W��ύX����
        transform.position = new Vector2(Posx, Posy);
        if(Player_Flag == true)
        {
            rb.transform.position = new Vector2(pl.transform.position.x + Movex,
                pl.transform.position.y + Movey);
        }
        //�擾�����I�u�W�F�N�g�͈̔͂��z�����ꍇ�̏�����
        if (Norm > 1.0f)
        {
            TurnMove();
        }
    }
    //�v���C���[�I�u�W�F�N�g���g���K�[�ɓ�������
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Player_Flag = true;
        }
    }
    //�v���C���[�I�u�W�F�N�g���g���K�[���甲������
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player_Flag = false;
        }
    }
}
