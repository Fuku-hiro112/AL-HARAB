using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPanel : MonoBehaviour
{
    [SerializeField] GameObject[] playerIcons;

    int hp;

    void Start()
    {
        
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

}
