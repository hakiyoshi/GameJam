using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] Vector3 Point;//���X�|�[���|�C���g


    public Vector3 GetRespawnPoint()
    {
        return Point;
    }
}
