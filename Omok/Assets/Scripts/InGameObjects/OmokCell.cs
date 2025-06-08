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
        if (stonePrefab == null) // ���� ����
        {
            GameObject.Destroy(Stone.gameObject);
            StoneType = Define.StoneType.NONE;
        }
        else // �ƹ��ų� ����
        {
            if (StoneType == stonePrefab.StoneType)
                return;
            Stone = Instantiate(stonePrefab, transform.position, Quaternion.identity, transform);
            StoneType = Stone.StoneType;
        }
    }
}