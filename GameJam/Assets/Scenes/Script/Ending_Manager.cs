using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending_Manager : MonoBehaviour
{
    //プレイヤーのデスカウントを保持する
    static int Dead_Count;

    // Start is called before the first frame update
    void Start()
    {
        //シーン移動したときにオブジェクトを破棄させない   
        //DontDestroyOnLoad(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //デスカウントの初期化(スタートボタンを押したときに呼び出す
    public static void Reset()
    {
        Dead_Count = 0;
    }

    //デスカウントを増やす(プレイヤーがギミックに接触した場合に呼び出す
    public static void AddDead_Count()
    {
        Dead_Count++;
       
    }

    public static int GetDead_Count()
    {
        return Dead_Count;
    }
}
