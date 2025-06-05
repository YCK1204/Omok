using Define;
using JetBrains.Annotations;
using UnityEngine;

public class RenjuRuleManager
{
    public BoardController Board { get; set; }
    Vector2[] Directions = new Vector2[]
    {
        new Vector2 { x = 0, y = 1 },  // →
        new Vector2 { x = 1, y = 0 },  // ↓
        new Vector2 { x = 1, y = 1 },  // ↘
        new Vector2 { x = 1, y = -1 },  // ↙
    };

    public bool CanPlace(int y, int x, bool update = false)
    {
        if (Board.Cells[y][x].StoneType == Define.StoneType.NONE || (update == true && Board.Cells[y][x].StoneType == Define.StoneType.FORBIDDEN))
        {
            var placeType = GetOmokPlaceType(y, x);
            if (placeType == Define.OmokPlaceType.FIVE || placeType == Define.OmokPlaceType.VALID)
                return true;
        }
        return false;
    }

    bool InBoard(int y, int x) { return (0 <= y && y < 13 && 0 <= x && x < 13); }
    bool InBoard(Vector2 pos) { return InBoard((int)pos.y, (int)pos.x); }

    bool Is4(Vector2 pos, Vector2 direction)
    {
        int count = 1;
        int blankCount = 0;
        bool firstBlank = false;

        // 전방 (한 방향)
        Vector2 nextPos = pos + direction;
        while (InBoard(nextPos) && Board[nextPos].StoneType != StoneType.WHITE)
        {
            if (Board[nextPos].StoneType == Define.StoneType.BLACK)
            {
                count++;
                nextPos += direction;
            }
            else
            {
                blankCount++;
                if (firstBlank)
                    break;

                firstBlank = true;
                nextPos += direction;
            }
        }

        // 후방 (반대 방향)
        firstBlank = false;
        nextPos = pos - direction;
        while (InBoard(nextPos) && Board[nextPos].StoneType != StoneType.WHITE)
        {
            if (Board[nextPos].StoneType == Define.StoneType.BLACK)
            {
                count++;
                nextPos -= direction;
            }
            else
            {
                blankCount++;
                if (firstBlank)
                    break;

                firstBlank = true;
                nextPos -= direction;
            }
        }

        return (count == 4 && blankCount > 0);
    }
    int Count4(Vector2 pos)
    {
        int len = Directions.Length;
        int result = 0;

        for (int i = 0; i < len; i++)
        {
            Vector2 direction = Directions[i];
            if (Is4(pos, direction))
                result++;
        }
        return result;
    }
    int CountInLine(int y, int x, int dy, int dx)
    {
        int count = 1;
        int nx = x + dx;
        int ny = y + dy;

        while (InBoard(ny, nx))
        {
            if (Board.Cells[ny][nx].StoneType != Define.StoneType.BLACK)
                break;
            count++;
            nx += dx;
            ny += dy;
        }
        nx = x - dx;
        ny = y - dy;
        while (InBoard(ny, nx))
        {
            if (Board.Cells[ny][nx].StoneType != Define.StoneType.BLACK)
                break;
            count++;
            nx -= dx;
            ny -= dy;
        }
        return count;
    }
    bool IsOpen4(Vector2 maxPos, Vector2 minPos, Vector2 direction)
    {
        Vector2 newMaxPos1 = maxPos + direction;
        Vector2 newMaxPos2 = newMaxPos1 + direction;

        if (InBoard(newMaxPos1) == false)
            return false;
        if (Board[newMaxPos1].StoneType == StoneType.WHITE)
            return false;
        if (InBoard(newMaxPos2) && Board[newMaxPos2].StoneType == StoneType.BLACK) // 3칸 떨어진 곳에 흑돌이 있어서 6목이 되는 경우
            return false;

        Vector2 newMinPos1 = minPos - direction;
        Vector2 newMinPos2 = newMinPos1 - direction;
        if (InBoard(newMinPos1) == false)
            return false;
        if (Board[newMinPos1].StoneType == StoneType.WHITE)
            return false;
        if (InBoard(newMinPos2) && Board[newMinPos2].StoneType == StoneType.BLACK) // 3칸 떨어진 곳에 흑돌이 있어서 6목이 되는 경우
            return false;
        return true;
    }
    Vector2 FindBlank(Vector2 maxPos, Vector2 minPos, Vector2 direction)
    {
        Vector2 first = maxPos - direction;
        Vector2 second = minPos + direction;

        if (Board[first].StoneType == Define.StoneType.NONE || Board[first].StoneType == StoneType.FORBIDDEN)
            return first;
        return second;
    }
    bool IsOpen3(Vector2 maxPos, Vector2 minPos, Vector2 direction)
    {
        // 띈 3인 경우
        if (Mathf.Abs(maxPos.x - minPos.x) == 3 || Mathf.Abs(maxPos.y - minPos.y) == 3)
        {
            Vector2 blankPos = FindBlank(maxPos, minPos, direction);
            if (Count4(blankPos) > 0) // 띈 부분이 44가 되는 경우
                return false;
            if (IsOpen4(maxPos, minPos, direction) == false) // 띈 부분을 매꿧을 때 절대 열린 4가 안되는 경우
                return false;
        }
        // 일반 연속 3인 경우
        else
        {
            Vector2 newMaxPos = maxPos + direction;
            Vector2 newMinPos = minPos - direction;
            // 전방향 후방향 모두 열린4를 만들 수 없는 경우
            if (IsOpen4(newMaxPos, minPos, direction) == false && IsOpen4(maxPos, newMinPos, direction) == false)
                return false;
            // 양쪽 다 띈4만 쳐야하는 경우
            if (Count4(newMaxPos) > 0 && Count4(newMinPos) > 0)
                return false;
        }
        return true;
    }
    bool IsOpen3(Vector2 pos, Vector2 direction)
    {
        int count = 1;
        int blankCount = 0;
        bool firstBlank = false;

        // 전방 (한 방향)
        Vector2 nextPos = pos;
        Vector2 maxPos = pos;
        nextPos += direction;
        while (InBoard(nextPos) && Board[nextPos].StoneType != StoneType.WHITE)
        {
            if (Board[nextPos].StoneType == Define.StoneType.BLACK)
            {
                count++;
                maxPos = nextPos;
                nextPos += direction;
            }
            else
            {
                blankCount++;
                if (firstBlank)
                    break;

                firstBlank = true;
                nextPos += direction;
            }
        }

        // 후방 (반대 방향)
        firstBlank = false;
        Vector2 minPos = pos;
        nextPos = pos;
        nextPos -= direction;
        while (InBoard(nextPos) && Board[nextPos].StoneType != StoneType.WHITE)
        {
            if (Board[nextPos].StoneType == Define.StoneType.BLACK)
            {
                count++;
                minPos = nextPos;
                nextPos -= direction;
            }
            else
            {
                blankCount++;
                if (firstBlank)
                    break;

                firstBlank = true;
                nextPos -= direction;
            }
        }

        if ((count == 3) && blankCount >= 3)
            return IsOpen3(maxPos, minPos, direction); // 열린 3인지 확인
        return false;
    }
    int Count3(Vector2 pos)
    {
        int len = Directions.Length;
        int count = 0;

        for (int i = 0; i < len; i++)
        {
            Vector2 direction = Directions[i];

            if (IsOpen3(pos, direction) == true)
                count++;
        }
        return count;
    }

