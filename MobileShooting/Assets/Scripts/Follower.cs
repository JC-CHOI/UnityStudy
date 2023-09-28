using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject followerBullet;
    public GameManager manager;

    public Vector3 followPos;
    public Transform parent;
    public int followDelay;
    public Queue<Vector3> parentPos;

    private void Awake()
    {
        
        parentPos = new Queue<Vector3>();
    }
    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();


    }
    void Watch()
    {
        if( !parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        if (followDelay < parentPos.Count)
            followPos = parentPos.Dequeue();
        else if( followDelay > parentPos.Count)
            followPos = parent.position;
    }
    void Follow()
    { 
        transform.position = followPos;
        //transform.position = Vector3.Lerp(transform.position, parent.position - followPos, speed);
    }
    void Reload()
    {
        curShotDelay += Time.deltaTime;

    }
    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;
        if (curShotDelay < maxShotDelay) // 장전속도
            return;

                GameObject bullet = Instantiate(followerBullet, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);


        curShotDelay = 0;

    }
}
