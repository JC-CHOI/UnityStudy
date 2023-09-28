using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{

    [SerializeField] Gun[] guns; // 총이 여러개 일 수도 있으니까

    const int NORMAL_GUN = 0;

    GunController theGC;

    private void Start()
    {
        theGC = FindObjectOfType<GunController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if( other.transform.CompareTag("Item"))        
        {
            Item item = other.GetComponent<Item>();

            int extra = 0;

            if( item.itemType == ItemType.Score)
            {
                SoundManager.instance.PlaySE("Score");
                extra = item.extraScore;
                ScoreManager.extraScore += extra;
            }
            else if (item.itemType == ItemType.NormalGun_Bullet)
            {
                SoundManager.instance.PlaySE("Bullet");
                extra = item.extraBullet;
                guns[NORMAL_GUN].bulletCount += extra;
                theGC.BulletUiSetting();
            }
            string message = "+" + extra;
            FloatingTextManager.instance.CreateFloatingText(other.transform.position, message);

            Destroy(other.gameObject);
        }
    }
}
