using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum PlayerState { standby, traversing};

public struct NodeMeasure
{
    public Vector3 vec;
    public float calc;
}
public class NodeSearcher : MonoBehaviour {
    private Queue<Vector3> neighbors = new Queue<Vector3>();
    private IList<Vector3> visitednodes = new List<Vector3>();
    private IList<Vector3> closednodes = new List<Vector3>();

    private Vector3 goal; 
    private Vector3 current;

    private PlayerState playerState = PlayerState.standby;

    private FieldGen field;
    private Dictionary<Vector3,float> costDict= new Dictionary<Vector3,float>();
    private Vector3[] moveAbilities = { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0),
                                        new Vector3(1, 0, 1), new Vector3(1, 0, -1),new Vector3(-1, 0, -1),new Vector3(-1, 0, 1) };



    void OnEnable()
    {
        field = FindObjectOfType<FieldGen>();
    }

    void Start()
    {
        neighbors.Enqueue(zeroY(transform.position));
        current = zeroY(transform.position);
    }

    private Vector3 zeroY(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }

    private void GrabTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "Tile" && hit.transform.localScale.y == 1)
            {
                playerState = PlayerState.traversing;
                Debug.Log("hit a tile" );

                goal = hit.transform.position;
                var goalTileRenderer = field.tileDict[goal].GetComponent<Renderer>();
                goalTileRenderer.material.color = Color.yellow;
                AstarSearch();
            }

        }
    }

    private void AstarSearch()
    {
        while (current != goal)
        {
            current = neighbors.Dequeue();
            var surrounding = GetNeighbors();
            var minnode = surrounding.Min(node => node.calc);
        }
    }

    private List<NodeMeasure> GetNeighbors()
    {
        var neighbors = new List<NodeMeasure>();
        foreach(Vector3 move in moveAbilities)
        {
            try
            {
                var tile = field.tileDict[current + move];
                var node = new NodeMeasure();
                node.vec = current + move;
                node.calc = 0;
                neighbors.Add(node);
                Debug.Log("---" + (current + move));
            }
            catch
            {

            }
        }
        return neighbors;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerState == PlayerState.standby)
        {
            GrabTile();
        }
    }


}
