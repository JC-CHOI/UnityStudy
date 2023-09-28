using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("���� �÷��̾� ����")] [SerializeField] Transform tf_player;
    [Header("���� ���ǵ� ����")] [Range(0, 1f)] [SerializeField] float chaseSpeed;

    float camNormalXPos;

    [SerializeField] [Header("�ν��ͽ� ������ x�Ÿ�")]
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

        Vector3 movePos = Vector3.Lerp(transform.position, tf_player.position, chaseSpeed); // (��, Ÿ��, ���ǵ�);
        float cameraPosX = Mathf.Lerp(transform.position.x, camCurrentXPos, chaseSpeed); // Mathf.Lerp - float ��
        transform.position = new Vector3(cameraPosX, movePos.y, movePos.z);
    }
}
