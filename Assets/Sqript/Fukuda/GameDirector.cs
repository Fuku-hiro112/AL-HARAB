<<<<<<< HEAD
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
=======
>>>>>>> parent of e473dd6 (aaa)
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private float _waitTime;
    private Image _speedGage;
    private Text _timeText;
    private PlayerAction _playerAct;
    private float _speed;

    private bool _isSaveLoad = true;

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
=======
    Image _speedGage;
    Text _timeText;
    PlayerAction _playerAction;
    float _speed;
>>>>>>> parent of e473dd6 (aaa)

    void Start()
    {
        FindConponent();
<<<<<<< HEAD
        
        _tagEnemys = GameObject.FindGameObjectsWithTag("enemy");
        _tagBarricades = GameObject.FindGameObjectsWithTag("barricade");

        _enemyNum = _tagEnemys.Length;
        _barricadeNum = (byte)_tagBarricades.Length;
        _breakableObjNum = Convert.ToByte(_enemyNum + _barricadeNum);

        PlayerPrefs.SetInt("EnemyNum",_enemyNum);
        PlayerPrefs.SetInt("BarricadeNum",_barricadeNum);
=======


>>>>>>> parent of e473dd6 (aaa)
    }
    
    void Update()
    {
        if (_playerAct == null)
        {
            Debug.Log("null�ł�");
        }
        else
        {
            _speedGage.fillAmount = SpeedRate();

            //�_����\��
            _timeText.text = _playerAct.ClearTime.ToString("F2");
        }

        if (_playerAct._mode == PlayerAction.GameMode.Play && _isSaveLoad) return;
        _isSaveLoad = false;
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
        AsyncLoadScene(ct, _waitTime).Forget();
    }

    private void SaveBreakableObj()
    {
        PlayerPrefs.SetInt("BreakEnemyNum", _playerAct.BreakEnemyNum);
        PlayerPrefs.SetInt("BreakBarricadeNum", _playerAct.BreakBarricadeNum);
    }
    /// <summary>
    /// 
    /// </summary>
    private float SpeedRate()
    {
        _speed = _playerAct.SpeedGage;
        return _speed / 6;
    }
    /// <summary>
    /// �I�u�W�F�N�g��T���ăR���|�[�l���g���擾
    /// </summary>
    private void FindConponent()
    {
        _timeText = GameObject.Find("TimeText").GetComponent<Text>();
        _speedGage = GameObject.Find("SpeedGage").GetComponent<Image>();
        _playerAct = GameObject.Find("Player").GetComponent<PlayerAction>();
    }
<<<<<<< HEAD
    /// <summary>
    /// ���U���g��ʂ����[�h����
    /// </summary>
    private async UniTask AsyncLoadScene(CancellationToken ct,float waitTime)
    {
        try
        {
            await UniTask.Delay((int)(waitTime * 1000), cancellationToken: ct);
        }
        catch (OperationCanceledException) { }
        finally
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
=======
>>>>>>> parent of e473dd6 (aaa)
}
