using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Score, NormalGun_Bullet
}

public class Item : MonoBehaviour
{
    public ItemType itemType;

    public int extraScore;
    public int extraBullet;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
    }
}
