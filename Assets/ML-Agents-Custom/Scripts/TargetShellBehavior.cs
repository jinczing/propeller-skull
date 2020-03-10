using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShellBehavior : MonoBehaviour
{
    static public int collideCounter = 0;
    private bool collide = false;

    private void OnTriggerEnter(Collider other)
    {
        if(collide == false)
        {
            PropellerAgent agent = other.gameObject.GetComponent<PropellerAgent>();
            agent.AddReward(2.5f);
            collideCounter++;
            Debug.Log(collideCounter);
            collide = true;
        }
        
    }

    public void Initialize()
    {
        collide = false;
    }


}
