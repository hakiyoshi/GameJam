using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
    [Header("MainVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera maincamera = null;//�o�[�`�����J����

    [Header("DollyCartVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera dollycart = null;//�����ړ��J����

    [Header("TargetGroupVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera targetcamera = null;//�^�[�Q�b�g�J����

    private CinemachineDollyCart cart = null;
    private CinemachinePathBase path = null;
    private float speed;

    public enum CAMERAFLAG
    {
        MAIN,//���C���J����
        DOLLY,//�����ړ�
        TARGET,//�^�[�Q�b�g
    }
    private CAMERAFLAG moveflag = CAMERAFLAG.MAIN;//�ړ��t���O

    private Transform player = null;

    // Start is called before the first frame update
    void Start()
    {
        maincamera.Priority = 10;

        //�����ړ����ݒ肳��ĂȂ��ꍇ
        if (dollycart != null && dollycart.Follow != null)
        {
            cart = dollycart.Follow.GetComponent<CinemachineDollyCart>();//�����ړ��J�[�g�擾
            path = cart.m_Path;//�p�X�擾
            speed = cart.m_Speed;//�X�s�[�h����
            cart.m_Speed = 0.0f;//�X�s�[�h��0�ɐݒ�

            player = GameObject.Find("Player").transform;//�v���C���[�̍��W�擾
            ChangeMain();

            StartCoroutine(CheckDollyCart());//�����ړ������邩���䂷��
        }
            
    }

    private IEnumerator CheckDollyCart()
    {
        while (true)
        {
            if (IfCameraFlag(CAMERAFLAG.MAIN) && dollycart.Follow.position.x - this.transform.position.x < 0)//�X�^�[�g�n�_�ɋ߂Â����狭���ړ��J�n
            {
                moveflag = CAMERAFLAG.TARGET;
                ChangeTargetGroup();
            }
            else if (IfCameraFlag(CAMERAFLAG.DOLLY) && cart.m_Position >= path.PathLength && cart.gameObject.transform.position.x - player.position.x < 0)//�ŏI�n�_�ɂ��ǂ蒅���ăv���C���[�̈ʒu���J�������E��
            {
                ChangeMain();
                cart.m_Speed = 0.0f;
                yield break;//�R���[�`�������
            }
            
            yield return new WaitForSeconds(1.0f / 30.0f);
        }
    }

    //���C���J�����ɕύX
    void ChangeMain()
    {
        maincamera.Priority = 10;
        dollycart.Priority = 0;
        targetcamera.Priority = 0;
        cart.m_Speed = 0.0f;
    }


    //�����ړ��J�����ɕύX
    void ChangeDollyCart()
    {
        maincamera.Priority = 0;
        dollycart.Priority = 10;
        targetcamera.Priority = 0;
        cart.m_Speed = speed;
    }

    void ChangeTargetGroup()
    {
        maincamera.Priority = 0;
        dollycart.Priority = 0;
        targetcamera.Priority = 10;
        cart.m_Speed = 0.0f;
    }

    //��r�J�����t���O
    public bool IfCameraFlag(CAMERAFLAG flag)
    {
        return moveflag == flag ? true : false;
    }

    //�����ړ��J�n
    public void StartDollyCart()
    {
        moveflag = CAMERAFLAG.DOLLY;
        ChangeDollyCart();
    }
}
