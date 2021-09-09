using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //PlayerGimmickスクリプト
    PlayerGimmick PG;

    PauseController PC;

    Goal_Check GC;

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

    Vector3[] StartPos;
    Vector3[] EndPos;

    float RayDistance;

    float RayDistanceBottom;

    // Start is called before the first frame update
    void Start()
    {
        PG = this.GetComponent<PlayerGimmick>();

        PC = GameObject.Find("Pause").GetComponent<PauseController>();

        GC = GameObject.Find("Goal").GetComponent<Goal_Check>();

        //ギミック耐性オブジェクト用配列
        Cols = new GameObject[3];

        ////右のギミック耐性オブジェクト用
        //Cols[0] = transform.Find("Right").gameObject;
        ////上のギミック耐性オブジェクト用
        //Cols[1] = transform.Find("Top").gameObject;
        ////左のギミック耐性オブジェクト用
        //Cols[2] = transform.Find("Left").gameObject;

        //各抗体オブジェクトを初期化
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i] = this.transform.GetChild(i).gameObject;
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

        RayDistance = 1.35f;

        RayDistanceBottom = 1f;
    }

    void Update()
    {
        if (!PG.GetHitMoveFlag())
        {
            //生き返った時の表情変更
            anim.SetBool("isDeth", false);

            if (Time.timeScale != 0 && !PC.GetbChangeScene() && !GC.GetbGoal())
            {
                Jump();
                cc.enabled = true;
                MakeRay();
                CollisionRay();

                if (rotateZ == 0 && !bJump)
                    RayDistanceBottom = 1f;
                else
                    RayDistanceBottom = 1.35f;
            }
        }
        else
        {
            //死んだときの表情変更
            anim.SetBool("isDeth", true);
            cc.enabled = false;
            //各角度リセット
            now_Rotate = rotateZ = 0f;
        }
    }

    void FixedUpdate()
    {
        if (!PG.GetHitMoveFlag())
        {
            if (Time.timeScale != 0 && !PC.GetbChangeScene() && !GC.GetbGoal())
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
            //ジャンプカウント-1
            jumpCount--;

            bJump = true;

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
    void MakeRay()
    {
        int RayCount = 4;

        hits = new RaycastHit2D[RayCount];

        //方向ベクトル
        Vector3[] Dire_Vec = { transform.right, //右
                               transform.up,    //上
                              -transform.right, //左
                              -transform.up};   //下 

        StartPos = new Vector3[RayCount];

        EndPos = new Vector3[RayCount];

        StartPos[0] = this.transform.position + (Dire_Vec[1] * 1.6f + Dire_Vec[2] * RayDistance);
        EndPos[0] = this.transform.position + (Dire_Vec[1] * 1.6f + Dire_Vec[0] * RayDistance);
        
        StartPos[1] = this.transform.position + (Dire_Vec[0] * 1.4f + Dire_Vec[1] * RayDistance);
        EndPos[1] = this.transform.position + (Dire_Vec[0] * 1.4f + Dire_Vec[3] * RayDistance);

        StartPos[2] = this.transform.position + (Dire_Vec[2] * 1.4f + Dire_Vec[1] * RayDistance);
        EndPos[2] = this.transform.position + (Dire_Vec[2] * 1.4f + Dire_Vec[3] * RayDistance);

        StartPos[3] = this.transform.position + (Dire_Vec[3] * 1.65f + Dire_Vec[2] * RayDistanceBottom);
        EndPos[3] = this.transform.position + (Dire_Vec[3] * 1.65f + Dire_Vec[0] * RayDistanceBottom);

        for (int i = 0; i < RayCount; i++)
        {
            hits[i] = Physics2D.Linecast(StartPos[i], EndPos[i], Gimmick_Layer);
            Debug.DrawLine(StartPos[i], EndPos[i], Color.yellow);
        }
    }

    void CollisionRay()
    {
        //変更スプライト格納用
        Sprite change_Sprite = null;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i])
            {
                //デバッグでLineを見る用
                Debug.DrawLine(StartPos[i], EndPos[i], Color.red);

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

                //足元に当たったら
                if (i == 3)
                {
                    Deth();
                    PG.HitGimmick(hits[i].collider);

                    if (DamageSound != null)
                        AudioManager.PlayAudio(DamageSound, false, false);

                    Ending_Manager.AddDead_Count();
                    break;
                }
                //足元以外に当たったか
                else
                {
                    //当たった部分に色(耐性)を表示
                    Cols[i].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

                    jumpCount = 1;
                    //当たった面がギミック耐性を持っていないか判定
                    if (Cols[i].GetComponent<SpriteRenderer>().sprite != change_Sprite)
                    {

                        UseInertia = 0.0f;//慣性を消す

                        //ギミックに対応した耐性を付与
                        Cols[i].GetComponent<SpriteRenderer>().sprite = change_Sprite;
                        Cols[i].GetComponent<BoxCollider2D>().enabled = true;

                        if (DamageSound != null)
                            AudioManager.PlayAudio(DamageSound, false, false);

                        //ギミックにヒットしたことを通知してチェックポイントに戻す
                        PG.HitGimmick(hits[i].collider);

                        Ending_Manager.AddDead_Count();

                        jumpCount = 2;
                        break;
                    }
                }
            }

        }
    }

    void Deth()
    {
        PreGimmick = null;

        UseInertia = 0.0f;//慣性を消す
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
            //ジャンプカウントリセット
            jumpCount = 2;
            UseInertia = Inertia;//慣性を付ける
        }

        if (collision.gameObject.CompareTag("Virus"))
        {
            Deth();
            PG.HitGimmick(collision);
        }
    }

}
