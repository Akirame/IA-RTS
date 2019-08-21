using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{

    public enum States
    {
        Idle=0,
        GoMining,
        Mining,
        GoDepositing,
        Depositing,
        Count
    }

    public enum Events
    {
        MineralOnSight=0,
        MineralReached,
        MiningCompleted,
        DepositReached,
        DepositCompleted,
        Count
    }

    FSM fsm;
    public float speed = 10;
    private GameObject mineral;
    private GameObject baseWorkers;
    private float timer = 0;
    private float mineralTime = 2f;
    private Rigidbody rig;
    private Vector3 positionDifference;
    private Vector3 velocity;
    private Vector3 direction;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        fsm = new FSM((int)States.Count, (int)Events.Count, (int)States.Idle);

        fsm.SetRelation((int)States.Idle,         (int)Events.MineralOnSight,   (int)States.GoMining);
        fsm.SetRelation((int)States.GoMining,     (int)Events.MineralReached,   (int)States.Mining);
        fsm.SetRelation((int)States.Mining,       (int)Events.MiningCompleted,  (int)States.GoDepositing);
        fsm.SetRelation((int)States.GoDepositing, (int)Events.DepositReached,   (int)States.Depositing);
        fsm.SetRelation((int)States.Depositing,   (int)Events.DepositCompleted, (int)States.Idle);
    }
    void Update()
    {
        switch(fsm.GetState())
        {
            case (int)States.Idle:
                Idle();
                break;
            case (int)States.GoMining:
                GoMining();
                break;
            case (int)States.Mining:
                Mining();
                break;
            case (int)States.GoDepositing:
                GoDepositing();
                break;
            case (int)States.Depositing:
                Depositing();
                break;
        }
    }

    private void Depositing()
    {
        if(timer < mineralTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            fsm.SendEvent((int)Events.DepositCompleted);
            Debug.Log("EVENT: " + Events.DepositCompleted);
        }
    }

    private void GoDepositing()
    {
        if(!baseWorkers)
        {
            baseWorkers = FindObjectOfType<Base>().gameObject;
        }
        positionDifference = baseWorkers.transform.position - transform.position;
        direction = positionDifference.normalized;
        direction.y = 0;
        velocity = direction * speed * Time.deltaTime;
        rig.velocity = velocity;
    }

    private void Mining()
    {
        if(timer < mineralTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            fsm.SendEvent((int)Events.MiningCompleted);
            Debug.Log("EVENT: " + Events.MiningCompleted);
        }
    }

    private void GoMining()
    {
        positionDifference = mineral.transform.position - transform.position;
        direction = positionDifference.normalized;
        direction.y = 0;
        velocity = direction * speed * Time.deltaTime;
        rig.velocity = velocity;
    }

    private void Idle()
    {
        mineral = FindObjectOfType<Mineral>().gameObject;
        if(mineral)
        {
            fsm.SendEvent((int)Events.MineralOnSight);
            Debug.Log("EVENT: " + Events.MineralOnSight);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(fsm.GetState() == (int)States.GoMining && collision.gameObject.tag == "Mineral")
        {
            fsm.SendEvent((int)Events.MineralReached);
            Debug.Log("EVENT: " + Events.MineralReached);
            return;
        }
        if(fsm.GetState() == (int)States.GoDepositing && collision.gameObject.tag == "Base")
        {
            fsm.SendEvent((int)Events.DepositReached);
            Debug.Log("EVENT: " + Events.DepositReached);
            return;
        }
    }
}
