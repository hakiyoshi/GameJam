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

    // Start is called before the first frame update
    void Start()
    {
        VirusObject.SetActive(false);//非アクティブにする
        change = Camera.main.GetComponent<ChangeCamera>();

        VirusMove = VirusObject.GetComponent<VirusMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IfActive(ACTIVE.NONE) && change.IfCameraFlag(ChangeCamera.CAMERAFLAG.TARGET))//敵出現
        {
            activeflag = ACTIVE.ACTIVE;
            VirusObject.SetActive(true);
            VirusMove.StartActive();
        }
        else if (IfActive(ACTIVE.FORCED))//強制移動開始
        {
            change.StartDollyCart();
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
}
