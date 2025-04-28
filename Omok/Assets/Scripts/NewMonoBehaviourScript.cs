using UnityEngine;

public class OmokBoardDrawer : MonoBehaviour
{
    public GameObject linePrefab;  // ������ �� ������
    public GameObject dotPrefab;   // �� ������ (��: 3x3 ��)
    public GameObject cellPrefab;  // �� ������
    public int boardSize = 13;     // ������ ũ��
    public float cellSize = 1.0f; // �� ũ��

    public OmokCell[,] cells; // ������ �� �迭

    void Start()
    {
        cells = new OmokCell[boardSize + 2, boardSize + 2];  // �� �迭 �ʱ�ȭ

        DrawBoard();  // ������ �׸���
        DrawDots();   // �� �׸��� (3x3)
        CreateCells(); // �� ����
    }

    void DrawBoard()
    {
        float length = boardSize * cellSize;
        float halfLength = length / 2f;

        // ������ (����)
        for (int i = 0; i <= boardSize; i++)
        {
            Vector3 pos = new Vector3(i * cellSize - halfLength, 0, 0);
            GameObject line = Instantiate(linePrefab, transform);
            line.transform.localPosition = pos;
            line.transform.localScale = new Vector3(0.05f, length, 1f); // ���μ�
        }

        // ���� (����)
        for (int i = 0; i <= boardSize; i++)
        {
            Vector3 pos = new Vector3(0, i * cellSize - halfLength, 0);
            GameObject line = Instantiate(linePrefab, transform);
            line.transform.localPosition = pos;
            line.transform.localScale = new Vector3(length, 0.05f, 1f); // ���μ�
        }
    }

    void DrawDots()
    {
        float length = boardSize * cellSize;
        float halfLength = length / 2f;

        int[] points = { 3, 6, 9 }; // ���� ���� ��ġ (3, 6, 9)

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

        // �������� ��� ���� �����մϴ�. �� ���� ����.
        for (int x = 0; x <= boardSize; x++)
        {
            for (int y = 0; y <= boardSize ; y++)  // ��� ���� ����
            {
                // (0,0)�� ���� ���� ������ Y�� ����
                Vector3 pos = new Vector3(x * cellSize - halfLength, (boardSize - y) * cellSize - halfLength, 0);

                GameObject cellObj = Instantiate(cellPrefab, transform);
                cellObj.transform.localPosition = pos;

                OmokCell cell = cellObj.GetComponent<OmokCell>();
                cell.x = x;
                cell.y = y;

                cells[x, y] = cell; // �� �迭�� ����
            }
        }
    }
}
