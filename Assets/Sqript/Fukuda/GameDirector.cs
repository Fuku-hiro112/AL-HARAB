using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    [SerializeField] private float _waitTime;
    private Image _speedGage;
    private Text _timeText;
    private PlayerAction _playerAct;
    //PlayerAction.GameMode _gameMode;
    private float _speed;
    private string _sceneName;

    private GameObject[] _tagEnemys;
    private GameObject[] _tagBarricades;

    [NonSerialized]
    public static int _enemyNum;
    [NonSerialized]
    public static byte _barricadeNum;

    [NonSerialized]
    public static byte _breakableObjNum;
    [NonSerialized]
    public static float _clearTime;

    void Start()
    {
        FindConponent();

        //óvÇÁÇ»Ç¢Ç©Ç‡
        _sceneName = SceneManager.GetActiveScene().name;
        
        _tagEnemys = GameObject.FindGameObjectsWithTag("enemy");
        _tagBarricades = GameObject.FindGameObjectsWithTag("barricade");

        _enemyNum = _tagEnemys.Length;
        _barricadeNum = (byte)_tagBarricades.Length;
        _breakableObjNum = Convert.ToByte(_enemyNum + _barricadeNum);

        PlayerPrefs.SetInt("EnemyNum",_enemyNum);
        PlayerPrefs.SetInt("BarricadeNum",_barricadeNum);
    }
    
    async void Update()
    {
        if (_playerAct == null)
        {
            Debug.Log("nullÇ≈Ç∑");
        }
        else
        {
            _speedGage.fillAmount = SpeedRate();

            //ì_êîÇï\é¶
            _timeText.text = _playerAct.ClearTime.ToString("F2");
        }

        if (_playerAct._mode == PlayerAction.GameMode.Play) return;
        if (_playerAct._mode == PlayerAction.GameMode.GameCrear)
        {
            PlayerPrefs.SetFloat("ClearTime",_playerAct.ClearTime);
            PlayerPrefs.SetInt("HpCurrent",_playerAct.HpCurrent);
            SaveBreakableObj();
        }
        else if(_playerAct._mode == PlayerAction.GameMode.GameOver)
        {
            SaveBreakableObj();
        }
        PlayerPrefs.Save();
        var ct = this.GetCancellationTokenOnDestroy();
        await AsyncLoadScene(ct,_waitTime);
    }

    void SaveBreakableObj()
    {
        PlayerPrefs.SetInt("BreakEnemyNum", _playerAct.BreakEnemyNum);
        PlayerPrefs.SetInt("BreakBarricadeNum", _playerAct.BreakBarricadeNum);
    }

    private float SpeedRate()
    {
        _speed = _playerAct.SpeedGage;
        return _speed / 6;
    }

    private void FindConponent()
    {
        _timeText = GameObject.Find("TimeText").GetComponent<Text>();
        _speedGage = GameObject.Find("SpeedGage").GetComponent<Image>();
        _playerAct = GameObject.Find("Player").GetComponent<PlayerAction>();
    }

    private void GameEnd(float clearTime)
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
    private void GameOver()
    {

    }

    private async UniTask AsyncLoadScene(CancellationToken ct,float waitTime)
    {

        await UniTask.Delay((int)(waitTime * 1000), cancellationToken: ct);

        SceneManager.LoadScene("ResultScene");
    }
}
