using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [SerializeField] Transform _followObj;
    [SerializeField] Transform _goalObj;
    private int _diffBackGroundX = 4; //Player�Ƃ̋����̍��@�idiff = difference�j
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
            //�Ǐ]����I�u�W�F�N�g��X���W
            float followObjPosX = _followObj.position.x + _diffBackGroundX;
            transform.position = new Vector2(followObjPosX, transform.position.y);
        }
        
    }

    void Particle()
    {
        //����Obj�̌��݂����W���Q�Ƃ��A����ɒǐՂ���Obj�̂����W�𑫂�
        //Y������
        transform.position = _followObj.position;

    }
}
