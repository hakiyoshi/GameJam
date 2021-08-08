using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLastField : MonoBehaviour
{
    public Vector3 LastPosition { get; set; }//プレイヤーが最後に着地していた座標
    public bool LastPosiFlag { get; set; }//最終地点を記録するかしないか

    // Start is called before the first frame update
    void Start()
    {
        LastPosiFlag = true;
        LastPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HitGround(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitGround(collision);
    }


    private void HitGround(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ground" && !LastPosiFlag)
            return;

        Vector3 vec = (collision.transform.position - this.transform.position) / 4.0f;
        LastPosition = (this.transform.position + vec) + new Vector3(0.0f, 1.0f, 0.0f);
    }
}
