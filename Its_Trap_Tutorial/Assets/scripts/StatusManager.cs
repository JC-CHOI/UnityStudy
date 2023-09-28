using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatusManager : MonoBehaviour
{
    [SerializeField] int maxHp;
    int currentHp;

    [SerializeField] Text[] txt_HpArray;

    [SerializeField] float blinkSpeed;
    [SerializeField] int blinkCount;

    [SerializeField] MeshRenderer mesh_PLayerGraphics;

    bool isInvincibleMode = false;

    private void Start()
    {        
        currentHp = maxHp;
        UpdateHpStatus();
    }
    public void DecreaseHp(int _num)
    {
        if( !isInvincibleMode)
        {
            currentHp -= _num;
            if (currentHp <= 0)
                PlayerDead();
            UpdateHpStatus();

            SoundManager.instance.PlaySE("Hurt");
            StartCoroutine(BlinkCoroutine());
        }
    }
    public void IncreaseHp(int _num)
    {
        if (currentHp == maxHp)
            return;
        
        currentHp += _num;

        if (currentHp > maxHp)
            currentHp = maxHp;

        UpdateHpStatus();


    }
    IEnumerator BlinkCoroutine()
    {
        isInvincibleMode = true;
        for( int i=0; i<blinkCount*2; i++)
        {
            mesh_PLayerGraphics.enabled = !mesh_PLayerGraphics.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }
        isInvincibleMode = false;
    }

    void UpdateHpStatus()
    {
        for( int i=0; i<txt_HpArray.Length; i++)
        {
            if (i < currentHp)
                txt_HpArray[i].gameObject.SetActive(true);
            else
                txt_HpArray[i].gameObject.SetActive(false);
        }
    }
    void PlayerDead()
    {
        SceneManager.LoadScene("Title");
    }

}
