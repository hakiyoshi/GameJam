using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    [Header("�L�����N�^�[�̃I�u�W�F�N�g��I������")]
    public GameObject Charcter;
    //�L�����N�^�[�̃|�W�V�������擾
    Vector3 CharPos;
    //���̍��W���擾
    Vector3 Ice;
    //�M�~�b�N���̂̍��W
    float IcePos;
    [Header("��炪��������͈�")]
    public float Range;
    [Header("��炪�����鑬�x")]
    public float YSpeed;
    [Header("���̕������鍂��(���X�̍��� + �������鍂��)")]
    public float YHight;
    [Header("��炪�������鑬�x")]
    public float ReSpeed;
    //��炪�����ł����Ԃ��t���O�𗧂Ă�
    int Fall_Flag;

    // Start is called before the first frame update
    void Start()
    {
        //�L�����N�^�[�̃I�u�W�F�N�g�̃R���|�[�l���g���擾����
        GameObject Charcter = GetComponent<GameObject>();
        //���̍��W���擾
        Ice = this.transform.position;
        IcePos = Ice.y;
        //�����t���O�𗧂Ă�
        Fall_Flag = 0;
    }
    //�����̔��������
    void Fall()
    {
        //�L�����N�^�[�̍��W���擾����
        CharPos = Charcter.transform.position;
        //x���W�����̐�Βl���Z�o����
        float Dis = Mathf.Abs(Ice.x - CharPos.x);
        //�L�����N�^�[�Ƃ��͈̔͂��m�F����
        if (Dis < Range || Fall_Flag == 1)
        {
            //���𗎂Ƃ��t���O��1�ɂ���
            Fall_Flag = 1;
            //��������
            IcePos -= YSpeed;
        }
    }
    //��炪�ォ�琶���Ă��鏈��
    void Re()
    {
        //��炪�����Ă��鑬�x
        IcePos -= ReSpeed;
        //��炪�����Ă��鏉���̍��W�����Ⴂ�����f����
        if (IcePos < Ice.y)
        {
            //����y���W�̕ύX
            IcePos = Ice.y;
            //���̃t���O��0�ɂ���
            Fall_Flag = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�t���O�𔻕ʂ��ď���������
        if (Fall_Flag != -1)
        {
            //��������������
            Fall();
        }
        else if (Fall_Flag == -1)
        {
            //������̕�������
            Re();
        }
        //�I�u�W�F�N�g�̍��W��ύX����
        transform.position = new Vector2(Ice.x, IcePos);
    }
    //�R���W�����̔��肷��
    void OnTriggerEnter2D(Collider2D collider)
    {
        //�����������������傫�߂ɐݒ肷��
        if (Fall_Flag == 1)
        {
            Fall_Flag = -1;
            IcePos = Ice.y + YHight;
        }
    }
}