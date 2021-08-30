using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //PlayerGimmickスクリプト
    PlayerGimmick PG;

    //上、左右の耐性オブジェクト(仮)
    GameObject[] Cols;

    GameObject PreGimmick;

    //各耐性のスプライト
    public Sprite NeedleSprite;
    public Sprite LavaSprite;
    public Sprite IceSprite;

    //Rigidbody2D
    Rigidbody2D rb;

    CapsuleCollider2D cc;

    //Gimmick取得用レイヤー
    LayerMask Gimmick_Layer;

    //死亡時の表情変化用
    Animator anim;

    //移動方向判別用
    [Header("移動速度")]
    [SerializeField] float MaxSpeed;//最高速度
    float init_Maxspeed;
    float direction;

    [Header("ダッシュ時移動速度")]
    public float dashPower = 1.5f;

    [Header("ジャンプの高さ")]
    [SerializeField] float JumpPower;//ジャンプの高さ

    [Header("慣性 初期0.99")]
    [SerializeField] float Inertia;//慣性
    private float UseInertia;//使う慣性

    //プレイヤーの角度用
    float rotateZ;
    float rotateY;

    //プレイヤーの回転角度計算用
    float now_Rotate;

    //2段ジャンプカウント用
    int jumpCount;

    string DamageSound;

    bool bJump;

    RaycastHit2D[] hits;

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
            Cols[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        PreGimmick = null;

        //Rayで判定するレイヤー設定
        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        //物理演算コンポーネント取得
        rb = GetComponent<Rigidbody2D>();

        cc = GetComponent<CapsuleCollider2D>();

        //アニメーター取得
        anim = this.GetComponent<Animator>();

        //移動方向用数値初期化
        direction = 0f;

        init_Maxspeed = MaxSpeed;

        //角度初期化
        rotateZ = 0f;
        rotateY = 0f;

        //回転角度初期化
        now_Rotate = 0f;

        //ジャンプカウント初期化
        jumpCount = 2;

        //慣性セット
        UseInertia = Inertia;

        DamageSound = null;

        bJump = false;
    }

    void Update()
    {
        if (!PG.GetHitMoveFlag())
        {
            //生き返った時の表情変更
            anim.SetBool("isDeth", false);

            if (Time.timeScale != 0)
            {
                Collision();
                Jump();
                cc.enabled = true;

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit)
                    {
                        bJump = false;
                        jumpCount = 1;
                        PreGimmick = hit.collider.gameObject;
                    }
                    else if(!bJump && PreGimmick != null && PreGimmick.layer == 6)
                    {
                        jumpCount = 2;
                    }
                }
            }
        }
        else
        {
            //死んだときの表情変更
            anim.SetBool("isDeth", true);
            cc.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (!PG.GetHitMoveFlag())
        {
            if (Time.timeScale != 0)
            {
                //移動
                Move();
            }
        }

        //回転処理反映 
        this.rb.transform.eulerAngles = new Vector3(0, rotateY, rotateZ);
    }

    //キーボード入力等の処理
    void Move()
    {
        //ダッシュ判定用
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MaxSpeed = init_Maxspeed * dashPower;
        }
        else
        {
            MaxSpeed = init_Maxspeed;
        }

        //キー入力での移動処理
        if (Input.GetKey(KeyCode.D))
        {
            direction = MaxSpeed;

            if (jumpCount == 2)
            {
                //プレイヤーの左右反転
                rotateY = 0;

                //耐性が壁に埋もれないように修正
                for (int i = 0; i < Cols.Length; i++)
                {
                    Cols[i].transform.localPosition = new Vector3(Cols[i].transform.localPosition.x,
                                                                  Cols[i].transform.localPosition.y,
                                                                  -1f);
                }
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = -MaxSpeed;

            if (jumpCount == 2)
            {
                //プレイヤーの左右反転
                rotateY = 180;

                //耐性が壁に埋もれないように修正
                for (int i = 0; i < Cols.Length; i++)
                {
                    Cols[i].transform.localPosition = new Vector3(Cols[i].transform.localPosition.x,
                                                                  Cols[i].transform.localPosition.y,
                                                                  3f);
                }
            }
        }

        //左右移動
        rb.position += new Vector2(direction, 0.0f);

        //疑似完成
        direction *= UseInertia;
    }

    void Jump()
    {
        //スペースキーを押したときのジャンプ処理
        if (((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) && 0 < jumpCount)
        {
            bJump = true;
            //ジャンプカウント-1
            jumpCount--;

            AudioManager.PlayAudio("Jamp", false, false);

            //物理演算リセット
            rb.velocity = Vector2.zero;

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
    void Collision()
    {
        //当たり判定用Ray
        hits = new RaycastHit2D[20];

        //方向ベクトル
        Vector3[] Dire_Vec = { rb.transform.right, //右
                               rb.transform.up,    //上
                              -rb.transform.right, //左
                              -rb.transform.up};   //下 

        //rayの始点
        Vector3 sta_Position = new Vector3(this.rb.transform.position.x + cc.offset.x
                                         , this.rb.transform.position.y + cc.offset.y);
        //rayの終点配列
        Vector3[] end_Position = new Vector3[20];

        float end_distance = 1.35f;

        //rayの各終点設定(右)
        end_Position[0] = sta_Position + Dire_Vec[0] * end_distance;
        end_Position[1] = end_Position[0] + Dire_Vec[1] * 0.5f;
        end_Position[2] = end_Position[0] + Dire_Vec[1] * end_distance * 0.7f;
        end_Position[3] = end_Position[0] + Dire_Vec[3] * 0.5f;
        end_Position[4] = end_Position[0] + Dire_Vec[3] * end_distance * 0.7f;

        //rayの各終点設定(上)
        end_Position[5] = sta_Position + Dire_Vec[1] * end_distance * 1.25f;
        end_Position[6] = end_Position[5] + Dire_Vec[0] * 0.5f;
        end_Position[7] = end_Position[5] + Dire_Vec[0] * 1.2f;
        end_Position[8] = end_Position[5] + Dire_Vec[2] * 0.5f;
        end_Position[9] = end_Position[5] + Dire_Vec[2] * 1.2f;

        //rayの各終点設定(左)
        end_Position[10] = sta_Position + Dire_Vec[2] * end_distance;
        end_Position[11] = end_Position[10] + Dire_Vec[1] * 0.5f;
        end_Position[12] = end_Position[10] + Dire_Vec[1] * end_distance * 0.7f;
        end_Position[13] = end_Position[10] + Dire_Vec[3] * 0.5f;
        end_Position[14] = end_Position[10] + Dire_Vec[3] * end_distance * 0.7f;

        //rayの各終点設定(下)
        end_Position[15] = sta_Position + Dire_Vec[3] * end_distance * 1.25f;
        end_Position[16] = end_Position[15] + Dire_Vec[0] * 0.6f;
        end_Position[17] = sta_Position + Dire_Vec[3] + Dire_Vec[0];
        end_Position[18] = end_Position[15] + Dire_Vec[2] * 0.6f;
        end_Position[19] = sta_Position + Dire_Vec[3] + Dire_Vec[2];

        //rayの各設定(上下座右)
        for (int i = 0; i < hits.Length; i++)
            hits[i] = Physics2D.Linecast(sta_Position, end_Position[i], Gimmick_Layer);

        //変更スプライト格納用
        Sprite change_Sprite = null;

        //当たり判定確認用ループ
        for (int i = hits.Length - 1; 0 <= i; i--)
        {
            //i番目のRayが当たったか
            if (hits[i])
            {
                //デバッグでLineを見る用
                Debug.DrawLine(sta_Position, end_Position[i], Color.red);

                if (hits[i].collider.gameObject.name == "DropLava(Clone)")
                {
                    Destroy(hits[i].collider.gameObject);
                }

                //針ギミックに当たったか
                if (hits[i].collider.gameObject.tag == "Needle")
                {
                    //変更スプライトを針に設定
                    change_Sprite = NeedleSprite;
                    DamageSound = "Damage_Needle";
                }
                //マグマギミックに当たったか
                else if (hits[i].collider.gameObject.tag == "Lava")
                {
                    Debug.Log(hits[i].collider.gameObject.name);

                    //変更スプライトをマグマに設定
                    change_Sprite = LavaSprite;
                    DamageSound = "Damage_Lava";
                }
                //氷ギミックに当たったか
                else if (hits[i].collider.gameObject.tag == "Ice" || hits[i].collider.gameObject.tag == "IceGround")
                {
                    //変更スプライトを氷に設定
                    change_Sprite = IceSprite;
                    DamageSound = "Damage_Ice";
                }
                else if (hits[i].collider.gameObject.tag == "Fall")
                {
                    DamageSound = null;
                    Deth();
                    PG.HitGimmick(hits[i].collider);
                }
                //足元以外に当たったか
                if (0 <= i && i <= 14)
                {
                    //当たった部分に色(耐性)を表示
                    Cols[i / 5].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

                    //当たった面がギミック耐性を持っていないか判定
                    if (Cols[i / 5].GetComponent<SpriteRenderer>().sprite != change_Sprite)
                    {
                        //ギミックに対応した耐性を付与
                        Cols[i / 5].GetComponent<SpriteRenderer>().sprite = change_Sprite;
                        Cols[i / 5].GetComponent<BoxCollider2D>().enabled = true;

                        if (DamageSound != null)
                            AudioManager.PlayAudio(DamageSound, false, false);

                        //ギミックにヒットしたことを通知してチェックポイントに戻す
                        PG.HitGimmick(hits[i].collider);

                        //各角度リセット
                        now_Rotate = rotateZ = 0f;

                        Ending_Manager.AddDead_Count();

                        break;
                    }
                }
                //足元に当たったら
                else
                {
                    Deth();
                    PG.HitGimmick(hits[i].collider);

                    if (DamageSound != null)
                        AudioManager.PlayAudio(DamageSound, false, false);

                    Ending_Manager.AddDead_Count();
                    break;
                }
            }
            else
            {
                Debug.DrawLine(sta_Position, end_Position[i], Color.yellow);
            }
        }
    }

    void Deth()
    {
        PreGimmick = null;

        //全免疫削除処理
        for (int j = 0; j < Cols.Length; j++)
        {
            //全ての免疫を透明化
            Cols[j].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
            Cols[j].GetComponent<BoxCollider2D>().enabled = false;
            //ギミックに対応した耐性を付与
            Cols[j].GetComponent<SpriteRenderer>().sprite = null;
        }
        //各角度リセット
        now_Rotate = rotateZ = 0f;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //氷の地面との当たり判定
        if (collision.gameObject.CompareTag("IceGround"))
        {
            //慣性を多めに付ける
            UseInertia = 0.995f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //地面との当たり判定
        if (collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("IceGround"))
        {
            UseInertia = 0.0f;//慣性を消す
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //地面との当たり判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            bJump = false;
            PreGimmick = null;
            //ジャンプカウントリセット
            jumpCount = 2;
            UseInertia = Inertia;//慣性を付ける
        }
    }
}
