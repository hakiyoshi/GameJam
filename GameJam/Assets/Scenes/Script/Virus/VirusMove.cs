using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusMove : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    [SerializeField] float MoveSpeed;

    private void FixedUpdate()
    {
        this.transform.position += new Vector3(1.0f, 0.0f, 0.0f) * MoveSpeed;
    }
}
