using System;
using UnityEngine;

// ���� 3 : �� ������ �������� ���� 3
// 33 �ݼ� : ���� 3�� �ΰ��� ��ġ�鼭 �����Ǵ� ���
// ���� 3 üũ ��� : �����Ϸ��� ���� �������� �����ִ���?, �ݴ� ������ �����ִ���?
// �����ִ��� üũ : ���� �����̶� �����ִٸ� ���� 3, bool ������ ���� �����ִ°��� 2�� ������ üũ, 2�� ���Դٸ� �� ������ ����
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
    #region variables
    public Cells[] cells;
    public Stone BlackStone;
    public Stone WhiteStone;
    public Stone ForbiddenStone;
    int[,] Directions = new int[,]
    {
    { 0, 1 },  // ��
    { 1, 0 },  // ��
    { 1, 1 },  // ��
    { 1, -1 }  // ��
    };
    #endregion
    public bool CanPlaceStone(int y, int x, bool isBlack = true)
    {
        if (InBoard(y, x) == false)
            return false;
        if (isBlack == false)
            return true;

        return true;
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(touch.position);

                Debug.Log("���� ��ġ: " + worldPos);
            }
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int cellMask = LayerMask.GetMask("Cell");
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, cellMask);

            if (hit.collider != null)
            {
                var cell = hit.collider.GetComponent<OmokCell>();

                if (CanPlace(cell.y, cell.x))
                {
                    cell.PlaceStone(BlackStone);
                    UpdateBoard();
                }
            }
        }
