using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] float verticalDistance;
    [SerializeField] float horizontalDistance;

    [Range(0,1f)] [SerializeField] float moveSpeed;

    [SerializeField] int hp;

    [SerializeField] int damage;
    [SerializeField] GameObject go_EffectPrefab;

    Vector3 endPos1; // 첫번째 목적지
    Vector3 endPos2; // 두번째 목적지
    Vector3 currentDestination;

    private void Start()
    {
        Vector3 originPos = transform.position;
        endPos1.Set(originPos.x, originPos.y + verticalDistance, originPos.z + horizontalDistance);
        endPos2.Set(originPos.x, originPos.y - verticalDistance, originPos.z - horizontalDistance);
        currentDestination = endPos1;
    }
    private void Update()
    {
        if ((transform.position - endPos1).sqrMagnitude <= 0.1f) // 거듭제곱
            currentDestination = endPos2;
        else if ((transform.position - endPos2).sqrMagnitude <= 0.1f)
            currentDestination = endPos1;

        transform.position = Vector3.Lerp(transform.position, currentDestination, moveSpeed);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player")
        {
            collision.transform.GetComponent<StatusManager>().DecreaseHp(damage);
            Explosion();

        }
        
    }

    public void Damaged(int _num)
    {
        hp -= _num;
        if (hp <= 0)
            Explosion();
    }
    void Explosion()
    {
        SoundManager.instance.PlaySE("Mine");
        GameObject clone = Instantiate(go_EffectPrefab, transform.position, Quaternion.identity);

        Destroy(clone, 2f);
        Destroy(gameObject);
    }
}
