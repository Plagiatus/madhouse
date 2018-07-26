using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionDetection : MonoBehaviour {
    List<GameObject> targetList;
    private Camera camera;
     
    public void Start()
    {
        camera = this.GetComponent<Camera>();
    }
    public void Update()
    {

    }
    public void setTargetList(List<GameObject> targetList)
    {
        this.targetList = targetList;
    }
    public List<GameObject> getVisibleCharacters()
    {
        NPC[] allNPCs = GameObject.FindObjectsOfType<NPC>();
        Player player = GameObject.FindObjectOfType<Player>();
        List<GameObject> PossibleTargets = new List<GameObject>();
        foreach(NPC npc in allNPCs)
        {
            if(npc.isActiveAndEnabled)
                PossibleTargets.Add(npc.gameObject);
        }
        if (player.isActiveAndEnabled)
            PossibleTargets.Add(player.gameObject);
        List<GameObject> actualTargets = new List<GameObject>();
        foreach(GameObject target in PossibleTargets)
        {
            if(targetVisible(target))
            {
                actualTargets.Add(target);
            }
        }
        return actualTargets;
    }
    public List<GameObject> targetsVisible()
    {
        List<GameObject> detectedTargets = new List<GameObject>();
        foreach(GameObject target in targetList)
        {
            Vector3 targetPos = target.transform.position;

            Vector3 viewportPos = camera.WorldToViewportPoint(targetPos);
            if(viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0)
            {
                if(Physics.Raycast(this.transform.position, targetPos - this.transform.position))
                {
                    detectedTargets.Add(target);
                }
            }
        }
        return detectedTargets;
    }

    public bool targetVisible(GameObject target)
    {
        Vector3 targetPos = target.GetComponentInChildren<Renderer>().bounds.center;
        Vector3 viewportPos = camera.WorldToViewportPoint(targetPos);
        Debug.Log("viewportPos" + viewportPos);
        if (viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0)
        {
            RaycastHit hit;
            Physics.Raycast(this.transform.position, targetPos - this.transform.position, out hit);
            Debug.Log(hit.distance);
            Debug.DrawRay(this.transform.position, targetPos - this.transform.position, Color.white, 2);
            if (hit.transform.gameObject == target)
            {
                return true;
            }
        }
        return false;
    }
    //Returns -1 if the target being looked for is not visible
    public float targetDistance(GameObject target)
    {
        Vector3 targetPos = target.GetComponentInChildren<Renderer>().bounds.center;
        if(targetVisible(target))
        {
            RaycastHit hit;
            Physics.Raycast(this.transform.position, targetPos - this.transform.position, out hit);
            return hit.distance;
        }
        else
        {
            return -1;
        }
    }
}
