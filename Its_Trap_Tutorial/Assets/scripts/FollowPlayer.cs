using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("���� ��� ����")][SerializeField] protected Transform tf_player;
    [Header("���� �ӵ� ����")] [SerializeField] [Range(0, 1)] protected float speed;

    protected Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = tf_player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Lerp - ���� 
        transform.position = Vector3.Lerp(transform.position, tf_player.position - currentPos, speed);
    }
}
