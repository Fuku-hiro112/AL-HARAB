using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthPanel : MonoBehaviour
{
    Image[] _imagHealth;
    PlayerAction _playerAction;
    int hp_current;
    
    void Start()
    {
        _playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
        GetChildren(this.gameObject);
    }

    void Update()
    {

    }
    void UpdateHeart()
    {
        // for���i�J��Ԃ����j�E�E�E�܂��͊�{�`���o���܂��傤�I
        /*for (int i = 0; i < Hearts.Length; i++)
        {
            if (destroyCount <= i)
            {
                playerIcons[i].SetActive(true);
            }
            else
            {
                playerIcons[i].SetActive(false);
            }
        }*/
    }
    void GetChildren(GameObject obj)
    {
        /*_imagHealth = obj.GetComponentInChildren<Image>();
        //�q�v�f�����Ȃ���ΏI��
        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            //�����ɉ�������̏���
            //��@�{�[���ɂ��Ă镐����擾����
            //if (ob.name == "Right Hand")
            //  {
            //      rightHandWeapon = ob.transform.GetChild(0).gameObject;
            //   }
            GetChildren(ob.gameObject);
        }*/

    }
}
