using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNeedle : MonoBehaviour
{
    [Header("���������Ă���p�x")]
    public float AngleZ;
    [Header("�ړ�����")]
    public float Dis;
    [Header("�ړ�����X�s�[�h0.0f ~ 1.0f")]
    public float Speed;
    //�J�E���g1�܂ł�
    float Count;
    //�ړ���
    Vector3 MoveSpeed;
    //�N�H�[�^�j�I��
    Quaternion Quo;

    // Start is called before the first frame update
    void Start()
    {
        //�N�H�[�^�j�I���̊p�x��ݒ肷��
        Quo = Quaternion.Euler(0.0f, 0.0f, AngleZ);
        //�I�u�W�F�N�g�̊p�x��ύX����
        this.transform.rotation = Quo;
        //�J�E���g��������
        Count = 0.0f;
        //�X�s�[�h�̕ύX
        MoveSpeed = gameObject.transform.rotation * new Vector3(0.0f, Dis * Speed, 0.0f);
    }
  
    // Update is called once per frame
    void FixedUpdate()
    {
        //�J�E���g���X�s�[�h������������
        Count += Speed;
        //���݂̍��W����ړ��ʂ𑫂�
        this.transform.position += MoveSpeed;
        //�������A�J�E���g��1�ɂȂ�����
        if(Count >= 1.0f)
        {
            //�J�E���g��������
            Count = 0.0f;
            //�ړ��ʂ𔽓]������
            MoveSpeed = -MoveSpeed;
        }
    }
}
