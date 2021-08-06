using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icecle : MonoBehaviour
{
    [Header("キャラクターのオブジェクトを選択する")]
    public GameObject Charcter;
    //キャラクターのポジションを取得
    Vector3 CharPos;
    //つららの座標を取得
    Vector3 Ice;
    //ギミック自体の座標
    float IcePos;
    [Header("つららが反応する範囲")]
    public float Range;
    [Header("つららが落ちる速度")]
    public float YSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //キャラクターのオブジェクトのコンポーネントを取得する
        GameObject Charcter = GetComponent<GameObject>();
        //つららの座標を取得
        Ice = this.transform.position;
        IcePos = Ice.y;
    }

    // Update is called once per frame
    void Update()
    {
        //キャラクターの座標を取得する
        CharPos = Charcter.transform.position;
        //キャラクターとつららの範囲を確認する
        if (-Range < (Ice.x - CharPos.x) && Range > (Ice.x - CharPos.x))
        {
            IcePos -= YSpeed;
            transform.position = new Vector2(Ice.x, IcePos);
        }
    }
}
