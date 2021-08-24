using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1IventManager : IventManager
{
    [Header("始まってから流すBGM")]
    [SerializeField] PlaySound StartSound;


    private AudioController audiocon;
    private void Start()
    {
        audiocon = PlayAudio(StartSound);
    }

    IEnumerator CheckClear()
    {
        yield break;
    }
}
