using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //上、左右の耐性オブジェクト(仮)
    GameObject[] Cols;

    //Rigidbody2D
    Rigidbody2D rb;

    //耐性を持った部分色付け用(仮)
    Color setColor;

    //Gimmick取得用レイヤー
    LayerMask Gimmick_Layer;

    //ギミックの当たり判定用bool
    bool bNeedle,bFire,bWater;

    //移動方向判別用
    float direction;

    //プレイヤーの角度用
    float rotate;

    //プレイヤーの回転角度計算用
    float now_Rotate;

    //2段ジャンプカウント用
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        Cols = new GameObject[3];

        Cols[0] = transform.Find("Right").gameObject;

        Cols[1] = transform.Find("Top").gameObject;
        
        Cols[2] = transform.Find("Left").gameObject;

        for(int i = 0; i < Cols.Length; i++)
        {
            Cols[i].GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        }

        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        rb = GetComponent<Rigidbody2D>();

        bNeedle = bFire = bWater = false;

        direction = 0f;

        rotate = 0f;

        now_Rotate = 0f;

        jumpCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        StartCoroutine("Colision");

        Collision2();
    }

    //キーボード入力等の処理
    void Move()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount != 0)
        {
            rb.velocity = Vector2.zero;

            jumpCount -= 1;

            rb.AddForce(Vector2.up * 800);

            StartCoroutine("Rotarion");
        }

        if (Input.GetKey(KeyCode.D))
            direction = 3f;
        else if (Input.GetKey(KeyCode.A))
            direction = -3f;
        else
            direction = 0f;

        rb.velocity = new Vector2(direction, rb.velocity.y);

        this.transform.eulerAngles = new Vector3(0, 0, rotate);
    }

    //ジャンプ時の回転の処理
    IEnumerator Rotarion()
    {
        float time = 0.0f;
        now_Rotate -= 90;

        if (now_Rotate < -359)
                now_Rotate = 0f;

        while (time <= 0.2f)
        {
            rotate = Mathf.Lerp(now_Rotate + 90, now_Rotate, time / 0.1f);
            time += Time.deltaTime;
            yield return 0;
        }
    }

    //ギミックに当たった時の処理(仮)
    IEnumerator Colision()
    {
        if(bNeedle || bFire || bWater)
        {
            int i = Mathf.Abs((int)(now_Rotate/ 90)) - 1;

            if (0 <= i && i <= 2)
            {
                        Cols[i].GetComponent<Renderer>().material.color = setColor;
            }
            else
            {
                for (int j = 0; j < Cols.Length; j++)
                        Cols[j].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            }

            yield return new WaitForSeconds(0.01f);

            this.transform.transform.position = new Vector2(-9f, -1f);
            now_Rotate = rotate = 0f;
            bNeedle = bFire = bWater = false;
        }
        
    }

    //Ray当たり判定取得用(開発中)
    void Collision2()
    {
        //RaycastHit2D[,] hits = new RaycastHit2D[4, 6];

        //for (int i = 0; i < 4; i++)
        //{

        //    Vector3 ray_Direction = Vector3.zero;
        //    switch (i)
        //    {
        //        case 0:
        //            ray_Direction = transform.right;
        //            break;
        //        case 1:
        //            ray_Direction = -transform.right;
        //            break;
        //        case 2:
        //            ray_Direction = transform.up;
        //            break;
        //        case 3:
        //            ray_Direction = -transform.up;
        //            break;
        //    }


        //    for (int j = 0; j < 6; j++)
        //    {
        //        Vector3 now_Position = new Vector3(this.transform.position.x, this.transform.position.y + 2 * i);

        //    }
        //}

        RaycastHit2D[] hits = new RaycastHit2D[4];
        Vector3 now_Position = new Vector3 (this.transform.position.x,this.transform.position.y);
        Vector3[] end_Position = new Vector3[4];
        end_Position[0] = now_Position + transform.right * 3f;
        end_Position[1] = now_Position - transform.right * 3f;
        end_Position[2] = now_Position + transform.up * 3f;
        end_Position[3] = now_Position - transform.up * 3f;

        hits[0] = Physics2D.Linecast(now_Position, end_Position[0], Gimmick_Layer);
        hits[1] = Physics2D.Linecast(now_Position, end_Position[1], Gimmick_Layer);
        hits[2] = Physics2D.Linecast(now_Position, end_Position[2], Gimmick_Layer);
        hits[3] = Physics2D.Linecast(now_Position, end_Position[3], Gimmick_Layer);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i])
                Debug.DrawLine(now_Position, end_Position[i], Color.red);
            else
                Debug.DrawLine(now_Position, end_Position[i], Color.blue);

        }

    }

    //当たり判定取得用(仮)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 2;
        }
        if (collision.gameObject.CompareTag("Needle"))
        {
            bNeedle = true;
            setColor = Color.gray;
        }
        else if (collision.gameObject.CompareTag("Lava"))
        {
            bFire = true;
            setColor = Color.red;
        }
        else if (collision.gameObject.CompareTag("Ice"))
        {
            bWater = true;
            setColor = Color.cyan;
        }
    }

}
