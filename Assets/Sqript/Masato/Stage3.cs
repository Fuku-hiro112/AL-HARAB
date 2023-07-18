using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadSceneを使うのに必要

public class Stage3 : MonoBehaviour
{
    public bool firstPush = false;
    // Start is called before the first frame update
    public void Start()
    {
        if (!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //ここに次のシーンへ行く命令を書く
            firstPush = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("GameScene2");
        }
    }
}
