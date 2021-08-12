using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGimmick : MonoBehaviour
{

    [Header("雫を落とす間隔(フレーム)")]
    [SerializeField] int DropIntervalFlame;//落とす間隔

    [Header("落下速度")]
    [SerializeField] float Speed;//落下速度

    [Header("落とすオブジェクト")]
    [SerializeField] GameObject DropObject;//雫オブジェクト

    [Header("ギミック作動距離")]
    [SerializeField] float DropStartLength = 50.0f;

    private int FlameCount = 0;

    private Transform rp;//リスポーンポイント

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        rp = this.GetComponent<RespawnPoint>().GetRespawnPoint();

        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (FlameCount == DropIntervalFlame)//指定のフレームで処理
        {
            GameObject CreateObj = GameObject.Instantiate(DropObject);
            Drop drop = CreateObj.GetComponent<Drop>();
            drop.SetSpeed(Speed);
            drop.GetComponent<RespawnPoint>().SetRespawnPoint(rp);
            CreateObj.transform.position = this.transform.position;
            FlameCount = 0;
        }

        if (Mathf.Abs(player.position.x - this.transform.position.x) <= DropStartLength)
        {
            FlameCount++;
        }
    }
}
