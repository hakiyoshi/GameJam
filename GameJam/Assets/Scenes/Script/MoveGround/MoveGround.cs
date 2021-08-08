using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [Header("�J�n�ʒu�̍��W")]
    [SerializeField] Transform StartPosition;

    [Header("���B�n�_�̍��W")]
    [SerializeField] Transform EndPosition;

    [Header("�Г��ړ��t���[����")]
    [SerializeField] int MoveFlame;
    int FlameCount = 0;

    bool MoveFlag = true;

    Vector2 Vec = Vector2.zero;

    Rigidbody2D player;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform t = this.transform;

        Vec = t.position;//���O�ɍ��W����
        t.position = Move(StartPosition.position, EndPosition.position, (float)FlameCount / (float)MoveFlame);//�ړ���̍��W���擾
        Vec = new Vector2(t.position.x, t.position.y) - Vec;//�ړ��x�N�g�����쐬

        if (MoveFlag)
        {
            FlameCount++;
        }
        else
        {
            FlameCount--;
        }

        if (FlameCount > MoveFlame || FlameCount < 0)
        {
            MoveFlag = !MoveFlag;
        }

        if (player != null)
        {
            player.position += Vec;
        }
    }


    //�ړ��֐�
    //Time��0.0f~1.0f
    //Start�n�_
    //End�I�_�@
    private Vector2 Move(Vector2 Start, Vector2 End, float Time)
    {
        return Start + ((End - Start) * Time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }
}
