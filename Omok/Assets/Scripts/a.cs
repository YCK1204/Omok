using UnityEngine;

public class a : MonoBehaviour
{
    [SerializeField]
    GameObject gg;
    OmokBoardDrawer b;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int i = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            b = GameObject.Find("A").GetComponent<OmokBoardDrawer>();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            b.cells[i, i].PlaceStone(gg);
            i++;
        }
    }
}
