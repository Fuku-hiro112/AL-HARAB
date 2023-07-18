using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g���̂ɕK�v

public class Tutorial : MonoBehaviour
{
    private bool firstPush = false;
    //�`���[�g���A���{�^�����������ƌĂ΂��
    // Start is called before the first frame update
    public void PressStart()
    {
        Debug.Log("Press Start!");
        if (!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //�����Ɏ��̃V�[���֍s�����߂�����
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
