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
    public float searchSpeed = .13f;

    private List<NodeMeasure> expandednodes = new List<NodeMeasure>();
    private List<NodeMeasure> closednodes = new List<NodeMeasure>();


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

        var rootNode = new NodeMeasure();
        rootNode.SetNode(zeroY(transform.position));
        expandednodes.Add(rootNode);
        current = rootNode;
    }

    private IEnumerator AstarSearch()
    {
        while (current.pos != goal)
        {
            yield return new WaitForSeconds(searchSpeed);

            expandednodes.Sort((x, y) => x.fn.CompareTo(y.fn)); //sort the nodes by the fn value 
            var nodeT = expandednodes[0];
            closednodes.Add(nodeT);
            var closedTileRend = field.tileDict[nodeT.pos].GetComponent<Renderer>();
            closedTileRend.material.color = field.closed;
            current = nodeT;

            expandednodes.Remove(expandednodes[0]);
            GetNeighbors();

        }
        Debug.Log("Found goal");
    }

    private void GetNeighbors()
    {

        foreach (Vector3 move in moveAbilities)
        {
            try
            {
                
                var evalTile = field.tileDict[current.pos + move];
              
                if (evalTile.transform.localScale.y < 1)
                {
                    if (IsDiagonal(move))
                    {
                        Debug.LogFormat("expanded node {0} is DIAGONAL", evalTile.transform.position.ToString());
                        var adjTile1 = field.tileDict[new Vector3(evalTile.transform.position.x, 0, current.pos.z)];

                        var adjTile2 = field.tileDict[new Vector3(current.pos.x, 0, evalTile.transform.position.z)];
                        // screw the safety checks

                        if (adjTile1.transform.localScale.y > evalTile.transform.localScale.y && adjTile2.transform.localScale.y > evalTile.transform.localScale.y)
                        {
                            continue; // if the adjacent tiles to the diagonal move are raised. if they are skip 
                            Debug.Log("SKIPPED THIS DIAGONAL MOVE " + evalTile.transform.position);
                        }
                    }
                                        
                    var tileRenderer = evalTile.GetComponent<Renderer>();
                    var node = new NodeMeasure(); 

                    var hn = Vector3.Distance(current.pos + move, goal);
                    var gn = current.gn + Vector3.Distance(current.pos, current.pos + move);

                    Debug.LogFormat("expanding to node {0}, hn = {1}, gn = {2}, totalcost = {3}", current.pos + move, hn, gn, hn + gn);
                    node.SetNode(current.pos + move, current, hn, gn);

                    if (expandednodes.Exists(nd => nd.pos == node.pos))
                    {
                        ValueUpdateCheck(expandednodes.Find(nd => nd.pos == node.pos), node);

                    }
                    else if (closednodes.Exists(nd => nd.pos == node.pos))
                    {
                    
                    }
                    else
                    {
                        expandednodes.Add(node);
                        tileRenderer.material.color = field.expanded; 
                        Debug.Log("added new node");

                    }

                }
            }
            catch
            {
            }
        }
    }

    private bool IsDiagonal(Vector3 moveAb)
    {
        if (Mathf.Abs(moveAb.x) + Mathf.Abs(moveAb.z) == 2)
        {
            return true;
        }
        return false;
        
    }

    private void ValueUpdateCheck(NodeMeasure existingN, NodeMeasure expN)
    {
        if (expN.fn < existingN.fn)
        {
            existingN = expN;
        }
    }

    private void GrabTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Tile" && hit.transform.localScale.y < 1)
            {
                playerState = PlayerState.traversing;
                //goal is set
                goal = hit.transform.position;
                var goalTileRenderer = field.tileDict[goal].GetComponent<Renderer>();
                goalTileRenderer.material.color = Color.yellow;
                StartCoroutine(AstarSearch());
            }

        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerState == PlayerState.standby)
        {
            GrabTile();
        }
    }

    #region travelers

    #endregion

    #region helperfunctions

    private Vector3 zeroY(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }

    #endregion
}
