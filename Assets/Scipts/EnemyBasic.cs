using System.Collections;
using UnityEngine;

public class EnemyBasic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float maxHp;
    [SerializeField] float hitCD;
    [SerializeField] float knockBackPower;
    Bar hpBar;
    PlayerMeleeAttack playerMeleeAttack;
    bool ableToGetHit;
    float hp;
    Rigidbody2D rb;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        hpBar = GetComponentInChildren<Bar>();
        playerMeleeAttack = FindFirstObjectByType<PlayerMeleeAttack>();
    }
    void Start()
    {
        hp = maxHp;
        hpBar.UpdateBar(maxHp, hp);
        ableToGetHit = true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(ableToGetHit && other.CompareTag("PlayerDamage"))
        {
            StartCoroutine(HitCDStart());
            hp -= playerMeleeAttack.totalAtkPower;
            hpBar.UpdateBar(maxHp, hp);
            if(MusicSyncManager.onTime)
            {
                Vector2 knockBackDir;
                if(other.transform.position.x > transform.position.x)
                {
                    knockBackDir = new Vector2(-1, 1);
                }
                else
                {
                    knockBackDir = new Vector2(1, 1);
                }
                rb.AddForce(knockBackDir * knockBackPower, ForceMode2D.Impulse);
            }
            

            if(hp <= 0)
            {
                EnemyDie();
            }
        }
    }

    private void EnemyDie()
    {
        Destroy(this.gameObject);
    }

    IEnumerator HitCDStart()
    {
        ableToGetHit = false;
        yield return new WaitForSeconds(hitCD);
        ableToGetHit = true;
    }

    
}
