using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;
    public bool isRotate;

    private void Update()
    {
        if (isRotate)
            transform.Rotate(Vector3.forward * 2 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        }
    }
}
