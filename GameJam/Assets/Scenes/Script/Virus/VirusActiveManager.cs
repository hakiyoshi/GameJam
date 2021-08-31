using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusActiveManager : MonoBehaviour
{
    private ChangeCamera change;//強制移動してるか確認

    //フラグ
    public enum ACTIVE
    {
        NONE,
        ACTIVE,
        FORCED,
        END,
    }
    private ACTIVE activeflag = ACTIVE.NONE;

    [SerializeField] GameObject VirusObject;
    private VirusMove VirusMove;

    [SerializeField] List<RespawnPoint> VirusRP;//ウイルスのリスポーンポイント

    private void Awake()
    {
        VirusRP = new List<RespawnPoint>();//リスト生成
        VirusRP.Add(VirusObject.GetComponent<RespawnPoint>());//親入れる

        foreach (Transform child in VirusObject.transform)//子のリスポーンポイント受け取る
        {
            RespawnPoint rp;
            foreach (Transform item in child)
            {
                rp = item.GetComponent<RespawnPoint>();
                if (rp == null)//ない場合
                {
                    continue;
                }

                VirusRP.Add(rp);
            }

            rp = child.GetComponent<RespawnPoint>();
            if (rp == null)//ない場合
            {
                continue;
            }

            VirusRP.Add(rp);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        VirusObject.SetActive(false);//非アクティブにする
        change = Camera.main.GetComponent<ChangeCamera>();

        VirusMove = VirusObject.GetComponent<VirusMove>();

        

        

        StartCoroutine("CheckChange");
    }

    IEnumerator CheckChange()
    {
        while (true)
        {
            if (IfActive(ACTIVE.NONE) && change.IfCameraFlag(ChangeCamera.CAMERAFLAG.TARGET))//敵出現
            {
                activeflag = ACTIVE.ACTIVE;
                VirusObject.SetActive(true);
                VirusMove.StartActive();
            }
            else if (IfActive(ACTIVE.FORCED) && change.IfCameraFlag(ChangeCamera.CAMERAFLAG.TARGET))//強制移動開始
            {
                change.StartDollyCart();//強制移動開始
            }
            else if (IfActive(ACTIVE.FORCED) && change.IfCameraFlag(ChangeCamera.CAMERAFLAG.MAIN))
            {
                activeflag = ACTIVE.END;
            }
            else if (IfActive(ACTIVE.END))
            {
                VirusMove.EndActiveStart();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //アクティブフラグ比較
    public bool IfActive(ACTIVE flag)
    {
        return activeflag == flag ? true : false;
    }

    public ACTIVE GetActiveFlag()
    {
        return activeflag;
    }

    public void SetActiveFlag(ACTIVE active)
    {
        activeflag = active;
    }

    public void SetRespawnPoint(Transform rp)
    {
        for (int i = 0; i < VirusRP.Count; i++)
        {
            VirusRP[i].SetRespawnPoint(rp);
        }
    }
}
