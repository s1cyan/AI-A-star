using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum PlayerState
{
    standby,
    traversing}
;

public class NodeMeasure
{
    public Vector3 pos;
    public NodeMeasure previousNode;
    public float hn;
    public float gn;
    //calc gn by addin  distance(previous node.pos , node.pos+move)
    public float fn = hn + gn;

    public void SetNode(Vector3 position, NodeMeasure prevNode = null, float toGoal = 0, float totalTraveled = 0)
    {
        pos = position;
        previousNode = prevNode;
        hn = toGoal;
        gn = totalTraveled;
    }

}

public class NodeSearcher : MonoBehaviour
{
    private IList<Vector3> neighbors = new List<Vector3>();
    private IList<NodeMeasure> expandednodes = new List<Vector3>();
    private IList<NodeMeasure> closednodes = new List<Vector3>();


    private Vector3 goal;
    private NodeMeasure current;

    private PlayerState playerState = PlayerState.standby;

    private FieldGen field;

    private Vector3[] moveAbilities =
        { new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(-1, 0, 0),
            new Vector3(1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, -1), new Vector3(-1, 0, 1)
        };


    void OnEnable()
    {
        field = FindObjectOfType<FieldGen>();
    }

    void Start()
    {
        ReadyForSearch();
    }


    private void ReadyForSearch()
    {
        playerState = PlayerState.standby;
        expandednodes.Clear;
        closednodes.Clear;
        neighbors.Clear;

        var rootNode = new NodeMeasure();
        rootNode.SetNode(zeroY(transform.position));
        expandednodes.Add(rootNode);
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
        foreach (Vector3 move in moveAbilities)
        {
            try
            {
                var tile = field.tileDict[current + move];
                var node = new NodeMeasure();
                node.vec = current + move;
                node.previousNode = current;
                node.gn = 
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


    #region helperfunctions

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
            if (hit.transform.name == "Tile" && hit.transform.localScale.y < 1)
            {
                playerState = PlayerState.traversing;
                Debug.Log("hit a tile");

                goal = hit.transform.position;
                var goalTileRenderer = field.tileDict[goal].GetComponent<Renderer>();
                goalTileRenderer.material.color = Color.yellow;
                AstarSearch();
            }

        }
    }

    #endregion
}
