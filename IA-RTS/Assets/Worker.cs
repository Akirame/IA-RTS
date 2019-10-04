using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit {

    public FSM fsm;
    public enum States {GoToMine = 0, Mining, GoToTown, DepositGold, Idle}
    public enum Events {HasGold = 0, MineReached, EmptyGold, MiningTimeOut, TownReached, GoldDeposited}
    public Rigidbody rd;
    public float speed;
    public float miningTime;
    private float miningTimer = 0f;
    public float depositTime = 3f;
    private float depositTimer = 0f;
    public int mineAmount;
    public Mine mine;
    public Town town;
    public int goldInHands;
    public bool handsEmpty = true;
    public List<Vector2> pathToWalk = new List<Vector2>();
    public NodeManager nodeManager;
    public LayerMask rayMask;
    public BTSelectorNode rootNode = new BTSelectorNode();
    private WorkerConditions workerConditions;

    // Use this for initialization
    void Start () {
        fsm = new FSM();
        fsm.Create(5, 6);
        fsm.SetRelation((int)States.Idle, (int)States.GoToMine, (int)Events.HasGold);
        fsm.SetRelation((int)States.GoToMine, (int)States.Mining, (int)Events.MineReached);
        fsm.SetRelation((int)States.Mining, (int)States.GoToTown, (int)Events.MiningTimeOut);
        fsm.SetRelation((int)States.GoToTown, (int)States.DepositGold, (int)Events.TownReached);
        fsm.SetRelation((int)States.DepositGold, (int)States.Idle, (int)Events.GoldDeposited);
        fsm.SendEvent((int)States.Idle);

        workerConditions = new WorkerConditions(this);
        BTConditionNode hasGoldNode = new BTConditionNode(workerConditions.HasMaxGold);

        BTSequenceNode miningNode = new BTSequenceNode();
        BTActionNode goToMineAction = new BTActionNode(GoToMine);
        BTActionNode miningAction = new BTActionNode(Mining);
        miningNode.AddChild(hasGoldNode);
        miningNode.AddChild(goToMineAction);
        miningNode.AddChild(miningAction);

        BTSequenceNode backToTownNode = new BTSequenceNode();
        BTActionNode goToTownAction = new BTActionNode(GoToTown);
        BTActionNode depositGoldAction = new BTActionNode(DepositGold);
        backToTownNode.AddChild(goToTownAction);
        backToTownNode.AddChild(depositGoldAction);

        rootNode.AddChild(miningNode);
        rootNode.AddChild(backToTownNode);

    }

    // Update is called once per frame
    void Update() {
        /*switch ((States)fsm.GetState())
        {
            case States.GoToMine:
                GoToMine();
                break;
            case States.Mining:
                Mining();
                break;
            case States.GoToTown:
                GoToTown();
                break;
            case States.DepositGold:
                DepositGold();
                break;
            case States.Idle:
                Idle();
                break;
            default:
                break;
        }*/
        //if (Input.GetMouseButtonDown(0) && pathToWalk.Count <= 0)
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out hit, 100.0f, rayMask))
        //    {
        //        pathToWalk = nodeManager.GetPathBetweenPos(transform.position, hit.point);
        //        pathToWalk.Reverse();
        //    }
        //}
        //MoveOnPath();
        rootNode.Evaluate();
    }

    private BTNode.NodeStates DepositGold()
    {
        BTNode.NodeStates state = BTNode.NodeStates.Running;
        depositTimer += Time.deltaTime;
        if(depositTimer >= depositTime)
        {
            town.DepositGold(goldInHands);
            depositTimer = 0;
            goldInHands = 0;
            handsEmpty = true;
            fsm.SendEvent((int)Events.GoldDeposited);
            state = BTNode.NodeStates.Success;
        }
        return state;
    }


    private void Idle()
    {
        if (mine.HasGold())
        {
            fsm.SendEvent((int)Events.HasGold);
        }
        else
        {
            fsm.SendEvent((int)Events.EmptyGold);
        }
    }

    private BTNode.NodeStates GoToTown()
    {
        BTNode.NodeStates state = BTNode.NodeStates.Running;
        GetPathTo(town.gameObject);
        MoveOnPath();
        if (DestinationReached(town.transform.position) || pathToWalk.Count == 0)
        {
            rd.velocity = Vector2.zero;
            fsm.SendEvent((int)Events.TownReached);
            state = BTNode.NodeStates.Success;
        }
        return state;
    }

    private void MoveOnPath()
    {
        if (pathToWalk.Count > 0)
        {
            if (!DestinationReached(pathToWalk[0]))
            {
                rd.velocity = (pathToWalk[0] - (Vector2)transform.position).normalized * speed;
            }
            else
            {
                pathToWalk.RemoveAt(0);
            }
        }
        else
            rd.velocity = Vector2.zero;
    }

    private BTNode.NodeStates Mining()
    {
        BTNode.NodeStates state = BTNode.NodeStates.Running;
        miningTimer += Time.deltaTime;
        if (miningTimer >= miningTime)
        {
            miningTimer = 0;
            goldInHands = mine.MineGold(mineAmount);
            fsm.SendEvent((int)Events.MiningTimeOut);
            handsEmpty = false;
            state = BTNode.NodeStates.Success;
        }
        return state;
    }

    private BTNode.NodeStates GoToMine()
    {
        BTNode.NodeStates state = BTNode.NodeStates.Running;
        GetPathTo(mine.gameObject);
        MoveOnPath();
        if(DestinationReached(mine.transform.position) || pathToWalk.Count == 0)
        {
            fsm.SendEvent((int)Events.MineReached);
            rd.velocity = Vector2.zero;
            state = BTNode.NodeStates.Success;
        }
        return state;
    }

    private void GetPathTo(GameObject destination)
    {
        if (pathToWalk.Count == 0)
        {
            pathToWalk = nodeManager.GetPathBetweenPos(transform.position, destination.transform.position);
            pathToWalk.Reverse();
        }
    }

    private bool DestinationReached(Vector3 destination)
    {
        return Vector2.Distance(transform.position, destination) < 1;
    }

    private void OnDrawGizmos()
    {
        if (pathToWalk.Count > 0)
        {
            for (int i = 0; i < pathToWalk.Count; i++)
            {
                if (i + 1 < pathToWalk.Count)
                    Gizmos.DrawLine(pathToWalk[i], pathToWalk[i + 1]);
            }
        }
    }

}
