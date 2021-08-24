using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IventManager : MonoBehaviour
{
    [System.Serializable]
    public struct PlaySound
    {
        public string name;
        public bool loop;
        public bool fade;
    }
}
