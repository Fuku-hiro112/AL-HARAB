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
        // for文（繰り返し文）・・・まずは基本形を覚えましょう！
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
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            //ここに何かしらの処理
            //例　ボーンについてる武器を取得する
            //if (ob.name == "Right Hand")
            //  {
            //      rightHandWeapon = ob.transform.GetChild(0).gameObject;
            //   }
            GetChildren(ob.gameObject);
        }*/

    }
}