    int CountMax(Vector2 pos)
    {
        int len = Directions.Length;
        int result = 0;

        for (int i = 0; i < len; i++)
        {
            Vector2 direction = Directions[i];
            int cnt = CountInLine((int)pos.y, (int)pos.x, (int)direction.y, (int)direction.x);

            result = Mathf.Max(result, cnt);
        }
        return result;
    }
    Define.OmokPlaceType GetOmokPlaceType(int y, int x)
    {
        Vector2 pos = new Vector2(x, y);
        int max = CountMax(pos);
        if (max == 5) { return OmokPlaceType.FIVE; }


        Define.OmokPlaceType ret = Define.OmokPlaceType.VALID;
        bool is3 = false;
        bool is4 = false;

        if (Count3(pos) >= 2)
            ret = OmokPlaceType.DOUBLE_THREE;
        else if (Count4(pos) >= 2)
            ret = OmokPlaceType.DOUBLE_FOUR;

        switch (max)
        {
            case 6:
                ret = Define.OmokPlaceType.SIX;
                break;
            case 7:
                ret = Define.OmokPlaceType.SEVEN;
                break;
            case 8:
                ret = Define.OmokPlaceType.EIGHT;
                break;
            case 9:
                ret = Define.OmokPlaceType.NINE;
                break;
        }
        return ret;
    }
}
