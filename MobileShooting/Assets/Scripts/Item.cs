using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    Coin, Power, Boom,
}
public class Item : MonoBehaviour
{    
    public itemType it;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * 1.2f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
