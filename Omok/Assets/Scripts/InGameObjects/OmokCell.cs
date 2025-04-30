using UnityEngine;

public class OmokCell : MonoBehaviour
{
    public int x;
    public int y;
    public Stone Stone;

    public Define.StoneType GetStoneType
    {
        get
        {
            if (Stone == null)
                return Define.StoneType.NONE;
            return Stone.StoneType;
        }
    }
    public void PlaceStone(Stone stonePrefab)
    {
        if (Stone != null) return;

        Stone = Instantiate(stonePrefab, transform.position, Quaternion.identity, transform);
    }
}