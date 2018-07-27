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

    //Chasing
    public float range;
    GameObject currentTarget;
    private Vector3 returnPos;
    private Vector3 lastKnownPlayerPos;
    private float searchRadius;
    private bool nightShift;

    //Searching
    public float searchTime;
    private float searchDuration;

    private void navigate()
    {
        if (agent.remainingDistance < 0.1)
        {
            if (currentStop <= 0)
            {
                calculateNextTarget();
                currentRouteTarget = PatrolRoute[nextPatrolPosition];
                agent.destination = currentRouteTarget.transform.position;
                animator.SetBool("Move", true);
                currentStop = patrolRestTime + Random.Range(-1, 1);
            }
            else
            {

                animator.SetBool("Move", false);
                currentStop -= Time.deltaTime;
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
        if (state == eState.PATROLLING)
        {
            navigate();
            if (nightShift)
            {
                foreach (GameObject target in targets)
                {
                    if (eyes.targetVisible(target))
                    {
                        if (eyes.targetDistance(target) < range)
                        {
                            currentTarget = target;
                            returnPos = this.transform.position;
                            state = eState.CHASING;
                            lastKnownPlayerPos = target.transform.position;
                            agent.SetDestination(lastKnownPlayerPos);
                        }
                    }
                }
            }
        }
        else if (state == eState.CHASING)
        {
            if (eyes.targetVisible(currentTarget))
            {
                lastKnownPlayerPos = currentTarget.transform.position;
                agent.SetDestination(lastKnownPlayerPos);
            }
            else
            {
                if (agent.remainingDistance < 0.1)
                {
                    state = eState.SEARCH;
                    searchDuration = searchTime;
                    float randomDirection = Random.Range(0, searchRadius);
                    agent.SetDestination(new Vector3(lastKnownPlayerPos.x + randomDirection, lastKnownPlayerPos.y, lastKnownPlayerPos.z + randomDirection));
                }
            }
        }
        if (state == eState.SEARCH)
        {
            searchDuration -= Time.deltaTime;
            if (searchDuration >= 0)
            {
                if (agent.remainingDistance < 0.1)
                {
                    float randomDirection = Random.Range(0, searchRadius);
                    agent.SetDestination(new Vector3(lastKnownPlayerPos.x + randomDirection, lastKnownPlayerPos.y, lastKnownPlayerPos.z + randomDirection));
                }
            }

        }
    }
    // Use this for initialization
    void Start () {
		agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        state = eState.PATROLLING;
        nightShift = true;
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
