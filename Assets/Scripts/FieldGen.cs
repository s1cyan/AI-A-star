using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGen : MonoBehaviour
{

    public GameObject tile;
    public int x = 22;
    public int z = 10;
    public int num_obstacles = 10;
    public Matrix m;
    private IList<GameObject> tilesList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        m.InitializeMatrix(x, z);
        InitializeField();
        SetObstacles();

    }

    private void SetObstacles()
    {
        var need_obs = num_obstacles;

        if (num_obstacles < x + z)
        {
            for (int i = 0; i < need_obs; i++)
            {
                var tempx = Random.Range(0, x);
                var tempz = Random.Range(0, z);
                if (m.refMatrix[tempx,tempz] != null)
                {
                    //matrix.SetCell(tempx, tempz, VisitStat.obstacle);
                    var obs = getTile(tempx, tempz);
                    obs.transform.localScale += new Vector3(0, 5, 0);
                }
           
            }
         
        }
    }

    private GameObject getTile(int inputx, int inputz)
    {
        return tilesList[inputx + inputz * z];
    }


    private void InitializeField()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                var single = Instantiate(tile);
                single.transform.position = new Vector3(i, 0, j);
                tilesList.Add(single);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
		
    }

}
