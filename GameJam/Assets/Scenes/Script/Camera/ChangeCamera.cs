using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
    [Header("DollyTrack")]
    [SerializeField] CinemachineSmoothPath path = null;

    [Header("MainVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera maincamera = null;//�o�[�`�����J����

    [Header("DollyCartVirtualCamera")]
    [SerializeField] CinemachineVirtualCamera dollycart = null;//�����ړ��J����

    private CinemachineDollyCart cart;
    private float speed;

    private bool moveflag = true;//�ړ��t���O

    // Start is called before the first frame update
    void Start()
    {
        maincamera.Priority = 10;

        //�����ړ����ݒ肳��ĂȂ��ꍇ
        if (dollycart != null && dollycart.Follow != null)
        {
            cart = dollycart.Follow.GetComponent<CinemachineDollyCart>();
            speed = cart.m_Speed;//�X�s�[�h����
            cart.m_Speed = 0.0f;//�X�s�[�h��0�ɐݒ�
            ChangeMain();

            StartCoroutine(CheckDollyCart());//�����ړ������邩���䂷��
        }
            
    }

    private IEnumerator CheckDollyCart()
    {
        while (true)
        {
            if (moveflag && path.m_Waypoints[0].position.x - this.transform.position.x < 0)//�X�^�[�g�n�_�ɋ߂Â����狭���ړ��J�n
            {
                moveflag = false;
                ChangeDollyCart();
            }
            else if (!moveflag && Vector3.Distance(path.m_Waypoints[path.m_Waypoints.Length - 1].position, this.transform.position) <= 10.1f)//�ŏI�n�_�ɂ��ǂ蒅������v���C���[�ړ��ɕς��
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
        cart.m_Speed = 0.0f;
    }


    //�����ړ��J�����ɕύX
    void ChangeDollyCart()
    {
        maincamera.Priority = 0;
        dollycart.Priority = 10;
        cart.m_Speed = speed;
    }
}
