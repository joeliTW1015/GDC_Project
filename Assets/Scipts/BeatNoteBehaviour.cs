using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
public class BeatNoteBehaviour : MonoBehaviour
{
    public int LeftBeats;
    Vector3 m_startPos;
    float m_curBpm =120;
    float speed;
    public void SetBpm(float bpm){
        m_curBpm = bpm;
    }
    
    void Start(){
        Tween moveToIndicator = transform.DOMoveX(transform.parent.position.x, 60/m_curBpm * MusicSyncManager.showNotePreBeats).SetEase(Ease.Linear);
        moveToIndicator.OnComplete(DestoryBeatNote);
    }
    public void DestoryBeatNote()
    {
        //animation
        //not finish yet
        Destroy(gameObject);
    }
}
