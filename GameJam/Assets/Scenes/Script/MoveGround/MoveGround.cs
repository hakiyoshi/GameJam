using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [Header("開始位置の座標")]
    [SerializeField] Transform StartPosition;

    [Header("到達地点の座標")]
    [SerializeField] Transform EndPosition;

    [Header("片道移動フレーム数")]
    [SerializeField] int MoveFlame;
    int FlameCount = 0;

    bool MoveFlag = true;

    Vector2 Vec = Vector3.zero;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = StartPosition.position;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position = Move(StartPosition.position, EndPosition.position, (float)FlameCount / (float)MoveFlame);

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
    }


    //移動関数
    //Timeは0.0f~1.0f
    //Start始点
    //End終点　
    private Vector2 Move(Vector2 Start, Vector2 End, float Time)
    {
        Vec = (End - Start) * Time;

        return Start + Vec;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.transform.parent = collision.transform;
        }
    }
}
