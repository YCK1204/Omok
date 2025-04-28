using UnityEngine;

public class OmokBoardDrawer : MonoBehaviour
{
    public GameObject linePrefab;  // 오목판 선 프리팹
    public GameObject dotPrefab;   // 점 프리팹 (예: 3x3 점)
    public GameObject cellPrefab;  // 셀 프리팹
    public int boardSize = 13;     // 오목판 크기
    public float cellSize = 1.0f; // 셀 크기

    public OmokCell[,] cells; // 오목판 셀 배열

    void Start()
    {
        cells = new OmokCell[boardSize + 2, boardSize + 2];  // 셀 배열 초기화

        DrawBoard();  // 오목판 그리기
        DrawDots();   // 점 그리기 (3x3)
        CreateCells(); // 셀 생성
    }

    void DrawBoard()
    {
        float length = boardSize * cellSize;
        float halfLength = length / 2f;

        // 수직선 (세로)
        for (int i = 0; i <= boardSize; i++)
        {
            Vector3 pos = new Vector3(i * cellSize - halfLength, 0, 0);
            GameObject line = Instantiate(linePrefab, transform);
            line.transform.localPosition = pos;
            line.transform.localScale = new Vector3(0.05f, length, 1f); // 세로선
        }

        // 수평선 (가로)
        for (int i = 0; i <= boardSize; i++)
        {
            Vector3 pos = new Vector3(0, i * cellSize - halfLength, 0);
            GameObject line = Instantiate(linePrefab, transform);
            line.transform.localPosition = pos;
            line.transform.localScale = new Vector3(length, 0.05f, 1f); // 가로선
        }
    }

    void DrawDots()
    {
        float length = boardSize * cellSize;
        float halfLength = length / 2f;

        int[] points = { 3, 6, 9 }; // 점을 찍을 위치 (3, 6, 9)

        foreach (int x in points)
        {
            foreach (int y in points)
            {
                Vector3 pos = new Vector3(x * cellSize - halfLength, y * cellSize - halfLength, 0);
                GameObject dot = Instantiate(dotPrefab, transform);
                dot.transform.localPosition = pos;
                dot.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
            }
        }
    }

    void CreateCells()
    {
        float length = boardSize * cellSize;
        float halfLength = length / 2f;

        // 오목판의 모든 셀을 생성합니다. 맨 윗줄 포함.
        for (int x = 0; x <= boardSize; x++)
        {
            for (int y = 0; y <= boardSize ; y++)  // 모든 줄을 포함
            {
                // (0,0)이 왼쪽 위로 오도록 Y축 반전
                Vector3 pos = new Vector3(x * cellSize - halfLength, (boardSize - y) * cellSize - halfLength, 0);

                GameObject cellObj = Instantiate(cellPrefab, transform);
                cellObj.transform.localPosition = pos;

                OmokCell cell = cellObj.GetComponent<OmokCell>();
                cell.x = x;
                cell.y = y;

                cells[x, y] = cell; // 셀 배열에 저장
            }
        }
    }
}
