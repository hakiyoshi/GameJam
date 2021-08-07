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
        //â∫Ç…óéâ∫
        rb.position += new Vector2(0.0f, -Speed);

        if (rb.position.y <= -50.0f)//âΩÇ‡Ç»Ç≠ÇƒÇªÇÃÇ‹Ç‹óéÇøÇƒÇ¢Ç¡ÇƒÇµÇ‹Ç¡ÇΩèÍçá
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Lava" && collision.gameObject.transform.position.y > this.transform.position.y)
        {
            Destroy(this.gameObject);//é©ï™ÇçÌèú
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Lava" && collision.gameObject.transform.position.y > this.transform.position.y)
        {
            Destroy(this.gameObject);//é©ï™ÇçÌèú
        }
    }

    public void SetSpeed(float _speed)
    {
        Speed = _speed;
    }
}
