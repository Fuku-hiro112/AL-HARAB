using UnityEngine;
using UnityEngine.UI;

public class ResoltManager : MonoBehaviour
{
    [SerializeField] private Text _txtNumberOfBroken;
    [SerializeField] private Text _txtClearTime;
    [SerializeField] private Text _txtClearScore;
    [SerializeField] private Text _txtRank;

    private float _clearTime;
    private int _hpCurrent;

    private int _enemyNum;
    private int _barricadeNum;
    
    private int _breakEnemyNum;
    private int _breakBarricadeNum;

    private float _totalScore;
    private string _rank = "F";

    private const int _noneClear = -1; 

    private const int _scoreS = 9000;
    private const int _scoreA = 8000;
    private const int _scoreB = 6000;

    void Start()
    {
        _clearTime = PlayerPrefs.GetFloat("ClearTime", _noneClear);
        _hpCurrent = PlayerPrefs.GetInt("HpCurrent", 0);
        _enemyNum = PlayerPrefs.GetInt("EnemyNum", 0);
        _barricadeNum = PlayerPrefs.GetInt("BarricadeNum", 0);
        _breakEnemyNum = PlayerPrefs.GetInt("BreakEnemyNum", 0);
        _breakBarricadeNum = PlayerPrefs.GetInt("BreakBarricadeNum", 0);

        _totalScore = ScoreCalculation();

        _txtNumberOfBroken.text = $"{_breakEnemyNum}/{_enemyNum}";

        if (_clearTime == _noneClear)
        {
            _rank = "F";
            _txtRank.text = _rank;
            _txtClearScore.text = $"{100 * _breakEnemyNum + 100 * _breakBarricadeNum}";
            _txtClearTime.text = "~~~";
            return;
        }

        if (_totalScore >= _scoreS) _rank = "S";
        else if (_totalScore >= _scoreA) _rank = "A";
        else if (_totalScore >= _scoreB) _rank = "B";
        else _rank = "C";
        _txtRank.text = _rank;
        _txtClearTime.text = $"{_clearTime.ToString("F")} SEC";
        _txtClearScore.text = _totalScore.ToString("f0");
    }
    float ScoreCalculation()
    {
        float timeScone = 10000 - (this._clearTime * 115);
        float enemyScore = 100 * _breakEnemyNum + 100 * _breakBarricadeNum;
        float hpScore = _hpCurrent * 1000;
        return timeScone + enemyScore + hpScore;
    }
}
