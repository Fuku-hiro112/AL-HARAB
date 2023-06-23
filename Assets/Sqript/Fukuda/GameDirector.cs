using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    Image speedGage;
    Text timeText;
    PlayerAction playerAction;
    float speed;

    void Start()
    {
        FindConponent();


    }
    
    void Update()
    {
        speed = playerAction.speed_elapsed_time;

        speedGage.fillAmount = Speed_rate();

        //ì_êîÇï\é¶
        timeText.text = PlayerAction.clear_time.ToString("F2");
    }

    

    float Speed_rate()
    {
        float rate;
        rate = speed/5;
        return rate;
    }

    void FindConponent()
    {
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        speedGage = GameObject.Find("SpeedGage").GetComponent<Image>();
        playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
    }
}
