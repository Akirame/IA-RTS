using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour {

    public FSM fsm;
    public enum States {GoToMine = 0, Mining, GoToTown, DepositGold, Idle}
    public enum Events {HasGold = 0, MineReached, EmptyGold, MiningTimeOut, TownReached, GoldDeposited}
    public Rigidbody2D rd;
    public float speed;
    public float miningTime;
    private float miningTimer = 0;
    public int mineAmount;
    public Mine mine;
    public Town town;
    public int goldInHands;
    public List<Vector2> pathToWalk = new List<Vector2>();
    public NodeManager nodeManager;
    public bool zarlanga = false;
    public LayerMask rayMask;

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
        if (Input.GetMouseButtonDown(0) && pathToWalk.Count <= 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, rayMask))
            {
                pathToWalk = nodeManager.GetPathBetweenPos(transform.position, hit.point);
                pathToWalk.Reverse();
            }
        }
        MoveOnPath();
    }

    private void DepositGold()
    {
        town.DepositGold(goldInHands);
        goldInHands = 0;
        fsm.SendEvent((int)Events.GoldDeposited);
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

    private void GoToTown()
    {
        rd.velocity = (town.transform.position - transform.position).normalized * speed;
        if (DestinationReached(town.transform.position))
        {
            rd.velocity = Vector2.zero;
            fsm.SendEvent((int)Events.TownReached);
        }
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

    private void Mining()
    {
        miningTimer += Time.deltaTime;
        if (miningTimer >= miningTime)
        {
            miningTimer = 0;
            goldInHands = mine.MineGold(mineAmount);
            fsm.SendEvent((int)Events.MiningTimeOut);
        }
    }

    private void GoToMine()
    {
        MoveOnPath();
        if (pathToWalk.Count <= 0)
        {
            fsm.SendEvent((int)Events.MineReached);
            rd.velocity = Vector2.zero;
        }
    }

    private bool DestinationReached(Vector3 destination)
    {
        return Vector2.Distance(transform.position, destination) < 1;
    }
}
