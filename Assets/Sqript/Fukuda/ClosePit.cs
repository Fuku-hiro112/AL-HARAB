using UnityEngine;
using UnityEngine.Tilemaps;

public class ClosePit : MonoBehaviour
{
    //�p�����g�����Ƃ������ǌ��ǂ����Ŏg�����@���v�����Ȃ�����
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
