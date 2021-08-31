using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeDollyTrack : MonoBehaviour
{
    //�g���b�N
    [SerializeField] CinemachineSmoothPath[] track;

    //�`�F�b�N�|�C���g
    [SerializeField] Transform[] RespawnPoint;

    //�J�[�g
    private CinemachineDollyCart cart;

    //���݂̃g���b�N
    private int nowtrack = 0;

    //�G�̋����ړ�
    [SerializeField] VirusActiveManager virus;

    private void Start()
    {
        //�J�[�g�擾
        cart = this.GetComponent<CinemachineDollyCart>();

        //�p�X����ԍŏ��ɐݒ�
        ChangePath(0);

        //�R���[�`���X�^�[�g
        StartCoroutine("CheckCartPosition");
    }

    IEnumerator CheckCartPosition()
    {
        while (true)
        {
            //�p�X���Ō㓖����ɋ߂Â����玟�̃p�X�Ɉڍs
            if (cart.m_Position >= track[nowtrack].PathLength)
            {
                //���̃g���b�N�ɕύX
                nowtrack++;

                //�g���b�N���Z�b�g
                ChangePath(nowtrack);

                //���̃p�X���Ȃ��ꍇ�͏I��
                if (track.Length <= nowtrack + 1)
                {
                    nowtrack = -1;
                    yield break;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //�p�X�ύX
    void ChangePath(int id)
    {
        cart.m_Path = track[id];
        cart.m_Position = 0.0f;
        if (RespawnPoint[id] != null)
        {
            virus.SetRespawnPoint(RespawnPoint[id]);
        }
    }

    //�����ړ��Đ�����
    public bool GetDollyCartPlay()
    {
        return nowtrack != -1 ? true : false;
    }
}
