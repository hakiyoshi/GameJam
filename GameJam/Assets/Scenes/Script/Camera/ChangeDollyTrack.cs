using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeDollyTrack : MonoBehaviour
{
    //トラック
    [SerializeField] CinemachineSmoothPath[] track;

    //カート
    private CinemachineDollyCart cart;

    //現在のトラック
    private int nowtrack = 0;

    private void Start()
    {
        //カート取得
        cart = this.GetComponent<CinemachineDollyCart>();

        //パスを一番最初に設定
        ChangePath(nowtrack);

        //コルーチンスタート
        StartCoroutine("CheckCartPosition");
    }

    IEnumerator CheckCartPosition()
    {
        while (true)
        {
            //パスが最後当たりに近づいたら次のパスに移行
            if (cart.m_Position >= track[nowtrack].PathLength - 1.0f)
            {
                if (track.Length <= nowtrack - 1)//次のパスがない場合は終了
                    yield break;

                //次のトラックに変更
                nowtrack++;

                //トラックをセット
                ChangePath(nowtrack);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //パス変更
    void ChangePath(int id)
    {
        cart.m_Path = track[id];
        cart.m_Position = 0.0f;
    }
}
