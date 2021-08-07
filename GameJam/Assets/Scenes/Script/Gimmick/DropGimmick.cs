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

    private int FlameCount = 0;

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
        if (FlameCount == DropIntervalFlame)//指定のフレームで処理
        {
            GameObject CreateObj = GameObject.Instantiate(DropObject);
            CreateObj.GetComponent<Drop>().SetSpeed(Speed);
            CreateObj.transform.position = this.transform.position;
            FlameCount = 0;
        }

        FlameCount++;
    }
}
