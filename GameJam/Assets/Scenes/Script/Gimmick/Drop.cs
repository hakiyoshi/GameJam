using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    private float Speed;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //下に落下
        rb.position += new Vector2(0.0f, -Speed);

        if (rb.position.y <= -50.0f)//何もなくてそのまま落ちていってしまった場合
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Lava" && collision.gameObject.transform.position.y + 0.3f > this.transform.position.y)
        {
            Destroy(this.gameObject);//自分を削除
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Lava" && collision.gameObject.transform.position.y + 0.3f > this.transform.position.y)
        {
            Destroy(this.gameObject);//自分を削除
        }
    }

    public void SetSpeed(float _speed)
    {
        Speed = _speed;
    }
}
