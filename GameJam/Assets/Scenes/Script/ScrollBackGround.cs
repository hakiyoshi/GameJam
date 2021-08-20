using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour
{
    CharacterMovement CM;

    [Header("�X�N���[�����x")]
    [SerializeField]float ScrollSpeed;

    [Header("�X�N���[��������摜�̐�")]
    [SerializeField]int BackGroundCount;

    Renderer[] BackGround;

    float Time;

    // Start is called before the first frame update
    void Start()
    {
        CM = GameObject.Find("Player").GetComponent<CharacterMovement>();

        BackGround = new Renderer[BackGroundCount];

        for(int i = 0; i< BackGroundCount; i++)
        {
            BackGround[i] = this.transform.GetChild(i).gameObject.GetComponent<Renderer>();
            BackGround[i].material.SetFloat("_Pos", ScrollSpeed);
        }

        Time = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        if(CM.GetbRight())
            Time++;
        else if (CM.GetbLeft())
            Time--;

            for (int i = 0; i < BackGroundCount; i++)
                BackGround[i].material.SetFloat("_Pos", ScrollSpeed * Time);
    }
}
