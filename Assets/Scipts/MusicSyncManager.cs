using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct MusicFile{
    public AudioClip audioClip;
    public float bpm;
    public float startOffset;
    public bool[] beatMap;
}

public class MusicSyncManager : MonoBehaviour
{
    public delegate void OnTimeAction(float bpm);
    public static event OnTimeAction onTime; 
    AudioSource audioSource;
    [SerializeField] MusicFile[] musicFiles;

    [SerializeField] int currentMusicIndex;
    [SerializeField] float currentBpm;
    [Range(0,16)]public static int showNotePreBeats = 4;

    float currentbeatTime;
    int palyingBeatsCount;
    int displayBeatsCount;
    public float GlobalOffset;
    
    float timer;

    void Start()
    {
        audioSource =  GetComponent<AudioSource>();
        timer = 0;
        PlayMusic(0);
    }
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= currentbeatTime * displayBeatsCount){

            onTime?.Invoke(currentBpm);
            displayBeatsCount++;
            
            if(audioSource.isPlaying){
                palyingBeatsCount++;
            }
        }
    }

    public void PlayMusic(int index)
    {
        
        if(index >= musicFiles.Length || index < 0)
        {
            Debug.LogError("Index out of range");
            return;
        }
        ///Play another music and update the bpm and other info;
        timer = 0;
        displayBeatsCount = 0;
        palyingBeatsCount = 0;

        currentMusicIndex = index;
        
        audioSource.clip = musicFiles[index].audioClip;
        
        currentBpm = musicFiles[index].bpm;
        currentbeatTime = 60/currentBpm;
        audioSource.Pause();
        ////temp        
        StartCoroutine(PlayMusicAfterBeats(showNotePreBeats,musicFiles[index].bpm,musicFiles[index].startOffset));
    }
    IEnumerator PlayMusicAfterBeats(int beatsNum, float bpm,float offset){
        yield return new WaitForSeconds(beatsNum * 60/bpm);
        Debug.Log("play music");
        audioSource.time = offset;
        audioSource.Play();
        yield return null;
    }
}
