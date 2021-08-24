using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1IventManager : IventManager
{
    [Header("Žn‚Ü‚Á‚Ä‚©‚ç—¬‚·BGM")]
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
