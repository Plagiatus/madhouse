using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : NPC {

    public List<NavTarget> PatrolRoute;
    public List<GameObject> targets;
    private int nextPatrolPosition;
    private bool routeCompleted;
    private Animator animator;

    //Patrol Route Modifiers
    public bool BackAndForth = false;
    public bool randomizeRoute = false;

    private NavMeshAgent agent;
    public float patrolRestTime;
    private float currentStop;
    private NavTarget currentRouteTarget;
    public float movementSpeed = 3.5f;
    public VisionDetection eyes;

    private void navigate()
    {
        if (agent.remainingDistance < 0.1)
        {
            Debug.Log("Does this bithc even do anything right now?");
            if (currentStop <= 0)
            {
                calculateNextTarget();
                currentRouteTarget = PatrolRoute[nextPatrolPosition];
                agent.destination = currentRouteTarget.transform.position;
                animator.SetBool("Move", true);
                currentStop = patrolRestTime + Random.Range(-1, 1);
                Debug.Log("It do!!");
            }
            else
            {

                animator.SetBool("Move", false);
                currentStop -= Time.deltaTime;
                Debug.Log("It do!!");
            }
        }
        
    }
    private void calculateNextTarget()
    {
        if(!BackAndForth)
        {
            if(!routeCompleted)
            {
                nextPatrolPosition +=1;
                if (nextPatrolPosition >= PatrolRoute.Count - 1)
                {
                    routeCompleted = true;
                    nextPatrolPosition = PatrolRoute.Count - 1;
                }
            }
            else
            {
                routeCompleted = false;
                nextPatrolPosition = 0;
            }
        }
        else if(BackAndForth)
        {
            if (!routeCompleted)
            {
                nextPatrolPosition +=1;
                if (nextPatrolPosition >= PatrolRoute.Count - 1)
                {
                    routeCompleted = true;
                    nextPatrolPosition = PatrolRoute.Count - 1;
                }
            }
            else
            {
                nextPatrolPosition -=1;
                if (nextPatrolPosition <= 0)
                {
                    routeCompleted = false;
                    nextPatrolPosition = 0;
                }
            }
        }
    }

    public override void AI()
    {
        agent.Move(new Vector3(0,0,0));
        //foreach(GameObject target in targets)
        navigate();
    }
    // Use this for initialization
    void Start () {
		agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentStop = 0;
        currentRouteTarget = PatrolRoute[0];
        if(PatrolRoute.Count > 1)
            nextPatrolPosition = 1;
        agent.destination = currentRouteTarget.transform.position;
        animator.SetFloat("MoveSpeed", movementSpeed);
        animator.SetBool("Move", true);
        agent.speed = movementSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (AIEnabled)
            AI();
        //Debug.Log("Current Target: " + currentRouteTarget + " Distance to Target: " + agent.remainingDistance);
    }
}
