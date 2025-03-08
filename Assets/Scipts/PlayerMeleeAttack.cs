using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    BoxCollider2D atkHitbox;
    [SerializeField] float atkCD;
    [SerializeField] float atkCDMultiplier; //multiple cd when timing is miss
    [SerializeField] float atkDuration;
    float timer;
    [SerializeField] float atkPower;
    [SerializeField] float atkMissMultiplier; 
    public float totalAtkPower;
    public bool ableAtk;
    [SerializeField] GameObject atkZone;

    

    private void Awake()
    {
        atkHitbox = atkZone.GetComponentInChildren<BoxCollider2D>();
        
    }

    void Start()
    {
        atkHitbox.enabled = false;
        ableAtk = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0 && ableAtk)
        {
            if(Input.GetMouseButtonDown(0)) //left button
            {
                //timer = MusicSyncManager.onTime ? atkCD : atkCDMultiplier * atkCD;
                //totalAtkPower = MusicSyncManager.onTime ? atkPower : atkPower * atkMissMultiplier;
                StartCoroutine(AtkZoneEnable());
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    IEnumerator AtkZoneEnable()
    {
        atkHitbox.enabled = true;
        //debug
        Color org = atkZone.GetComponent<SpriteRenderer>().color;
        atkZone.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(atkDuration);
        atkZone.GetComponent<SpriteRenderer>().color = org;
        atkHitbox.enabled = false;
    }
}
