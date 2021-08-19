#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetPositionVirusButtery : MonoBehaviour
{
    [Header("�������̕�")]
    [SerializeField] float Length;

    [Header("��]�p�x")]
    [SerializeField] float rotation;

    public void SetPosition()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);
        Vector3 vec = this.transform.rotation * new Vector3(1.0f, 0.0f, 0.0f);
        this.transform.localPosition = vec * Length;
    }
}

[CustomEditor(typeof(SetPositionVirusButtery))]
public class SetCustom : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var set = target as SetPositionVirusButtery;
        if (GUILayout.Button("���W�ύX"))
        {
            set.SetPosition();
        }
    }
}

#endif
