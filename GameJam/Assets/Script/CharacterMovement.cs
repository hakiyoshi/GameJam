using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    GameObject[] Cols;

    Rigidbody2D rb;

    Color setColor;

    bool bNeedle,bFire,bWater;

    float direction;

    float rotate;

    float now_Rotate;

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
    }

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
        else if (collision.gameObject.CompareTag("Fire"))
        {
            bFire = true;
            setColor = Color.red;
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            bWater = true;
            setColor = Color.cyan;
        }
    }

}