#endif
    }
    bool Is33(int x, int y, int dx, int dy)
    {
        int count = 1;
        bool firstBlank = false;
        bool forwardOpen = false;
        bool backwardOpen = false;

        // ���� (�� ����)
        int nx = x + dx;
        int ny = y + dy;
        while (InBoard(ny, nx))
        {
            if (cells[ny][nx].GetStoneType == Define.StoneType.BLACK)
            {
                count++;
                nx += dx;
                ny += dy;
            }
            else if (cells[ny][nx].GetStoneType == Define.StoneType.NONE || cells[ny][nx].GetStoneType == Define.StoneType.FORBIDDEN)
            {
                if (!firstBlank)
                {
                    firstBlank = true;
                    nx += dx;
                    ny += dy;
                }
                else
                {
                    forwardOpen = true;
                    break;
                }
            }
            else break;
        }

        // �Ĺ� (�ݴ� ����)
        firstBlank = false;
        nx = x - dx;
        ny = y - dy;

        while (InBoard(ny, nx))
        {
            if (cells[ny][nx].GetStoneType == Define.StoneType.BLACK)
            {
                count++;
                nx -= dx;
                ny -= dy;
            }
            else if (cells[ny][nx].GetStoneType == Define.StoneType.NONE || cells[ny][nx].GetStoneType == Define.StoneType.FORBIDDEN)
            {
                if (!firstBlank)
                {
                    firstBlank = true;
                    nx -= dx;
                    ny -= dy;
                }
                else
                {
                    backwardOpen = true;
                    break;
                }
            }
            else break;
        }

        // ���� �Ǵ�
        return (count == 3) && forwardOpen && backwardOpen;
    }
    bool Is44(int x, int y, int dx, int dy)
    {
        int count = 1;
        bool firstBlank = false;
        bool forwardOpen = false;
        bool backwardOpen = false;

        // ���� (�� ����)
        int nx = x + dx;
        int ny = y + dy;
        while (InBoard(ny, nx))
        {
            if (cells[ny][nx].GetStoneType == Define.StoneType.BLACK)
            {
                count++;
                nx += dx;
                ny += dy;
            }
            else if (cells[ny][nx].GetStoneType == Define.StoneType.NONE || cells[ny][nx].GetStoneType == Define.StoneType.FORBIDDEN)
            {
                if (!firstBlank)
                {
                    firstBlank = true;
                    nx += dx;
                    ny += dy;
                }
                else
                {
                    forwardOpen = true;
                    break;
                }
            }
            else break;
        }

        // �Ĺ� (�ݴ� ����)
        firstBlank = false;
        nx = x - dx;
        ny = y - dy;

        while (InBoard(ny, nx))
        {
            if (cells[ny][nx].GetStoneType == Define.StoneType.BLACK)
            {
                count++;
                nx -= dx;
                ny -= dy;
            }
            else if (cells[ny][nx].GetStoneType == Define.StoneType.NONE || cells[ny][nx].GetStoneType == Define.StoneType.FORBIDDEN)
            {
                if (!firstBlank)
                {
                    firstBlank = true;
                    nx -= dx;
                    ny -= dy;
                }
                else
                {
                    backwardOpen = true;
                    break;
                }
            }
            else break;
        }

        // ���� �Ǵ�
        return (count == 4) && forwardOpen && backwardOpen;
    }
    int CountInLine(int y, int x, int dy, int dx)
    {
        int count = 1;
        int nx = x + dx;
        int ny = y + dy;

        while (InBoard(ny, nx))
        {
            if (cells[ny][nx].GetStoneType != Define.StoneType.BLACK)
                break;
            count++;
            nx += dx;
            ny += dy;
        }
        nx = x - dx;
        ny = y - dy;
        while (InBoard(ny, nx))
        {
            if (cells[ny][nx].GetStoneType != Define.StoneType.BLACK)
                break;
            count++;
            nx -= dx;
            ny -= dy;
        }
        return count;
    }
    Define.OmokPlaceType GetOmokPlaceType(int y, int x)
    {
        Define.OmokPlaceType ret = Define.OmokPlaceType.VALID;
        int len = Directions.GetLength(0);
        bool is4 = false;
        int max = 0;

        if (Check33(y, x) == true)
            ret = Define.OmokPlaceType.DOUBLE_THREE;
        if (Check44(y, x) == true)
            ret = Define.OmokPlaceType.DOUBLE_FOUR;
        for (int i = 0; i < len; i++)
        {
            int dx = Directions[i, 1];
            int dy = Directions[i, 0];

            int cnt = CountInLine(y, x, dy, dx);
            if (max < cnt)
                max = cnt;
            if (cnt == 4)
            {
                if (is4 == true)
                    ret = Define.OmokPlaceType.DOUBLE_FOUR;
                else
                    is4 = true;
            }
        }
        switch (max)
        {
            case 9:
                ret = Define.OmokPlaceType.NINE;
                break;
            case 8:
                ret = Define.OmokPlaceType.EIGHT;
                break;
            case 7:
                ret = Define.OmokPlaceType.SEVEN;
                break;
            case 6:
                ret = Define.OmokPlaceType.SIX;
                break;
            case 5:
                ret = Define.OmokPlaceType.FIVE;
                break;
        }
        return ret;
    }
    bool CanPlace(int y, int x, bool update = false)
    {
        if (cells[y][x].GetStoneType == Define.StoneType.NONE || (update == true && cells[y][x].GetStoneType == Define.StoneType.FORBIDDEN))
        {
            var placeType = GetOmokPlaceType(y, x);
            if (placeType == Define.OmokPlaceType.FIVE)
                return true;
            if (placeType == Define.OmokPlaceType.VALID)
                return true;
        }
        return false;
    }
    bool Check33(int y, int x)
    {
        int count = 0;
        int len = Directions.GetLength(0);

        for (int i = 0; i < len; i++)
        {
            int dx = Directions[i, 1];
            int dy = Directions[i, 0];
            if (Is33(x, y, dx, dy) == true)
                count++;
        }
        return count > 1;
    }
    bool Check44(int y, int x)
    {
        int count = 0;
        int len = Directions.GetLength(0);

        for (int i = 0; i < len; i++)
        {
            int dx = Directions[i, 1];
            int dy = Directions[i, 0];
            if (Is44(x, y, dx, dy) == true)
                count++;
        }
        return count > 1;
    }
    bool InBoard(int y, int x) { return (0 <= y && y < 13 && 0 <= x && x < 13); }
    void UpdateBoard()
    {
        for (int y = 0; y < 13; y++)
        {
            for (int x = 0; x < 13; x++)
            {
                if (cells[y][x].GetStoneType == Define.StoneType.NONE)
                {
                    if (CanPlace(y, x, true) == false)
                        cells[y][x].PlaceStone(ForbiddenStone);
                }
                else if (cells[y][x].GetStoneType == Define.StoneType.FORBIDDEN)
                {
                    if (CanPlace(y, x, true) == true)
                        cells[y][x].PlaceStone(null);
                }
            }
        }
    }
}
