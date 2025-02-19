using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MusicSyncManager.onTime)
        {
            spriteRenderer.color = Color.red;

        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
