using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBulletManager : MonoBehaviour
{
    [Header("�e�v���n�u")]
    [SerializeField] GameObject BulletPrefab;

    [Header("�C�䃊���[�h�t���[����")]
    [SerializeField] int ReloadFlame = 20;

    //�e���
    private GameObject[] bullet;

    //�v���C���[���W
    private Transform player;

    //�e���˃N�[���^�C��
    public const int COOLTIME = 1;//��񌂂�����COOLTIME�񐔕������Ȃ�
    private int CountCoolTime = 0;//�N�[���^�C���J�E���g�p

    // Start is called before the first frame update
    void Start()
    {
        //�e���擾
        bullet = new GameObject[this.transform.childCount];
        for (int i = 0; i < bullet.Length; i++)
        {
            bullet[i] = this.transform.GetChild(i).gameObject;
        }

        //�v���C���[���W�󂯎��
        player = GameObject.Find("Player").transform;
    }

    public GameObject CreateBullet(GameObject obje)
    {
        GameObject newobj = Instantiate(BulletPrefab);
        newobj.transform.position = obje.transform.position;
        newobj.transform.rotation = obje.transform.rotation;
        return newobj;
    }

    //�v���C���[�ւ̕����x�N�g���@���K����
    public Vector3 CalPlayerVec(Vector3 Position)
    {
        return Vector3.Normalize(Quaternion.AngleAxis(30.0f, new Vector3(0.0f, 0.0f, 1.0f)) * new Vector3(1.0f, 0.0f, 0.0f));
    }

    //�������ԑ��t���[�����擾
    public int GetReloadFlame()
    {
        return ReloadFlame;
    }

    //�w�肵���^�C�������Z
    public void AddCoolTime(int addtime)
    {
        CountCoolTime += addtime;
    }

    //�Q�b�^�[
    public int GetCoolTime()
    {
        return CountCoolTime;
    }

    //�Z�b�^�[
    public void SetCoolTime(int settime)
    {
        CountCoolTime = settime;
    }

    //�w�肵��CoolTime�ȏ�ɂȂ��Ă�����true
    public bool GetIfCoolTime(int CoolTime = COOLTIME)
    {
        return CountCoolTime >= CoolTime ? true : false;
    }
}
