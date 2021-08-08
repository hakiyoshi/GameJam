using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStartManager : MonoBehaviour
{
    [SerializeField] string AudioName;
    [SerializeField] bool loop;
    [SerializeField] bool fade;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.PlayAudio(AudioName, loop, fade);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
