using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BeatManager : MonoBehaviour
{
    public delegate void OnBeatAction(float accuracy);
    public static event OnBeatAction OnBeat;
    float curBpm = 120;
    [SerializeField]float rangeToHit = 1500;
    
    RectTransform m_rectTransform;

    public GameObject beatsPrefab;    
    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        MusicSyncManager.onTime += AddBeatNote;
        
    }

    
    void Update(){

        if(Input.GetKeyDown(KeyCode.Space)){          
            //OnBeat?.Invoke();
            //too early
            //on time
            //too late
        }
    }
    void AddBeatNote(float bpm){
        curBpm = bpm;
        
        if(beatsPrefab==null){
            Debug.LogError("beatsPrefab is null");
            return;
        }

        GameObject beatnote = Instantiate(beatsPrefab, transform.position + Vector3.right * rangeToHit, Quaternion.identity,transform);
        beatnote.GetComponent<BeatNoteBehaviour>().SetBpm(curBpm);
        return;
        
    }
}
