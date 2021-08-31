using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoController : MonoBehaviour
{
    //�g���[���[�̓������I�u�W�F�N�g���擾����
    public GameObject Video;

    //�Đ��������肷��p��bool
    bool bPlay;

    // Start is called before the first frame update
    void Start()
    {
        //������
        bPlay = false;
        
        //���������ɔ�\���ɂ���
        Video.SetActive(bPlay);
    }

    // Update is called once per frame
    void Update()
    {
        //Ctl + V�L�[�̓��������ōĐ��J�n
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V) && !bPlay)
        {
            //�S�Ẳ�������
            AudioManager.AllStopAudio();
            bPlay = true;
        }
        //�ǂ����̃L�[�������Βʏ��ʂɖ߂�
        else if (Input.anyKeyDown && !Input.GetKey(KeyCode.Space) && bPlay )
        {
            //�^�C�g���̉����o��
            AudioManager.PlayAudio("Title", false, true);
            bPlay = false;
        }

        //bPlay�̒l�ɂ���čĐ�����
        Video.SetActive(bPlay);
    }
}
