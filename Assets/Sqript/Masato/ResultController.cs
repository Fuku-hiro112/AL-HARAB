using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g���̂ɕK�v

public class ResultController : MonoBehaviour
{
    private bool firstPush = false;
    //�X�e�[�W�N���A��Q�[����ʂ������ꂽ��Ă΂��
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Press Start!");
        if(!firstPush)
        {
            Debug.Log("Go Next Scene!");
            //�����Ɏ��̃V�[���֍s�����߂�����
            firstPush = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Scene");
        }
    }
}
