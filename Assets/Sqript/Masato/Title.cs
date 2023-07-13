using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneを使うために必要！！

public class Title : MonoBehaviour
{
    private bool firstPush = false;
    //スタートボタンを押されたら呼ばれる
   
   public void PressStart()
    {
        Debug.Log("Press Start!");
        
        if(!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //ここに次のシーンへ行く命令を書く
            firstPush = true;
           
        }
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("StageScene");
        }
    }
}
