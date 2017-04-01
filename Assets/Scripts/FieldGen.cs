using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGen : MonoBehaviour
{

    public GameObject tile;
    public int x = 22;
    public int z = 10;
    public int num_obstacles = 10;
    private Matrix matrix;
    private IList<GameObject> tilesList = new List<GameObject>();

    // Use this for initialization
    void Awake()
    {
        matrix = GetComponent<Matrix>();

        matrix.InitializeMatrix(x, z);
        InitializeField();

    }

    void Start()
    {
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
                if (matrix.GetCell(tempx, tempz).visitstat != VisitStat.obstacle)
                {
                    matrix.SetCell(tempx, tempz, VisitStat.obstacle);
                    var obs = getTile(tempx, tempz);
                    obs.transform.localScale += new Vector3(0, 3, 0);
                    obs.transform.parent = gameObject.transform;

                }
                else
                    need_obs += 1;

            }
         

        }
    }

    private GameObject getTile(int inputx, int inputz)
    {
        return tilesList[x + inputz * z];
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
