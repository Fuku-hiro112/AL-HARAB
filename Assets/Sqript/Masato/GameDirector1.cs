using UnityEngine;
using UnityEngine.UI;

public class GameDirector1 : MonoBehaviour
{
    Image speedGage;
    Text timeText;
    PlayerAction playerAction1;
    float speed;

    void Start()
    {
        FindConponent();


    }
    
    void Update()
    {
        speed = playerAction1.speed_elapsed_time;
        //Debug.Log(speed);
        speedGage.fillAmount = Speed_rate();


        //ì_êîÇï\é¶
        //timeText.text = PlayerAction.time.ToString("F2");
    }

    

    float Speed_rate()
    {
        float rate;
        float v = speed / 6;
        rate = v;
        return rate;
    }

    void FindConponent()
    {
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        speedGage = GameObject.Find("SpeedGage").GetComponent<Image>();
        playerAction1 = GameObject.Find("Player").GetComponent<PlayerAction>();
    }
}
