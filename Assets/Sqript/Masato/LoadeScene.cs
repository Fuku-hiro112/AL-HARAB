using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
   
    
    public void Stage1()
    {

        SceneManager.LoadScene("GameScene1");

    }
    public void Stage2()
    {

        SceneManager.LoadScene("GameScene2");

    }
    public void Stage3()
    {

        SceneManager.LoadScene("GameScene3");

    }
    public void Tutorial()
    {

        SceneManager.LoadScene("TutorialScene");

    }
}
