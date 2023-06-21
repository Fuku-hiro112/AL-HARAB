using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    Image speedGage;
    PlayerAction playerAction;
    float speed;

    void Start()
    {
        speedGage = GameObject.Find("SpeedGage").GetComponent<Image>();
        playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
        //playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
    }

    
    void Update()
    {
        speed = playerAction.speed_elapsed_time;
        //Debug.Log(speed);
        speedGage.fillAmount = Speed_rate();
    }

    float Speed_rate()
    {
        float rate;
        rate = speed/6;
        return rate;
    }
}
