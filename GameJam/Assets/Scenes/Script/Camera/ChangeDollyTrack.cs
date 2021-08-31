using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeDollyTrack : MonoBehaviour
{
    //トラック
    [SerializeField] CinemachineSmoothPath[] track;

    //チェックポイント
    [SerializeField] Transform[] RespawnPoint;

    //カート
    private CinemachineDollyCart cart;

    //現在のトラック
    private int nowtrack = 0;

    //敵の強制移動
    [SerializeField] VirusActiveManager virus;

    private void Start()
    {
        //カート取得
        cart = this.GetComponent<CinemachineDollyCart>();

        //パスを一番最初に設定
        ChangePath(0);

        //コルーチンスタート
        StartCoroutine("CheckCartPosition");
    }

    IEnumerator CheckCartPosition()
    {
        while (true)
        {
            //パスが最後当たりに近づいたら次のパスに移行
            if (cart.m_Position >= track[nowtrack].PathLength)
            {
                //次のトラックに変更
                nowtrack++;

                //トラックをセット
                ChangePath(nowtrack);

                //次のパスがない場合は終了
                if (track.Length <= nowtrack + 1)
                {
                    nowtrack = -1;
                    yield break;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //パス変更
    void ChangePath(int id)
    {
        cart.m_Path = track[id];
        cart.m_Position = 0.0f;
        if (RespawnPoint[id] != null)
        {
            virus.SetRespawnPoint(RespawnPoint[id]);
        }
    }

    //強制移動再生中か
    public bool GetDollyCartPlay()
    {
        return nowtrack != -1 ? true : false;
    }
}
