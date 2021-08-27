using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAnimation : MonoBehaviour
{
    private float MoveSpeed = 0.0f;

    bool IsOpen = false;//true開く　falseとじる
    public bool IsStay { get; set; }//待機フラグ


    float EndScale = 0.0f;

    private void Start()
    {
        this.transform.localScale = new Vector3(0.0f, this.transform.localScale.y, this.transform.localScale.z);
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (IsStay)
            return;

        if (IsOpen)
        {
            this.transform.localScale += new Vector3(MoveSpeed, 0.0f, 0.0f);

            if (this.transform.localScale.x >= EndScale)
            {
                this.transform.localScale = new Vector3(EndScale, this.transform.localScale.y, this.transform.localScale.z);
                IsStay = true;
            }
        }
        else
        {
            this.transform.localScale -= new Vector3(MoveSpeed, 0.0f, 0.0f);

            if (this.transform.localScale.x <= EndScale)
            {
                this.transform.localScale = new Vector3(0.0f, this.transform.localScale.y, this.transform.localScale.z);
                IsStay = true;
            }
        }
    }


    public void Open(float speed, float startscale = 0.5f, float endscale = 1.0f)
    {
        if (IsOpen)
        {
            return;
        }

        IsStay = false;
        IsOpen = true;

        EndScale = endscale;
        MoveSpeed = speed;
        this.transform.localScale = new Vector3(startscale, this.transform.localScale.y, this.transform.localScale.z);
    }

    public void Close(float speed, float endscale = 0.0f)
    {
        if (!IsOpen)
        {
            return;
        }

        IsStay = false;
        IsOpen = false;

        EndScale = endscale;
        MoveSpeed = speed;
        this.transform.localScale = new Vector3(1.0f, this.transform.localScale.y, this.transform.localScale.z);
    }
}
