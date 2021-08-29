using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusActiveManager : MonoBehaviour
{
    private ChangeCamera change;//�����ړ����Ă邩�m�F

    //�t���O
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
        VirusObject.SetActive(false);//��A�N�e�B�u�ɂ���
        change = Camera.main.GetComponent<ChangeCamera>();

        VirusMove = VirusObject.GetComponent<VirusMove>();

        StartCoroutine("CheckChange");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckChange()
    {
        while (true)
        {
            if (IfActive(ACTIVE.NONE) && change.IfCameraFlag(ChangeCamera.CAMERAFLAG.TARGET))//�G�o��
            {
                activeflag = ACTIVE.ACTIVE;
                VirusObject.SetActive(true);
                VirusMove.StartActive();
            }
            else if (IfActive(ACTIVE.FORCED) && change.IfCameraFlag(ChangeCamera.CAMERAFLAG.TARGET))//�����ړ��J�n
            {
                change.StartDollyCart();//�����ړ��J�n
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

    //�A�N�e�B�u�t���O��r
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
