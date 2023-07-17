using UnityEngine;
using UnityEngine.Tilemaps;

public class ClosePit : MonoBehaviour
{
    //継承を使おうとしたけど結局ここで使う方法が思いつかなかった
    public void OnPasteLine(Tile line1, Tile line2, Vector3Int pos, Tilemap tilemap)
    {
        for (int i = 0; i < 2; i++)
        {
            tilemap.SetTile(pos, line1);
            pos.x++;
            tilemap.SetTile(pos, line2);
            pos.x++;
        }
    }
}
