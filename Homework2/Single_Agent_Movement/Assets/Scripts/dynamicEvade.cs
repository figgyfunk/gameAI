using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicEvade : MonoBehaviour {

    public GameObject target;
    public float targetRadius;
    public float dangerRadius;
    public float maxSpeed;
    public float timeToTarget;
    public float maxAcceleration;
    public Vector2 velocity;
    private Vector2 position;
    private float orientation;
    private Vector2 linearAcceleration;
    private float rotation;
    private float angularAcceleration;

    public float detectDistance;
    public float wanderOffset;
    public float wanderRadius;
    public float wanderRate;
    private float wanderOrientation = 0f;
    private bool setWander = true;

    private void Start()
    {
        position = transform.position;
        velocity = new Vector2(0, 0);
    }

    private void wander()
    {
        wanderOrientation += Random.value * wanderRate;
        float targetOrientation = wanderOrientation + orientation;
        Vector2 wanderTarget = position + wanderOffset * (new Vector2(Mathf.Sin(orientation), Mathf.Cos(orientation)));
        wanderTarget += wanderRadius * (new Vector2(Mathf.Sin(orientation), Mathf.Cos(orientation)));
        linearAcceleration = maxAcceleration * new Vector2(Mathf.Sin(orientation), Mathf.Cos(orientation));
        orientation = wanderOrientation;

    }

    public Vector2 flee()
    {
        Vector2 direction = transform.position - target.transform.position;
        float distance = direction.magnitude;
        float targetSpeed;
        if (distance < targetRadius)
        {
            // do something else.
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
        }
        gameObject.transform.position = position;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, orientation);


    }
}
