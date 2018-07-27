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
    public float patrolRestTime = 5;
    private float currentStop;
    private NavTarget currentRouteTarget;
    
    public float movementSpeed = 3.5f;
    public VisionDetection eyes;

    //Chasing
    public float range = 10;
    GameObject currentTarget;
    private Vector3 returnPos;
    private Vector3 lastKnownPlayerPos;
    public float searchRadius = 10;
    public bool nightShift = true;

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
                        Debug.Log("Target aquired");
                        if (eyes.targetDistance(target) < range)
                        {
                            Debug.Log("Target in range: " + eyes.targetDistance(target) + " < " + range);
                            currentTarget = target;
                            returnPos = this.transform.position;
                            state = eState.CHASING;
                            Debug.Log("Current State: " + state);
                            lastKnownPlayerPos = target.transform.position;
                            animator.SetBool("Move", true);
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
                animator.SetBool("Move", true);
                agent.SetDestination(lastKnownPlayerPos);
            }
            else
            {
                if (agent.remainingDistance < 0.1)
                {
                    state = eState.SEARCH;
                    Debug.Log("Current State: " + state);
                    searchDuration = searchTime;
                    float randomDirection = Random.Range(0, searchRadius);
                    animator.SetBool("Move", true);
                    agent.SetDestination(new Vector3(lastKnownPlayerPos.x + randomDirection, lastKnownPlayerPos.y, lastKnownPlayerPos.z + randomDirection));
                }
            }
        }
        if (state == eState.SEARCH)
        {
            searchDuration -= Time.deltaTime;
            if (searchDuration >= 0)
            {
                if(searchRadius > 0)
                {
                    if (agent.remainingDistance < 0.1)
                    {
                        float randomDirectionX = Random.Range(-searchRadius, searchRadius);
                        float randomDirectionZ = Random.Range(-searchRadius, searchRadius);
                        animator.SetBool("Move", true);
                        agent.SetDestination(new Vector3(lastKnownPlayerPos.x + randomDirectionX, lastKnownPlayerPos.y, lastKnownPlayerPos.z + randomDirectionZ));
                        Debug.DrawLine(this.transform.position, agent.destination, Color.red, 100);
                        Debug.DrawLine(lastKnownPlayerPos, agent.destination, Color.cyan, 100);
                    }
                    if (eyes.targetVisible(currentTarget))
                    {
                        state = eState.CHASING;
                        lastKnownPlayerPos = currentTarget.transform.position;
                        animator.SetBool("Move", true);
                        agent.SetDestination(lastKnownPlayerPos);
                    }

                }
            }
            else if(searchDuration <= 0)
            {
                state = eState.PATROLLING;
                animator.SetBool("Move", true);
                agent.SetDestination(currentRouteTarget.transform.position);
            }

        }
    }
    // Use this for initialization
    void Start () {
        
        this.gameObject.AddComponent<NavMeshAgent>();
        agent = this.GetComponent<NavMeshAgent>();
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
