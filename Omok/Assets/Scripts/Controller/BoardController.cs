using Define;
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
    public OmokCell this[float x] { get { return cells[(int)x]; } set { cells[(int)x] = value; } }
}
public class BoardController : MonoBehaviour
{
    #region variables
    public Cells[] Cells;
    public Stone BlackStone;
    public Stone WhiteStone;
    public Stone ForbiddenStone;
    public OmokCell this[float y, float x]
    {
        get => Cells[(int)y][x];
        set => Cells[(int)y][x] = value;
    }
    public OmokCell this[Vector2 pos]
    {
        get => Cells[(int)pos.y][pos.x];
        set => Cells[(int)pos.y][pos.x] = value;
    }
    #endregion
    int a = 0;
    private void Start()
    {
        GameManager.Rule.Board = this;
    }
    private void Update()
    {
#if UNITY_EDITOR // 디버깅용
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int cellMask = LayerMask.GetMask("Cell");
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, cellMask);

            if (hit.collider != null)
            {
                var cell = hit.collider.GetComponent<OmokCell>();

                if (a % 2 == 0)
                {
                    if (Cells[cell.y][cell.x].StoneType == StoneType.NONE)
                    {
                        cell.PlaceStone(BlackStone);
                        UpdateBoard();
                        //a++;
                    }
                }
                else
                {
                    cell.PlaceStone(WhiteStone);
                    a++;
                }
            }
        }
#endif
    }
    void UpdateBoard()
    {
        for (int y = 0; y < 13; y++)
        {
            for (int x = 0; x < 13; x++)
            {
                if (Cells[y][x].StoneType == Define.StoneType.NONE)
                {
                    if (GameManager.Rule.CanPlace(y, x, true) == false)
                        Cells[y][x].PlaceStone(ForbiddenStone);
                }
                else if (Cells[y][x].StoneType == Define.StoneType.FORBIDDEN)
                {
                    if (GameManager.Rule.CanPlace(y, x, true) == true)
                        Cells[y][x].PlaceStone(null);
                }
            }
        }
    }
}
