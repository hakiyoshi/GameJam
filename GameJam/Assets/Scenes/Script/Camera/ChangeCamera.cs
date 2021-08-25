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

    private OriginalDollyCart cart = null;
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
            cart = dollycart.Follow.GetComponent<OriginalDollyCart>();//�����ړ��J�[�g�擾
            speed = cart.m_Speed;//�X�s�[�h����
            cart.m_Speed = 0.0f;//�X�s�[�h��0�ɐݒ�

            player = GameObject.Find("Player").transform;//�v���C���[�̍��W�擾
            ChangeMain();

            StartCoroutine(CheckDollyCart());//�����ړ������邩���䂷��
        }
            
    }

    //�����ړ��J�n���邩�`���b�N
    private IEnumerator CheckDollyCart()
    {
        while (true)
        {
            if (IfCameraFlag(CAMERAFLAG.MAIN) && dollycart.Follow.position.x - this.transform.position.x < 0)//�X�^�[�g�n�_�ɋ߂Â����狭���ړ��J�n
            {
                moveflag = CAMERAFLAG.TARGET;
                ChangeTargetGroup();

                StartCoroutine("CheckEndDollyCart");//�`�F�b�N
                yield break;
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    //�����ړ����I�����邩�`�F�b�N
    private IEnumerator CheckEndDollyCart()
    {
        while (true)
        {
            
            //�ŏI�n�_�ɂ��ǂ蒅���ăv���C���[�̈ʒu���J�������E�ɍs�����烁�C���J�����Ɉړ�
            if (IfCameraFlag(CAMERAFLAG.DOLLY) && cart.m_Position >= cart.m_Path.PathLength && this.transform.position.x - player.position.x < 0)
            {
                ChangeMain();
                cart.m_Speed = 0.0f;
                yield break;//�R���[�`�������
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //���C���J�����ɕύX
    void ChangeMain()
    {
        if (dollycart != null && dollycart.Follow != null)
        {
            cart.m_Speed = 0.0f;
            dollycart.Priority = 0;
            targetcamera.Priority = 0;
        }
        maincamera.Priority = 10;
        moveflag = CAMERAFLAG.MAIN;
    }

    //�����ړ��J�����ɕύX
    void ChangeDollyCart()
    {
        if (dollycart != null && dollycart.Follow != null)
        {
            cart.m_Speed = speed;
            maincamera.Priority = 0;
            dollycart.Priority = 10;
            targetcamera.Priority = 0;
            moveflag = CAMERAFLAG.DOLLY;
        }
    }

    void ChangeTargetGroup()
    {
        if (dollycart != null && dollycart.Follow != null)
        {
            cart.m_Speed = 0.0f;
            maincamera.Priority = 0;
            dollycart.Priority = 0;
            targetcamera.Priority = 10;
            moveflag = CAMERAFLAG.TARGET;
        }
    }

    //��r�J�����t���O
    public bool IfCameraFlag(CAMERAFLAG flag)
    {
        return moveflag == flag ? true : false;
    }

    public void StartDollyCart()
    {
        ChangeDollyCart();
    }

    public void StartMain()
    {
        ChangeMain();
    }

    public void ResetDollyCart()
    {
        if (dollycart != null && dollycart.Follow != null)
        {
            cart.m_Position = 0.0f;
        }
    }

    public void StopDollyCart()
    {
        if (dollycart != null && dollycart.Follow != null)
        {
            cart.m_Speed = 0.0f;
        }
    }
}
