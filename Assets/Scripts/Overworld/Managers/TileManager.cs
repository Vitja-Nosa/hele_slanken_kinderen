using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase completedNodeTile;

    public void NodeCompleted(Node node)
    {
        node.locked = false;
        Vector3 worldPosition = node.transform.position;
        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition);
        tilemap.SetTile(tilePosition, completedNodeTile);
    }
     
}
