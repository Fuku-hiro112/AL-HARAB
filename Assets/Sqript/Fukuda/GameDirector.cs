using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    Image _speedGage;
    Text _timeText;
    PlayerAction _playerAction;
    float _speed;

    void Start()
    {
        FindConponent();


    }
    
    void Update()
    {
        if (_playerAction == null)
        {
            Debug.Log("nullです");
        }
        else
        {
            _speedGage.fillAmount = SpeedRate();

            //点数を表示
            _timeText.text = PlayerAction.ClearTime.ToString("F2");
        }
    }

    

    float SpeedRate()
    {
        _speed = _playerAction.SpeedGage;
        return _speed / 6;
    }

    void FindConponent()
    {
        _timeText = GameObject.Find("TimeText").GetComponent<Text>();
        _speedGage = GameObject.Find("SpeedGage").GetComponent<Image>();
        _playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
    }
}
