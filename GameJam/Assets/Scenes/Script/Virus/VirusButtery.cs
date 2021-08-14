using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusButtery : MonoBehaviour
{
    //�e�}�l�[�W���[
    private VirusBulletManager bulletmana;

    //�X�v���C�g�����_���[
    private SpriteRenderer sr;

    //�e�����߂��Ă��邩
    private bool beginbullet = true;

    //�t���[���J�E���g
    private int FlameCount = 0;

    //�p�x�͈̔͌덷
    private const float ERRORRANGE = 1.0f;

    //�e�̏����ʒu����
    private const float STARTRANGE = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        //�e�}�l�[�W���[
        bulletmana = this.transform.parent.GetComponent<VirusBulletManager>();

        //�X�v���C�g�����_���[
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //�e�����˂��Ă��Ȃ���ԂŃv���C���[�̈ʒu�Ǝ����̌����̂Ȃ��p���͈͓��̎�
        if (beginbullet && Mathf.Abs(CalPlayerWithThisAngle()) <= ERRORRANGE)//�͈͓��̎�
        {
            ShotBullet();
        }
    }

    private void FixedUpdate()
    {
        //�e�����߂��ĂȂ��ꍇ
        if (!beginbullet)
        {
            //���B�n�_�v�Z
            Vector3 end = (this.transform.localRotation * new Vector3(1.0f, 0.0f, 0.0f)) * STARTRANGE;
            this.transform.localPosition = Move(Vector3.zero, end, (float)FlameCount / (float)bulletmana.GetReloadFlame());

            FlameCount++;

            if (FlameCount == bulletmana.GetReloadFlame())
            {
                ReloadBullet(end);
            }
        }
    }

    //�e����
    void ShotBullet()
    {
        bulletmana.CreateBullet(this.gameObject);
        beginbullet = false;
        this.transform.localPosition = Vector3.zero;
    }

    //�����[�h
    void ReloadBullet(Vector3 end)
    {
        beginbullet = true;
        FlameCount = 0;
        this.transform.localPosition = end;
    }

    //�v���C���[�Ǝ����̃x�N�g���̂Ȃ��p�����߂�
    float CalPlayerWithThisAngle()
    {
        return Vector3.Angle(
            GetForward(this.transform.rotation),
            bulletmana.CalPlayerVec(this.transform.position));
    }

    //���ʃx�N�g��
    public Vector3 GetForward(Quaternion Rotation)
    {
        return Rotation * new Vector3(1.0f, 0.0f, 0.0f);
    }

    //�ړ�
    Vector3 Move(Vector3 start, Vector3 end, float time)
    {
        return start + ((end - start) * time);
    }
}
