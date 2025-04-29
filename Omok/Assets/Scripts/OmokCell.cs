using UnityEngine;

public class OmokCell : MonoBehaviour
{
    public int x;
    public int y;
    public bool hasStone = false; // ���� ��������

    public void PlaceStone(GameObject stonePrefab)
    {
        if (hasStone) return;

        Instantiate(stonePrefab, transform.position, Quaternion.identity, transform);
        hasStone = true;
    }
}