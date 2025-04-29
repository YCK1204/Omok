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
    //public int boardSize = 13;     // 오목판 크기
    //public float cellSize = 1.0f; // 셀 크기


    void Start()
    {
        //cells = new OmokCell[boardSize + 1, boardSize + 1];  // 셀 배열 초기화
        //CreateCells();
    }
    //void CreateCells()
    //{
    //    float length = boardSize * cellSize;
    //    float halfLength = length / 2f;

    //    // 오목판의 모든 셀을 생성합니다. 맨 윗줄 포함.
    //    for (int x = 0; x <= boardSize; x++)
    //    {
    //        for (int y = 0; y <= boardSize; y++)  // 모든 줄을 포함
    //        {
    //            // (0,0)이 왼쪽 위로 오도록 Y축 반전
    //            Vector3 pos = new Vector3(x * cellSize - halfLength, (boardSize - y) * cellSize - halfLength, 0);

    //            GameObject cellObj = Instantiate(cellPrefab, transform);
    //            cellObj.transform.localPosition = pos;

    //            OmokCell cell = cellObj.GetComponent<OmokCell>();
    //            cell.x = x;
    //            cell.y = y;

    //            cells[x, y] = cell; // 셀 배열에 저장
    //        }
    //    }
    //}
}
