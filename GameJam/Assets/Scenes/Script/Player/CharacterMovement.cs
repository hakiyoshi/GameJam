using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //PlayerGimmickスクリプト
    PlayerGimmick PG;

    //上、左右の耐性オブジェクト(仮)
    GameObject[] Cols;

    //各耐性のスプライト
    public Sprite NeedleSprite;
    public Sprite LavaSprite;
    public Sprite IceSprite;

    //Rigidbody2D
    Rigidbody2D rb;

    //Gimmick取得用レイヤー
    LayerMask Gimmick_Layer;

    //死亡時の表情変化用
    Animator anim;

    //生死判定用のbool
    bool bDeth;

    //針ギミック用bool
    bool bNeedle;
    //マグマギミック用bool
    bool bLava;

    //移動方向判別用
    [Header("移動速度")]
    [SerializeField] float MaxSpeed;//最高速度
    float direction;

    [Header("ジャンプの高さ")]
    [SerializeField] float JumpPower;//ジャンプの高さ

    [Header("慣性 初期0.99")]
    [SerializeField] float Inertia;//慣性

    //プレイヤーの角度用
    float rotateZ;
    float rotateY;

    //プレイヤーの回転角度計算用
    float now_Rotate;

    //2段ジャンプカウント用
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        PG = this.GetComponent<PlayerGimmick>();

        //ギミック耐性オブジェクト用配列
        Cols = new GameObject[3];
        //右のギミック耐性オブジェクト用
        Cols[0] = transform.Find("Right").gameObject;
        //上のギミック耐性オブジェクト用
        Cols[1] = transform.Find("Top").gameObject;
        //左のギミック耐性オブジェクト用
        Cols[2] = transform.Find("Left").gameObject;

        //各オブジェクトを透明化(仮)
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        }

        //Rayで判定するレイヤー設定
        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        //物理演算コンポーネント取得
        rb = GetComponent<Rigidbody2D>();

        //アニメーター取得
        anim = this.GetComponent<Animator>();

        //生死判定用のbool初期化
        SetDeth(false);

        //各ギミック判定用bool初期化
        bNeedle = false;
        bLava = false;

        //移動方向用数値初期化
        direction = 0f;

        //角度初期化
        rotateZ = 0f;
        rotateY = 0f;

        //回転角度初期化
        now_Rotate = 0f;

        //ジャンプカウント初期化
        jumpCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PG.GetHitMoveFlag())
        {
            anim.SetBool("isDeth", false);
            Move();
            StartCoroutine("Collision");
        }
        else
        {
            anim.SetBool("isDeth", true);
        }

        //回転処理反映 
        this.rb.transform.eulerAngles = new Vector3(0, rotateY, rotateZ);
    }

    //キーボード入力等の処理
    void Move()
    {
        //スペースキーを押したときのジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount != 0)
        {
            //物理演算リセット
            rb.velocity = Vector2.zero;

            //ジャンプカウント-1
            jumpCount -= 1;

            //ジャンプ処理
            rb.AddForce(Vector2.up * JumpPower);

            if (0 < this.transform.localScale.x)
            {
                //回転処理開始
                StartCoroutine("Rotation", -90);
            }
            else
            {
                //回転処理開始
                StartCoroutine("Rotation", 90);
            }
        }

        //キー入力での移動処理
        if (Input.GetKey(KeyCode.D))
        {
            direction = MaxSpeed;
            
            if(jumpCount == 2)
                rotateY = 0;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -MaxSpeed;

            if (jumpCount == 2)
                rotateY = 180;
        }


        //左右移動
        rb.position += new Vector2(direction, 0.0f);

        //疑似完成
        direction *= Inertia;
    }

    //ジャンプ時の回転の処理(コルーチン)
    IEnumerator Rotation(float angle)
    {
        //回転速度インターバル初期化
        float time = 0.0f;

        //回転角度計算
        now_Rotate += angle;

        //360度を超えたら0に戻す
        if (now_Rotate < -359 || 359 < now_Rotate)
            now_Rotate = 0f;

        //回転処理実行
        while (time <= 0.2f)
        {
                //今の角度から90度回転
                rotateZ = Mathf.Lerp(now_Rotate - angle, now_Rotate, time / 0.1f);

            //インターバル加算
            time += Time.deltaTime;

            yield return 0;
        }
    }

    //Ray当たり判定取得用(開発中)&(コルーチン)
    IEnumerator Collision()
    {
        //当たり判定用Ray
        RaycastHit2D[] hits = new RaycastHit2D[12];
        //rayの長さ
        float end_distance = 1.4f;
        //方向ベクトル
        Vector3[] Dire_Vec = { rb.transform.right * end_distance,       //右
                                    rb.transform.up * end_distance,     //上
                                    -rb.transform.right * end_distance, //左
                                    -rb.transform.up * end_distance};   //下 
        //rayの始点
        Vector3 sta_Position = new Vector3(this.rb.transform.position.x + this.GetComponent<BoxCollider2D>().offset.x
                                         , this.rb.transform.position.y + this.GetComponent<BoxCollider2D>().offset.y);
        //rayの終点配列
        Vector3[] end_Position = new Vector3[12];

        //rayの各終点設定(上下座右)
        end_Position[0] = sta_Position + Dire_Vec[0];
        end_Position[1] = end_Position[0] + Dire_Vec[1] / 2f;
        end_Position[2] = end_Position[0] + Dire_Vec[3] / 2f;
        end_Position[3] = sta_Position + Dire_Vec[1];
        end_Position[4] = end_Position[3] + Dire_Vec[0] / 2f;
        end_Position[5] = end_Position[3] + Dire_Vec[2] / 2f;
        end_Position[6] = sta_Position + Dire_Vec[2];
        end_Position[7] = end_Position[6] + Dire_Vec[1] / 2f;
        end_Position[8] = end_Position[6] + Dire_Vec[3] / 2f;
        end_Position[9] = sta_Position + Dire_Vec[3];
        end_Position[10]= end_Position[9] + Dire_Vec[0] / 2f;
        end_Position[11]= end_Position[9] + Dire_Vec[2] / 2f;


        //rayの各設定(上下座右)
        for (int i = 0; i < hits.Length; i++)
            hits[i] = Physics2D.Linecast(sta_Position, end_Position[i], Gimmick_Layer);

        //変更スプライト格納用
        Sprite change_Sprite = null;

        //当たり判定確認用ループ
        for (int i = 0; i < hits.Length; i++)
        {
            //i番目のRayが当たったか
            if (hits[i])
            {
                //デバッグでLineを見る用
                Debug.DrawLine(sta_Position, end_Position[i], Color.red);

                //足元以外に当たったか
                if (0 <= i && i <= 8)
                {
                    //当たった部分に色(耐性)を表示
                    Cols[i / 3].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

                    //針ギミックに当たったか
                    if (hits[i].collider.gameObject.tag == "Needle" && bNeedle)
                    {
                        //変更スプライトを針に設定
                        change_Sprite = NeedleSprite;
                    }
                    //マグマギミックに当たったか
                    else if (hits[i].collider.gameObject.tag == "Lava")
                    {
                        //変更スプライトをマグマに設定
                        change_Sprite = LavaSprite;
                    }
                    //氷ギミックに当たったか
                    else if (hits[i].collider.gameObject.tag == "Ice")
                    {
                        //変更スプライトを氷に設定
                        change_Sprite = IceSprite;
                    }

                    //当たった面がギミック耐性を持っていないか判定
                    if (Cols[i / 3].GetComponent<SpriteRenderer>().sprite != change_Sprite)
                    {
                        //ギミックに対応した耐性を付与
                        Cols[i / 3].GetComponent<SpriteRenderer>().sprite = change_Sprite;

                        //ギミックにヒットしたことを通知してチェックポイントに戻す
                        PG.HitGimmick(hits[i].collider);

                        yield return new WaitForSeconds(0.2f);

                        //各角度リセット
                        now_Rotate = rotateZ = 0f;
                    }
                        break;
                }
                //足元に当たったら
                else
                {
                    //全免疫削除処理
                    for (int j = 0; j < Cols.Length; j++)
                    {
                        //全ての免疫を透明化
                        Cols[j].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
                        //ギミックに対応した耐性を付与
                        Cols[j].GetComponent<SpriteRenderer>().sprite = null;
                    }

                    PG.HitGimmick(hits[i].collider);
                    yield return new WaitForSeconds(0.2f);
                    //各角度リセット
                    now_Rotate = rotateZ = 0f;
                }

            }
            else
            {
                //デバッグでLineを見る用
                Debug.DrawLine(sta_Position, end_Position[i], Color.blue);
            }
        }
    }

    //生死判断用bool取得用
    public bool GetDeth()
    {
        return bDeth;
    }
    //生死判断用bool設定用
    public void SetDeth(bool set)
    {
        bDeth = set;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //地面との当たり判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            //ジャンプカウントリセット
            jumpCount = 2;
        }
    }
        //当たり判定取得用(仮)
        /*void OnCollisionEnter2D(Collision2D collision)
        {
            //地面との当たり判定
            if (collision.gameObject.CompareTag("Ground"))
            {
                //ジャンプカウントリセット
                jumpCount = 2;
            }

            //マグマとの当たり判定
            if (collision.gameObject.CompareTag("Lava"))
            {
                bLava = true;
            }
            //氷との当たり判定
            else if (collision.gameObject.CompareTag("Ice"))
            {
                bIce = true;
            }
        }
        */

        void OnTriggerEnter2D(Collider2D collision)
        {
            //針との当たり判定
            if (collision.gameObject.CompareTag("Needle"))
            {
                bNeedle = true;
            }

            //マグマとの当たり判定
            if (collision.gameObject.CompareTag("Lava"))
            {
                bLava = true;
            }
    }
}
