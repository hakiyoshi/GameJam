using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBullet : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] float MoveSpeed = 0.8f;

    //�G�{�̍��W
    private Transform MasterVirus;

    //�����ʒu
    private float fallposi;
    private Vector3 posi = Vector3.zero;//���W
    private bool fallflag = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //�ړ�
        if(!fallflag)//�O�i
        {
            this.transform.position += CalMoveVec();
        }
        else//����
        {
            this.transform.position = CalFallVec();
        }


        if (MasterVirus.position.y + 20.0f <= this.transform.position.y)//���̍����ɒB������
        {
            this.transform.rotation = Quaternion.AngleAxis(-90.0f, new Vector3(0.0f, 0.0f, 1.0f));//�e�̐i�s���������ɂ���
            this.transform.position = CalFallPosition();
            fallflag = true;
        }
        else if (this.transform.position.y <= -40.0f)
        {
            Destroy(this.gameObject);
        }
    }

    //�ړ�
    Vector3 CalMoveVec()
    {
        Vector3 vec = this.transform.rotation * new Vector3(1.0f, 0.0f, 0.0f);
        return vec * MoveSpeed;
    }

    //����
    Vector3 CalFallVec()
    {
        return new Vector3(MasterVirus.position.x + fallposi + 12.0f, this.transform.position.y, 0.0f) + CalMoveVec();
    }

    //�������W�����l
    Vector3 CalFallPosition()
    {
        return new Vector3(MasterVirus.position.x + fallposi + 12.0f, MasterVirus.position.y + 20.0f, 0.0f);
    }

    //virus �G�{�̍��W
    //fallposi �G���猩�ė������鋗��
    public void CreateSet(Transform virus, float fallposi)
    {
        MasterVirus = virus;
        this.fallposi = fallposi;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(CalFallPosition(), CalFallPosition() + new Vector3(0.0f, -100.0f, 0.0f));
    }

#endif

}
