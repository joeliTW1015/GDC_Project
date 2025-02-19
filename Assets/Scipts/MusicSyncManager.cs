using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSyncManager : MonoBehaviour
{
    public static bool onTime; 
    AudioSource audioSource;
    [SerializeField] float bpm;
    [SerializeField] float maxError;
    [SerializeField] float offset;

    public float timer;
    public float beatTime;
    public float preBeatTime;
     // Start is called before the first frame update
    void Start()
    {
        audioSource =  GetComponent<AudioSource>();
        timer = 0;
        beatTime = offset;
        preBeatTime = -999;
    }

    // Update is called once per frame
    void Update()
    {
        timer = audioSource.time + offset;
        if(timer >= beatTime)
        {
            preBeatTime = beatTime;
            beatTime += 60/bpm;
        }

        if(Mathf.Abs(timer - beatTime) < maxError || Mathf.Abs(timer - preBeatTime) < maxError)  
        {
            onTime = true;
        }
        else
        {
            onTime = false;
        }
    }
}
