using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisitStat
{
    unknown,
    obstacle,
    visited,
    closed
}
public class Cell
{
    public VisitStat visitstat = VisitStat.unknown;
    public float hn = float.MaxValue;
    public float gn = float.MaxValue;

    public float getFN()
    {
        return hn + gn;

    }

}
[CreateAssetMenu]
public class Matrix : ScriptableObject
{
    

    public Cell[,] refMatrix;
    private int maxz;
    private int maxx;


    public void InitializeMatrix(int x, int z)
    {
      
        refMatrix = new Cell[x, z];
        maxz = z;
        maxx = x;
        refMatrix[0, 0].visitstat = VisitStat.closed;
        refMatrix[0, 0].hn = 10;
        refMatrix[0, 0].gn = 10;

        Debug.Log("-- "+ refMatrix[0,0].visitstat);
        /*
        for (int i = 0; i <x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                cellsList.Add(new Cell() );
            }
        }
        */

    }

    public void SetCell(int x, int z, VisitStat visitStat)
    {
        refMatrix[x,z].visitstat = visitStat;
    }

    public void SetCalcs(int x, int z, float hn, float gn)
    {
        refMatrix[x,z].hn = hn;
        refMatrix[x,z].gn = gn;

    }

    public Cell GetCell(int x, int z)
    {
        Debug.LogFormat("-- {0}, {1}", x, z);
        return refMatrix[x,z];

    }
   
  
}
