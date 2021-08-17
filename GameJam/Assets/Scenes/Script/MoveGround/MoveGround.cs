using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    Vector2 Vec = Vector2.zero;

    Rigidbody2D player;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(StartPosition.position, EndPosition.position);

        DrawGizmosBox(StartPosition.position, 0.1f);
        DrawGizmosBox(EndPosition.position, 0.1f);

        if(!EditorApplication.isPlaying)
            this.transform.position = StartPosition.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(StartPosition.position, EndPosition.position);

        DrawGizmosBox(StartPosition.position, 0.1f);
        DrawGizmosBox(EndPosition.position, 0.1f);
    }

    void DrawGizmosBox(Vector3 posi, float radius)
    {
        Gizmos.DrawLine(new Vector3(posi.x - radius, posi.y + radius, 0.0f), new Vector3(posi.x + radius, posi.y + radius, 0.0f));//上
        Gizmos.DrawLine(new Vector3(posi.x - radius, posi.y - radius, 0.0f), new Vector3(posi.x + radius, posi.y - radius, 0.0f));//下
        Gizmos.DrawLine(new Vector3(posi.x - radius, posi.y + radius, 0.0f), new Vector3(posi.x - radius, posi.y - radius, 0.0f));//左
        Gizmos.DrawLine(new Vector3(posi.x + radius, posi.y + radius, 0.0f), new Vector3(posi.x + radius, posi.y - radius, 0.0f));//右
    }
#endif

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform t = this.transform;

        Vec = t.position;//事前に座標を避難
        t.position = Move(StartPosition.position, EndPosition.position, (float)FlameCount / (float)MoveFlame);//移動先の座標を取得
        Vec = new Vector2(t.position.x, t.position.y) - Vec;//移動ベクトルを作成

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


    //移動関数
    //Timeは0.0f~1.0f
    //Start始点
    //End終点　
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
            player = null;
        }
    }
}
