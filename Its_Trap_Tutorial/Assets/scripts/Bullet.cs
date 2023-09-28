using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�ǰ� �̺�Ʈ")] [SerializeField] GameObject go_RicochetEffect;
    [Header("�Ѿ� ������")] [SerializeField] int damage;
    [Header("�ǰ� ȿ����")] [SerializeField] string sound_Ricochet;

    private void OnCollisionEnter(Collision collision)
    {
        
        ContactPoint contactPoint = collision.contacts[0];
        SoundManager.instance.PlaySE(sound_Ricochet);
        var clone = Instantiate(go_RicochetEffect, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
        //GameObject clone = Instantiate(go_RicochetEffect, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
        
        if( collision.transform.CompareTag("Mine"))
        {
            collision.transform.GetComponent<Mine>().Damaged(damage);
        }
        
        Destroy(clone, 0.5f);
        Destroy(gameObject);
    }
}
