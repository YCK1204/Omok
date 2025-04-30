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
        if (stonePrefab == null) // ±ÝÁö ¾ø¾Ú
        {
            GameObject.Destroy(Stone.gameObject);
        }
        else // ¾Æ¹«°Å³ª »ý±è
        {
            if (GetStoneType == stonePrefab.StoneType)
                return;
            Stone = Instantiate(stonePrefab, transform.position, Quaternion.identity, transform);
        }

    }
}