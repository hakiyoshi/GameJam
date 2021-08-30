using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusMove : MonoBehaviour
{
    [Header("カメラからずらす距離")]
    [SerializeField] Vector3 ShiftSize = Vector3.zero;

    //カメラの座標
    private Transform cameraposi = null;

    //アクティブフラグ
    private VirusActiveManager active;

    //出現
    [Header("出現速度")]
    [SerializeField] float activespeed = 1.0f;

    private Animator anime;

    private void OnEnable()
    {
        active = this.transform.parent.GetComponent<VirusActiveManager>();
        cameraposi = Camera.main.transform;//カメラの座標を取得

        //移動
        this.transform.position = CalPosition();

        anime = this.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (active.IfActive(VirusActiveManager.ACTIVE.FORCED))//強制移動しているか
        {
            this.transform.position = CalPosition();
        }
        else if (active.IfActive(VirusActiveManager.ACTIVE.ACTIVE))//出現
        {
            PlayActive();
        }
        else if (active.IfActive(VirusActiveManager.ACTIVE.END) && anime.GetCurrentAnimatorStateInfo(0).IsName("EndIdle"))
        {
            this.gameObject.SetActive(false);
        }
    }

    //出現
    private void PlayActive()
    {
        this.transform.position = new Vector3(CalPosition().x, this.transform.position.y - activespeed, 0.0f);

        if (CalPosition().y >= this.transform.position.y)//カメラの位置を通り過ぎたら
        {
            active.SetActiveFlag(VirusActiveManager.ACTIVE.FORCED);
        }
    }

    //座標計算
    private Vector3 CalPosition()
    {
        return ChangeCameraPosition() + ShiftSize;
    }

    //カメラ座標を使いやすいように変換
    private Vector3 ChangeCameraPosition()
    {
        return new Vector3(cameraposi.position.x, cameraposi.position.y, 0.0f);
    }

    public void StartActive()
    {
        this.transform.position = CalPosition() + new Vector3(0.0f, 60.0f, 0.0f);
    }

    public void EndActiveStart()
    {
        anime.SetTrigger("End");
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        
        //cameraposi = Camera.main.transform;
        //this.transform.position = CalPosition();
    }

#endif
}
