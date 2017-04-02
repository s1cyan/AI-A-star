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
    public float fn;

    public void SetNode(Vector3 position, NodeMeasure prevNode = null, float toGoal = 0, float totalTraveled = 0)
    {
        pos = position;
        previousNode = prevNode;
        hn = toGoal;
        gn = totalTraveled;
        fn = hn + gn;
    }

}

public class NodeSearcher : MonoBehaviour
{
    private IList<Vector3> neighbors = new List<Vector3>();
    private IList<NodeMeasure> expandednodes = new List<NodeMeasure>();
    private IList<NodeMeasure> closednodes = new List<NodeMeasure>();


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
        expandednodes.Clear();
        closednodes.Clear();
        neighbors.Clear();

        var rootNode = new NodeMeasure();
        rootNode.SetNode(zeroY(transform.position));
        expandednodes.Add(rootNode);
        current = rootNode;
    }

    private void AstarSearch()
    {
        while (current.pos != goal)
        {
            Debug.Log("hit search initiation");
            current.pos = goal;
        }
    }

    private List<NodeMeasure> GetNeighbors()
    {
        var neighbors = new List<NodeMeasure>();
        foreach (Vector3 move in moveAbilities)
        {
            try
            {
                var tile = field.tileDict[current.pos + move];
                var node = new NodeMeasure();
                node.pos = current.pos + move;
                node.previousNode = current;
                neighbors.Add(node);
                Debug.Log("---" + (current.pos + move));
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
            Debug.Log("CLICK");
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
            if (hit.transform.tag == "Tile")
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
