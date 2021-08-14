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

    // Update is called once per frame
    void Update()
    {

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
        return Vector3.Normalize(player.position - Position);
    }

    //�������ԑ��t���[�����擾
    public int GetReloadFlame()
    {
        return ReloadFlame;
    }

}
