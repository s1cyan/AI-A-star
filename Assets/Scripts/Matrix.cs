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

public class Matrix : MonoBehaviour
{
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

    public Cell[,] matrix;


    public void InitializeMatrix(int x, int z)
    {
      
        matrix = new Cell[x, z];

    }

    public void SetCell(int x, int z, VisitStat visitStat)
    {
        matrix[x, z].visitstat = visitStat;
    }

    public void SetCalcs(int x, int z, float hn, float gn)
    {
        matrix[x, z].hn = hn;
        matrix[x, z].gn = gn;

    }

    public Cell GetCell(int x, int z)
    {
        Debug.LogFormat("-- {0}, {1}", x, z);
        return matrix[x, z];

    }


  
}
