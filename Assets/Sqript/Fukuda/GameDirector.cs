using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    private Image _speedGage;
    private Text _timeText;
    private PlayerAction _playerAction;
    private float _speed;
    private string _sceneName;

    private GameObject[] _tagEnemys;
    private GameObject[] _tagBarricades;

    private byte _enemyNum;
    private byte _barricadeNum;

    public byte _breakableObjNum;

    void Start()
    {
        FindConponent();

        _sceneName = SceneManager.GetActiveScene().name;

        _tagEnemys = GameObject.FindGameObjectsWithTag("enemy");
        _tagBarricades = GameObject.FindGameObjectsWithTag("barricade");
        _enemyNum = (byte)_tagEnemys.Length;
        _barricadeNum = (byte)_tagBarricades.Length;
        _breakableObjNum = Convert.ToByte(_enemyNum + _barricadeNum);
    }
    
    void Update()
    {
        if (_playerAction == null)
        {
            Debug.Log("nullÇ≈Ç∑");
        }
        else
        {
            _speedGage.fillAmount = SpeedRate();

            //ì_êîÇï\é¶
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

    private void GameEnd()
    {
        switch (_sceneName)
        {
            case "GameScene1":

                break;
            case "GameScene2":
                break;
            case "GameScene3":
                break;
            default:
                
                break;
        }
    }
}
