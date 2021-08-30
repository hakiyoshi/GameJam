using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusMove : MonoBehaviour
{
    [Header("�J�������炸�炷����")]
    [SerializeField] Vector3 ShiftSize = Vector3.zero;

    //�J�����̍��W
    private Transform cameraposi = null;

    //�A�N�e�B�u�t���O
    private VirusActiveManager active;

    //�o��
    [Header("�o�����x")]
    [SerializeField] float activespeed = 1.0f;

    private Animator anime;

    private void OnEnable()
    {
        active = this.transform.parent.GetComponent<VirusActiveManager>();
        cameraposi = Camera.main.transform;//�J�����̍��W���擾

        //�ړ�
        this.transform.position = CalPosition();

        anime = this.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (active.IfActive(VirusActiveManager.ACTIVE.FORCED))//�����ړ����Ă��邩
        {
            this.transform.position = CalPosition();
        }
        else if (active.IfActive(VirusActiveManager.ACTIVE.ACTIVE))//�o��
        {
            PlayActive();
        }
        else if (active.IfActive(VirusActiveManager.ACTIVE.END) && anime.GetCurrentAnimatorStateInfo(0).IsName("EndIdle"))
        {
            this.gameObject.SetActive(false);
        }
    }

    //�o��
    private void PlayActive()
    {
        this.transform.position = new Vector3(CalPosition().x, this.transform.position.y - activespeed, 0.0f);

        if (CalPosition().y >= this.transform.position.y)//�J�����̈ʒu��ʂ�߂�����
        {
            active.SetActiveFlag(VirusActiveManager.ACTIVE.FORCED);
        }
    }

    //���W�v�Z
    private Vector3 CalPosition()
    {
        return ChangeCameraPosition() + ShiftSize;
    }

    //�J�������W���g���₷���悤�ɕϊ�
    private Vector3 ChangeCameraPosition()
    {
        return new Vector3(cameraposi.position.x, cameraposi.position.y, 0.0f);
    }

    public void StartActive()
    {
        this.transform.position = CalPosition() + new Vector3(0.0f, 60.0f, 0.0f);
    }

    public void EndActiveStart()
    {
        anime.SetTrigger("End");
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        
        //cameraposi = Camera.main.transform;
        //this.transform.position = CalPosition();
    }

#endif
}
