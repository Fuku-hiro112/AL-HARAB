using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g���̂ɕK�v

public class Stage3 : MonoBehaviour
{
    public bool firstPush = false;
    // Start is called before the first frame update
    public void Start()
    {
        if (!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //�����Ɏ��̃V�[���֍s�����߂�����
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
