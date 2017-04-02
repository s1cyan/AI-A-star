using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGen : MonoBehaviour
{

    public int x = 20;
    public int z = 30;

    public int obstacles = 30;
    public GameObject tile;
    public Color color;


    public Dictionary<Vector3, GameObject> tileDict = new Dictionary<Vector3, GameObject>();

    void Awake()
    {
        InitializeField();
        SetObstacles();
    }

    private void InitializeField()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                var initTile = Instantiate(tile);
                initTile.transform.position = new Vector3(i, 0, j);
                tileDict.Add(new Vector3(i, 0, j), initTile);
            }
        }
    }

    private void SetObstacles()
    {
        for (int obs = 0; obs < obstacles; obs++)
        {
            var tempx = Random.Range(1, x);
            var tempz = Random.Range(1, z);

            tileDict[new Vector3(tempx, 0, tempz)].transform.localScale += new Vector3(0, 5, 0);
        }
    }
}
