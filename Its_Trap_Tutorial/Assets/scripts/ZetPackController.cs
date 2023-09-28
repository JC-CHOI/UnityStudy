using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZetPackController : FollowPlayer
{
    [Header("제트 엔진 회전 속도")] [SerializeField] [Range(0, 1)] float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = tf_player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.position = Vector3.Lerp(transform.position, tf_player.position - currentPos, speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), spinSpeed);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.position = Vector3.Lerp(transform.position, tf_player.position - new Vector3(currentPos.x, currentPos.y, -currentPos.z), speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-100, 0, 0), spinSpeed);
        }
        else if(Input.GetAxisRaw("Horizontal") == 0)
        {
            transform.position = Vector3.Lerp(transform.position, tf_player.position - new Vector3(currentPos.x, currentPos.y, 0), speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-56, 0, 0), spinSpeed);
        }
    }
}
