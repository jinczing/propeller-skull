using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PropellerStaticAgent : Agent
{
    [Header("Propeller")]
    public GameObject target;
    private Rigidbody rd;

    IFloatProperties mResetParams;

    public override void InitializeAgent()
    {
        base.InitializeAgent();

        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;

        mResetParams = Academy.Instance.FloatProperties;

        SetResetParams();
    }

    public override void CollectObservations()
    {
        base.CollectObservations();

        AddVectorObs(rd.velocity);
        AddVectorObs(rd.angularVelocity);
        AddVectorObs(Vector3.Distance(this.transform.position, target.transform.position));
    }

    public override void AgentAction(float[] vectorAction)
    {
        base.AgentAction(vectorAction);
        for (int i = 0; i < vectorAction.Length; ++i)
        {
            vectorAction[i] = Mathf.Clamp(vectorAction[i], 0, 1);
        }
        rd.AddForceAtPosition(new Vector3(0, vectorAction[0], 0), new Vector3(0, 0, 1.5f));
        rd.AddForceAtPosition(new Vector3(0, vectorAction[1], 0), new Vector3(1.5f, 0, 0));
        rd.AddForceAtPosition(new Vector3(0, vectorAction[2], 0), new Vector3(0, 0, -1.5f));
        rd.AddForceAtPosition(new Vector3(0, vectorAction[3], 0), new Vector3(-1.5f, 0, 0));

        AddReward(-0.05f * (
        vectorAction[0] * vectorAction[0] +
        vectorAction[1] * vectorAction[1] +
        vectorAction[2] * vectorAction[2] +
        vectorAction[3] * vectorAction[3]) / 4f);
    }

    public override void AgentReset()
    {
        base.AgentReset();

        SetResetParams();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void SetResetParams()
    {

    }
}
