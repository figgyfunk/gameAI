using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dynamicEvade : MonoBehaviour {

    public int numSegments;
    public GameObject target;
    public Sprite targetSprite;
    public float targetRadius;
    public float dangerRadius;
    public float maxSpeed;
    public float timeToTarget;
    public float maxAcceleration;
    public Vector2 velocity;
    private Vector2 position;
    private float orientation = 0f;
    private Vector2 linearAcceleration;
    private float rotation = 0f;
    private float angularAcceleration = 1f;

    public float detectDistance;
    public float wanderOffset;
    public float wanderRadius;
    public float wanderRate;
    private float wanderOrientation = 0f;
    private bool setWander = true;
    private GameObject start;

    private void Start()
    {
        position = transform.position;
        velocity = new Vector2(0, 0);
        start = GameObject.Find("startGame");
    }

    private void OnGUI()
    {
        if (setWander)
        {
            GUI.Label(new Rect(0, 0, 250, 50), "Wolf: Wandering");
        }
        else
        {
            
            GUI.Label(new Rect(0, 0, 250, 50), "Wolf: Evading");
        }
    }
    private void wander()
    {
        wanderOrientation += Random.value * wanderRate;
        float targetOrientation = wanderOrientation + orientation;
        Vector2 wanderTarget = position + wanderOffset * (new Vector2(Mathf.Sin(orientation), Mathf.Cos(orientation)));
        wanderTarget += wanderRadius * (new Vector2(Mathf.Sin(orientation), Mathf.Cos(orientation)));
        linearAcceleration = maxAcceleration * new Vector2(Mathf.Sin(orientation), Mathf.Cos(orientation));


    }

    public Vector2 flee()
    {
        Vector2 direction = transform.position - target.transform.position;
        float distance = direction.magnitude;
        float targetSpeed;
        if (distance < targetRadius)
        {
            start.SendMessage("targetsCollide");
        }

        if (distance > dangerRadius)
        {
            targetSpeed = maxSpeed * (distance / dangerRadius);
        }
        else
        {
            targetSpeed = maxSpeed;
        }

        Vector2 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        Vector2 returnVelocity = targetVelocity - velocity;
        returnVelocity /= timeToTarget;

        if (returnVelocity.magnitude > maxAcceleration)
        {
            returnVelocity.Normalize();
            returnVelocity *= maxAcceleration;
        }
        return returnVelocity;
    }

    private void updateKinematics(float time)
    {
        position += velocity * time + .5f * linearAcceleration * time * time;
        orientation += rotation * time + .5f * angularAcceleration * time * time;
        velocity += linearAcceleration * time;
        if(velocity.magnitude > maxSpeed)
        {
            velocity = new Vector2(maxSpeed, maxSpeed);
        }
        rotation += angularAcceleration * time;
    }

    private void updateSteering()
    {

        linearAcceleration = flee();
        //angularAcceleration = 0f;
    }

    public void DoRenderer()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        float radius = target.GetComponent<dynamicPursue>().slowRadius;
        lineRenderer.positionCount =(numSegments + 1);
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, z, 0);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    private float updateOrientation()
    {
        if (velocity.magnitude > 0)
        {
            return Mathf.Atan2(-velocity.x, velocity.y);
        }
        else
        {
            return orientation;
        }
    }
    private void Update()
    {
        if (setWander)
        {
            wander();
            updateKinematics(Time.deltaTime);
            if((target.transform.position - transform.position).magnitude <= detectDistance)
            {
                setWander = false;
            }
        }
        else
        {
            updateSteering();
            updateKinematics(Time.deltaTime);
            DoRenderer();
        }
        gameObject.transform.position = position;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * updateOrientation());


    }
}
