using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    GameObject _batDeath;
    //SpriteRenderer _batDeathSprt;
    GameObject _whilteBatDeath;
    //SpriteRenderer _whiteBatDeathSprt;
    GameObject _barricadeDeath;

    [SerializeField]
    Object _obj;
    enum Object
    {
        Bat,
        WhiteBat,
        Barricade
    }

    void Start()
    {
        _batDeath = GameObject.Find("BatDeathImg");
        _whilteBatDeath = GameObject.Find("WhilteBatDeathImg");
        _barricadeDeath = GameObject.Find("BarricadeDeathImg");
    }

    public void BreakAnimation()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        switch (_obj)
        {
            case Object.Bat:
                AsyncBreakAnime(_batDeath,ct).Forget();
                break;
            case Object.WhiteBat:
                AsyncBreakAnime(_whilteBatDeath,ct).Forget();
                break;
            case Object.Barricade:
                AsyncBarricadeBreakAnime(_barricadeDeath,ct).Forget();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 指定秒数、指定の位置で表示する
    /// </summary>
    async UniTask AsyncBreakAnime(GameObject obj ,CancellationToken ct)
    {
        obj.GetComponent<SpriteRenderer>().enabled = true;
        obj.transform.position = this.transform.position;
        
        await UniTask.Delay(200, cancellationToken:ct);

        obj.GetComponent<SpriteRenderer>().enabled = false;
    }

    async UniTask AsyncBarricadeBreakAnime(GameObject obj ,CancellationToken ct)
    {
        obj.SetActive(true);
        obj.transform.position = this.transform.position;
        
        await UniTask.Delay(2000, cancellationToken:ct);

        obj.SetActive(false);
    }
}
