using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [SerializeField] Transform _followObj;
    [SerializeField] Transform _goalObj;
    private int _diffBackGroundX = 4; //Playerとの距離の差　（diff = difference）
    //Transform _sparkPos;

    public enum ObjName
    {
        Spark,
        BackGround
    }
    [SerializeField] ObjName _objName = ObjName.BackGround;

    private void Start()
    {
        //_sparkPos = GameObject.Find("SparkPos").GetComponent<Transform>();
    }

    void Update()
    {
        switch (_objName)
        {
            case ObjName.Spark:
                Particle();
                break;

            case ObjName.BackGround:
                BackGround();
                break;

            default:
                break;
        }
    }

    void BackGround()
    {
        if (_followObj.position.x < _goalObj.position.x)
        {
            //追従するオブジェクトのX座標
            float followObjPosX = _followObj.position.x + _diffBackGroundX;
            transform.position = new Vector2(followObjPosX, transform.position.y);
        }
        
    }

    void Particle()
    {
        //このObjの現在ｘ座標を参照し、それに追跡するObjのｘ座標を足す
        //Yも同じ
        transform.position = _followObj.position;

    }
}
