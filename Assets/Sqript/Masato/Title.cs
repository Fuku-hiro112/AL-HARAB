using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g�����߂ɕK�v�I�I

public class Title : MonoBehaviour
{
    private bool firstPush = false;
    //�X�^�[�g�{�^���������ꂽ��Ă΂��
   
   public void PressStart()
    {
        Debug.Log("Press Start!");
        
        if(!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //�����Ɏ��̃V�[���֍s�����߂�����
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
