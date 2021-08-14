using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBullet : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    [SerializeField] float MoveSpeed = 0.8f;

    [Header("”­Ë‚µ‚Ä‚©‚çíœ‚·‚éƒtƒŒ[ƒ€”")]
    [SerializeField] int DeleteFlame = 90;//”­Ë‚µ‚Ä‚©‚çíœ‚·‚éŠÔ
    private int FlameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 vec = this.transform.rotation * new Vector3(1.0f, 0.0f, 0.0f);
        this.transform.position += vec * MoveSpeed;

        if (FlameCount == DeleteFlame)
        {
            Destroy(this.gameObject);
        }
        FlameCount++;
    }

}
