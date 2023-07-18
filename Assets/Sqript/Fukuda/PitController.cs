using UnityEngine;
using UnityEngine.Tilemaps;

public class PitController : MonoBehaviour
{
    [SerializeField]
    private Tile _line1;
    [SerializeField]
    private Tile _line2;

    [SerializeField]
    private Vector3Int _pos;

    private Tilemap _tilemap;
    private Transform _player;
    private ClosePit _closePit;
    private TilemapCollider2D _collider;

    private float _distance;
    [SerializeField]
    private Line _line;
    enum Line
    {
        Top,
        Bottom
    }
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
        _closePit = GameObject.Find("ClosePit").GetComponent<ClosePit>();
        _tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        _distance = _pos.x - _player.position.x; 
        if (_distance < -6)
        {
            _closePit.OnPasteLine(_line1,_line2,_pos,_tilemap);
            
            if (_line == Line.Top) return;
            _collider = GetComponent<TilemapCollider2D>();
            _collider.isTrigger = false;
        }
    }
}
