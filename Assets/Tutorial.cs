using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneを使うのに必要

public class Tutorial : MonoBehaviour
{
    private bool firstPush = false;
    //チュートリアルボタンを押されると呼ばれる
    // Start is called before the first frame update
    public void PressStart()
    {
        Debug.Log("Press Start!");
        if (!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //ここに次のシーンへ行く命令を書く
            firstPush = true;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }
}
