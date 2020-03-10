using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensor;

public class PropellerAgent : Agent
{
    [Header("Propeller")]
    public GameObject target;
    public GameObject agent;
    public float maxTime;
    public float forceScale;
    public float rewardTowardTarget;
    public Transform[] propellers;
    public ParticleSystem[] ps;
    public AudioClip[] clips; // die, crown, creepy, distance
    private Rigidbody rd;
    public float timer;
    private Vector3 initPos;
    private AudioSource source;
    

    IFloatProperties mResetParams;

    public override void InitializeAgent()
    {
        Random.seed = System.Guid.NewGuid().GetHashCode();

        rd = gameObject.GetComponent<Rigidbody>();
        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;

        source = GetComponentInChildren<AudioSource>();

        mResetParams = Academy.Instance.FloatProperties;

        SetResetParams();
    }

    public override void CollectObservations()
    {
        AddVectorObs(rd.velocity);
        AddVectorObs(rd.angularVelocity);
        AddVectorObs(rd.rotation);
        AddVectorObs(gameObject.transform.localPosition);
        AddVectorObs(target.transform.localPosition);
    }

    public override void AgentAction(float[] vectorAction)
    {

        rd.AddForceAtPosition(vectorAction[0] * forceScale * gameObject.transform.up, propellers[0].position);
        rd.AddForceAtPosition(vectorAction[1] * forceScale * gameObject.transform.up, propellers[1].position);
        rd.AddForceAtPosition(vectorAction[2] * forceScale * gameObject.transform.up, propellers[2].position);
        rd.AddForceAtPosition(vectorAction[3] * forceScale * gameObject.transform.up, propellers[3].position);

        foreach (ParticleSystem p in ps)
        {
            p.transform.rotation = (vectorAction[0] < 0) ? Quaternion.Euler(-90, 0, 0) : Quaternion.Euler(90, 0, 0);
            p.startLifetime = Mathf.Abs(vectorAction[0]) * 0.4f;
        }
    }

    public override void AgentReset()
    {

        timer = 0;
        rd.velocity = Vector3.zero;
        rd.angularVelocity = Vector3.zero;
        Transform env = this.transform.parent.gameObject.GetComponentInParent<Transform>();
        env.position = new Vector3(Random.Range(5, 60), Random.Range(5, 60), Random.Range(5, 60));
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
;       //TargetBehavior tb = target.GetComponent<TargetBehavior>();
        //tb.Initialize();

        SetResetParams();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (ParticleSystem p in ps)
        {
            p.Play();
        }

        timer += Time.fixedDeltaTime;

        if((int)timer % 5 == 0 && (int)timer != 0)
        {
            float ran = Random.value;
            if (ran < 0.05f)
            {
                source.PlayOneShot(clips[1]);
            }
            else if (ran >= 0.05f && ran < 0.2f)
            {
                if(Vector3.Distance(transform.position, target.transform.position) > 20)
                    source.PlayOneShot(clips[3]);
                else
                    source.PlayOneShot(clips[2]);
            }
            else
            {
                
            }
            timer += 1;
        }

        RequestDecision();

        PlaySound();

        //RewardTowardTarget();

        AddReward(-0.001f);

        if(timer >= maxTime)
        {
            //AddReward(-5f);
            Done();
        }
        else if(gameObject.transform.localPosition.x > 40 || gameObject.transform.localPosition.x < -40 ||
            gameObject.transform.localPosition.z > 40 || gameObject.transform.localPosition.z < -40 ||
            gameObject.transform.localPosition.y > 40 || gameObject.transform.localPosition.y < -40)
        {
            //AddReward(-5f);
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[4];
        action[0] = Input.GetKey(KeyCode.Q) ? 1.0f : 0f;
        action[1] = Input.GetKey(KeyCode.W) ? 1.0f : 0f;
        action[2] = Input.GetKey(KeyCode.A) ? 1.0f : 0f;
        action[3] = Input.GetKey(KeyCode.S) ? 1.0f : 0f;
        return action;
    }

    public void SetTargetScale()
    {
        //var targetScale = mResetParams.GetPropertyWithDefault("target_scale", 1.0f);
        //target.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    public void SetResetParams()
    {
        SetTargetScale();
    }

    private void RewardTowardTarget()
    {
        float towardDot = (Vector3.Dot(Vector3.Normalize(target.transform.localPosition - this.transform.localPosition), rd.velocity) / 20 + rd.angularVelocity.magnitude / 100);
        AddReward(0.03f * towardDot);
    }

    private void RewardFaceTarget()
    {
        float faceDot = Mathf.Abs(Vector3.Dot(Vector3.Normalize(target.transform.localPosition - this.transform.localPosition), this.transform.up));
        AddReward(0.01f * faceDot);
    }

    private void RewardRotate()
    {
        float rotateReward = 0.01f * rd.angularVelocity.magnitude / 25;
        AddReward(rotateReward);
    }

    private void PlaySound()
    {
        
    }

    public void PlayDieSound()
    {
        source.PlayOneShot(clips[0]);
    }

}
