using System.Collections.Generic;
using System;
using UnityEngine;



[Serializable]
public class Cells
{
    public OmokCell[] cells = new OmokCell[13];
    public OmokCell this[int index]
    {
        get { return cells[index]; }
        set { cells[index] = value; }
    }
}
public class BoardController : MonoBehaviour
{
    public Cells[] cells;
    //public OmokCell[] cell;
    //public int boardSize = 13;     // ������ ũ��
    //public float cellSize = 1.0f; // �� ũ��


    void Start()
    {
        //cells = new OmokCell[boardSize + 1, boardSize + 1];  // �� �迭 �ʱ�ȭ
        //CreateCells();
    }
    //void CreateCells()
    //{
    //    float length = boardSize * cellSize;
    //    float halfLength = length / 2f;

    //    // �������� ��� ���� �����մϴ�. �� ���� ����.
    //    for (int x = 0; x <= boardSize; x++)
    //    {
    //        for (int y = 0; y <= boardSize; y++)  // ��� ���� ����
    //        {
    //            // (0,0)�� ���� ���� ������ Y�� ����
    //            Vector3 pos = new Vector3(x * cellSize - halfLength, (boardSize - y) * cellSize - halfLength, 0);

    //            GameObject cellObj = Instantiate(cellPrefab, transform);
    //            cellObj.transform.localPosition = pos;

    //            OmokCell cell = cellObj.GetComponent<OmokCell>();
    //            cell.x = x;
    //            cell.y = y;

    //            cells[x, y] = cell; // �� �迭�� ����
    //        }
    //    }
    //}
}
