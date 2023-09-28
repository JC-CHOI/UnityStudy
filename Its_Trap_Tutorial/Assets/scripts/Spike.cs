using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] int damage;

    [SerializeField] float force;

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.transform.CompareTag("Player"))
        {
            Debug.Log(damage + " �� �������ϴ�.");

            collision.transform.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, 5f);
            collision.transform.GetComponent<StatusManager>().DecreaseHp(damage);
        }
    }
}
