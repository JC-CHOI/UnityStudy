using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int grenade;
    public Camera followCamera;
    public GameObject grenadeObj;
    public GameManager manager;

    public int ammo;
    public int coin;
    public int health;
    public int score;

    public int hasGrenade;
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenade;

    float hAxis;
    float vAxis;
    
    bool wDown;
    bool jDown;
    bool iDown;
    bool fDown;
    bool gDown;
    bool rDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder;
    bool isDamage;
    bool isShop;
    bool isDead;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;
    Rigidbody rigid;
    MeshRenderer[] meshs;

    GameObject nearObject;
    public Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
                
        PlayerPrefs.SetInt("MaxScore",112500);

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Grenade();
        Dodge();
        Swap();
        Attack();
        Reload();
        Interation();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButton("Fire1");
        gDown = Input.GetButtonDown("Fire2");
        iDown = Input.GetButtonDown("Interation");
        rDown = Input.GetButtonDown("Reload");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        if (isSwap || !isFireReady || isReload || isDead)
            moveVec = Vector3.zero;
        if(!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }
    void Turn()
    {
        transform.LookAt(transform.position + moveVec);

        if (fDown && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextVec = rayhit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }
    void Grenade()
    {
        if (hasGrenade == 0)
            return;
        if( gDown && !isReload && !isSwap && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextVec = rayhit.point - transform.position;
                nextVec.y = 10;

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec + Vector3.forward * 10, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasGrenade--;
                grenades[hasGrenade].SetActive(false);

            }
        }
    }
    void Attack()
    {
        if (equipWeapon == null)
            return;
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if( fDown && isFireReady && !isDodge && !isSwap && !isShop)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
        
    }
    void Reload()
    {
        if (equipWeapon == null)
            return;
        if (equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;

        //if( rDown  && !isJump && !isDodge && !isSwap && !isFireReady  )
        if (rDown && !isJump && !isDodge && !isSwap && !isDead)
            {
            anim.SetTrigger("doReload");
            isReload = true;

            Invoke("ReloadOut", 1.5f);

        }
    }
    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }
    void Dodge()
    {
        if( jDown && moveVec != Vector3.zero &&!isJump && !isDodge && !isSwap && !isDead)
        {
            dodgeVec = moveVec;
            speed *= 2;            
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    void Swap()
    {
        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;
        
        if ( (sDown1 || sDown2 || sDown3) && !isJump && !isDodge && hasWeapons[weaponIndex] == true && weaponIndex != equipWeaponIndex && !isDead)
        {
            if( equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;
            Invoke("SwapOut", 0.5f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }
    void Interation()
    {
        if( iDown && nearObject!= null && !isJump && !isDodge && !isDead)
        {
            if( nearObject.tag == "Weapon")
            {
                Items item = nearObject.GetComponent<Items>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
            else if( nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                isShop = true;
            }
        }
    }
    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap && !isDead)
        {
            rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, moveVec, 5, LayerMask.GetMask("Wall"));

    }
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;            
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Item")
        {
            Items item = other.GetComponent<Items>();
            switch( item.type)
            {
                case Items.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Items.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Items.Type.Heart:
                    health += item.value;
                    if (health> maxHealth)
                        health = maxHealth;
                    break;
                case Items.Type.Grenade:
                    if (hasGrenade >= maxHasGrenade)
                        return;

                    hasGrenade += item.value;
                    if (hasGrenade >= maxHasGrenade)
                        hasGrenade = maxHasGrenade;
                    grenades[hasGrenade-1].SetActive(true);                              
                    
                    break;
            }
            Destroy(other.gameObject);
        }
        else if ( other.tag == "EnemyBullet")
        {
            if( !isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                bool isBossAtk = other.name == "Boss Melee Area";

                StartCoroutine(OnDamage(isBossAtk));
            }
            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);
        }
    }
    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach( MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }

        if (isBossAtk)
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
       
        if (health <= 0 && !isDead)
            OnDie();

        yield return new WaitForSeconds(0.5f) ;

        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        if (isBossAtk)
            rigid.velocity = Vector3.zero;        
    }
    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();

    }
    void OnTriggerStay(Collider other)
    {
        if( other.tag == "Weapon" || other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
        else if( other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            isShop = false;
            nearObject = null;
        }
    }
}
