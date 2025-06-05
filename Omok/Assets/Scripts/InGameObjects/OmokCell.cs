using UnityEngine;

public class OmokCell : MonoBehaviour
{
    public int x;
    public int y;
    public Stone Stone;

    Define.StoneType _type = Define.StoneType.NONE;
    public Define.StoneType StoneType { get { return _type; } set { _type = value; } }
    public void PlaceStone(Stone stonePrefab)
    {
        if (stonePrefab == null) // ±ÝÁö ¾ø¾Ú
        {
            GameObject.Destroy(Stone.gameObject);
            StoneType = Define.StoneType.NONE;
        }
        else // ¾Æ¹«°Å³ª »ý±è
        {
            if (StoneType == stonePrefab.StoneType)
                return;
            Stone = Instantiate(stonePrefab, transform.position, Quaternion.identity, transform);
            StoneType = Stone.StoneType;
        }
    }
}