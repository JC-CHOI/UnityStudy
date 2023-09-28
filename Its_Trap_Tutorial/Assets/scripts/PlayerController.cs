using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static bool canMove = true;

    [Header("이동속도 조절")][SerializeField] float moveSpeed;
    [Header("점프세기 조절")][SerializeField] float jetPackSpeed;
    Rigidbody myRigid;
    [Header("파티클 시스템(부스터")]
    [SerializeField] ParticleSystem ps_LeftEngine;
    [SerializeField] ParticleSystem ps_RightEngine;

    AudioSource audioSource;

    JetEngineFuelManager theFuel;

    public bool IsJet { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        IsJet = false;
        myRigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        theFuel = FindObjectOfType<JetEngineFuelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
        TryJet();
        
    }
    void TryMove()
    {
        Debug.Log("1");
        if (Input.GetAxisRaw("Horizontal") != 0 && canMove)
        {
            Vector3 moveDir = new Vector3(0, 0, Input.GetAxisRaw("Horizontal"));
            myRigid.AddForce(moveDir * moveSpeed);
        }
    }
    void TryJet()
    {
        //if( Input.GetKey(KeyCode.Space) && theFuel.IsFuel )
            if (Input.GetKey(KeyCode.Space) && theFuel.IsFuel && canMove)
            {
            if( !IsJet )
            {
                ps_LeftEngine.Play();
                ps_RightEngine.Play();
                audioSource.Play();
                IsJet = true;
            }
            
            myRigid.AddForce(Vector3.up * jetPackSpeed);
        }
        else
        {
            if (IsJet)
            {
                ps_LeftEngine.Stop();
                ps_RightEngine.Stop();
                audioSource.Stop();
                IsJet = false;
            }
            myRigid.AddForce(Vector3.down * jetPackSpeed);
        }
    }
}
