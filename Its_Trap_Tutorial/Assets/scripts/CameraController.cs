using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("따라갈 플레이어 설정")] [SerializeField] Transform tf_player;
    [Header("따라갈 스피드 조정")] [Range(0, 1f)] [SerializeField] float chaseSpeed;

    float camNormalXPos;

    [SerializeField] [Header("부스터시 떨어질 x거리")]
    float camJetXPos;
    float camCurrentXPos;

    PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = tf_player.GetComponent<PlayerController>();

        camNormalXPos = transform.position.x;
        camCurrentXPos = camNormalXPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (thePlayer.IsJet)
            camCurrentXPos = camJetXPos;
        else
            camCurrentXPos = camNormalXPos;

        Vector3 movePos = Vector3.Lerp(transform.position, tf_player.position, chaseSpeed); // (나, 타겟, 스피드);
        float cameraPosX = Mathf.Lerp(transform.position.x, camCurrentXPos, chaseSpeed); // Mathf.Lerp - float 형
        transform.position = new Vector3(cameraPosX, movePos.y, movePos.z);
    }
}
