using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimmick : MonoBehaviour
{

    //必要なもの
    private Rigidbody2D rb;//物理
    private PlayerLastField last;//最後の着地点

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        last = this.GetComponent<PlayerLastField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lava")//マグマヒット時
        {
            rb.position = last.LastPosition;
        }
    }


}
