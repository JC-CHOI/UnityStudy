using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public enum Type {  Ammo, Coin, Grenade, Heart, Weapon,};
    public Type type;
    public int value;

    Rigidbody rigid;
    SphereCollider sc;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 10 * Time.deltaTime);    
    }

    void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sc.enabled = false;
        }
    }
}
