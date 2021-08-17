using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VirusMove : MonoBehaviour
{
    [Header("カメラ座標とのずれ")]
    [SerializeField] Vector3 CameraShift;

    private Transform camera;

    [Header("移動フラグ")]
    [SerializeField] bool MoveFlag;

    private void Start()
    {
        camera = Camera.main.transform;
    }

    private void Update()
    {

        Vector3 posi = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);
        this.transform.position = posi + CameraShift;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            Vector3 posi = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);
            this.transform.position = posi + CameraShift;
        }
    }

#endif
}
