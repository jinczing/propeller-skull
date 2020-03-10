using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public GameObject targetShell;
    static public int collideCounter = 0;
    public int localCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        PropellerAgent agent = other.gameObject.GetComponent<PropellerAgent>();
        agent.AddReward(5.0f);
        agent.timer = 0;
        collideCounter++;
        localCounter++;
        if (localCounter >= 10)
        {
            localCounter = 0;
            agent.Done();
        }
            
        Debug.Log(collideCounter);
        Initialize();
    }


    public void Initialize()
    {
        this.transform.localPosition = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30));
        targetShell.GetComponent<TargetShellBehavior>().Initialize();
        targetShell.transform.position = this.transform.position;
    }
}
