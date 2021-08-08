using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGimmick : MonoBehaviour
{

    [Header("���𗎂Ƃ��Ԋu(�t���[��)")]
    [SerializeField] int DropIntervalFlame;//���Ƃ��Ԋu

    [Header("�������x")]
    [SerializeField] float Speed;//�������x

    [Header("���Ƃ��I�u�W�F�N�g")]
    [SerializeField] GameObject DropObject;//���I�u�W�F�N�g

    private int FlameCount = 0;

    private Transform rp;//���X�|�[���|�C���g

    // Start is called before the first frame update
    void Start()
    {
        rp = this.GetComponent<RespawnPoint>().GetRespawnPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (FlameCount == DropIntervalFlame)//�w��̃t���[���ŏ���
        {
            GameObject CreateObj = GameObject.Instantiate(DropObject);
            Drop drop = CreateObj.GetComponent<Drop>();
            drop.SetSpeed(Speed);
            drop.GetComponent<RespawnPoint>().SetRespawnPoint(rp);
            CreateObj.transform.position = this.transform.position;
            FlameCount = 0;
        }

        FlameCount++;
    }
}
