using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NavTarget : MonoBehaviour
{
    //Array containing Possible next targets and how likely they are to occur for random roaming behaviour
    public string name = "";
    public List<NavTarget> nextTarget;
    public List<float> probability;
    private Dictionary<NavTarget, float> RoamingNextTarget;
    private float ProbabilitySum;

	// Use this for initialization
	void Start () {
        RoamingNextTarget = new Dictionary<NavTarget, float>();
        if(nextTarget.Count == 0)
        {
            nextTarget.Add(this.GetComponent<NavTarget>());
        }
        if(probability.Count == 0)
        {
            probability.Add(1f);
        }
        for(int i = 0; i < nextTarget.Count; i++)
        {
            float nextProbability;
            if(i<probability.Count)
            {
                nextProbability = probability[i];
            }
            else
            {
                nextProbability = probability[probability.Count - 1];
            }
            
            RoamingNextTarget.Add(nextTarget[i], nextProbability);
        }
        ProbabilitySum = 0;
        if (RoamingNextTarget != null)
        {
            foreach (float weight in RoamingNextTarget.Values)
            {
                ProbabilitySum += weight;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    NavTarget GiveRandomNext()
    {
        float target = Random.Range(0, ProbabilitySum);
        float currentValue = 0;
        NavTarget returnValue = new NavTarget();
        foreach (KeyValuePair<NavTarget, float> entry in RoamingNextTarget)
        {
            currentValue += entry.Value;
            returnValue = entry.Key;
            if (currentValue >= target)
            {
                
                break;
            }
        }
        return returnValue;
    }
    /*public void OnGUI()
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        foreach(NavTarget nav in RoamingNextTarget.Keys)
        {
            Handles.DrawLine(this.transform.position, nav.transform.position);
        }
        
    }*/
}
