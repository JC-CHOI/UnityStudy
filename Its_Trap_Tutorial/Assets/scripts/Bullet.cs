using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("피격 이벤트")] [SerializeField] GameObject go_RicochetEffect;
    [Header("총알 데미지")] [SerializeField] int damage;
    [Header("피격 효과음")] [SerializeField] string sound_Ricochet;

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
