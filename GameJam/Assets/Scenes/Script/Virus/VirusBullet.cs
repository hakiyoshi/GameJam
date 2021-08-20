using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBullet : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] float MoveSpeed = 0.8f;

    //敵本体座標
    private Transform MasterVirus;

    //落下位置
    private float fallposi;
    private Vector3 posi = Vector3.zero;//座標
    private bool fallflag = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //移動
        if(!fallflag)//前進
        {
            this.transform.position += CalMoveVec();
        }
        else//落下
        {
            this.transform.position = CalFallVec();
        }


        if (MasterVirus.position.y + 20.0f <= this.transform.position.y)//一定の高さに達したら
        {
            this.transform.rotation = Quaternion.AngleAxis(-90.0f, new Vector3(0.0f, 0.0f, 1.0f));//弾の進行方向を下にする
            this.transform.position = CalFallPosition();
            fallflag = true;
        }
        else if (this.transform.position.y <= -40.0f)
        {
            Destroy(this.gameObject);
        }
    }

    //移動
    Vector3 CalMoveVec()
    {
        Vector3 vec = this.transform.rotation * new Vector3(1.0f, 0.0f, 0.0f);
        return vec * MoveSpeed;
    }

    //落下
    Vector3 CalFallVec()
    {
        return new Vector3(MasterVirus.position.x + fallposi + 12.0f, this.transform.position.y, 0.0f) + CalMoveVec();
    }

    //落下座標初期値
    Vector3 CalFallPosition()
    {
        return new Vector3(MasterVirus.position.x + fallposi + 12.0f, MasterVirus.position.y + 20.0f, 0.0f);
    }

    //virus 敵本体座標
    //fallposi 敵から見て落下する距離
    public void CreateSet(Transform virus, float fallposi)
    {
        MasterVirus = virus;
        this.fallposi = fallposi;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(CalFallPosition(), CalFallPosition() + new Vector3(0.0f, -100.0f, 0.0f));
    }

#endif

}
