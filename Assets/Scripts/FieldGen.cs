using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGen : MonoBehaviour {

    public int x = 20;
    public int z = 30;
    public GameObject tile;

    private Dictionary<Vector3, GameObject> tileDict = new Dictionary<Vector3, GameObject>();

    void Awake()
    {
        InitializeField();
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
}
