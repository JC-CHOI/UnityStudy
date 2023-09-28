using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;

    public float speed;

    public int health;

    public Sprite[] sprites;

    public int enemyScore;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;


    public GameObject bulletObjA;
    public GameObject bulletObjB;

    public GameObject ItemCoin;
    public GameObject ItemBoom;
    public GameObject ItemPower;
        
    public GameObject player;

    public float maxShotDelay;
    public float curShotDelay;

    int bossShotSeq;

    Animator anim;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();                
        if (enemyName == "B")            
        {
            anim = GetComponent<Animator>();
        }
    }
    void Update()
    {
        if (enemyName == "B")
        {

            transform.position = new Vector3(0, 3.5f, 0);
            rigid.velocity = Vector2.zero;
            BossShot();
            return;
        }
        else
        {
            Fire();
            
        }
        Reload();
    }
    void BossShot()
    {
        bossShotSeq = bossShotSeq == 3 ? 0 : bossShotSeq + 1;


        switch (bossShotSeq)
        {
            case 0:
                BossShot_0();
                break;
            case 1:
                BossShot_1();
                break;
            case 2:
                BossShot_2();
                break;
            case 3:
                BossShot_3();
                break;
        }
    }
    void BossShot_0()
    {
        Debug.Log("boss shot 0");        
    }
    void BossShot_1()
    {
        Debug.Log("boss shot 1");
    }
    void BossShot_2()
    {
        Debug.Log("boss shot 2");
    }
    void BossShot_3()
    {
        Debug.Log("boss shot 3");
    }
    void Reload()
    {
        curShotDelay += Time.deltaTime;

    }
    void Fire()
    {
        if (curShotDelay < maxShotDelay) // 장전속도
            return;
        if( enemyName == "S")
        {
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;

            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if(enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletObjB, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bulletObjB, transform.position + Vector3.left * 0.3f, transform.rotation);
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidR.AddForce(dirVecR.normalized * 5, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 5, ForceMode2D.Impulse);
        }
            curShotDelay = 0;

    }
    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        if (enemyName == "B")
            anim.SetTrigger("OnHit");
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        if ( health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            float ran =  enemyName == "B" ? 0 : Random.Range(0, 10f);
            if (ran < 5)
                Debug.Log("not item");
            else if( ran < 7)
            {
                GameObject clone = Instantiate(ItemCoin, transform.position, ItemCoin.transform.rotation);
                clone.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.down;
            }
            else if( ran < 8)
            {
                GameObject clone = Instantiate(ItemBoom, transform.position, ItemBoom.transform.rotation);
                clone.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.down;
            }
            else if( ran < 9)
            {
                GameObject clone = Instantiate(ItemPower, transform.position, ItemPower.transform.rotation);
                clone.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.down;
            }
            Destroy(gameObject);
        }
    }
    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag == "BorderBullet" && enemyName != "B")
        {
            Destroy(gameObject);
        }
        else if( collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            Destroy(collision.gameObject);
        }
        
    }
}
