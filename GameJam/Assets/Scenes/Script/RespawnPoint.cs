using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{

    [SerializeField] Transform Point;//���X�|�[���|�C���g
    

    public Vector3 GetRespawnPoint()
    {
        return Point.position;
    }

    public void SetRespawnPoint(Vector3 position)
    {
        Point.position = position;
    }
}
