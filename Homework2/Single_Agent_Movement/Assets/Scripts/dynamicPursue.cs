using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicPursue : MonoBehaviour {

    public GameObject target;
    public float maxAcceleration;
    public float targetRadius;
    public float maxSpeed;
    public float slowRadius;
    public float timeToTarget;
    public float maxPrediction;
    private Vector2 linearAcceleration;
    private float angularAcceleration;
    private Vector2 position;
    private float orientation;
    private Vector2 velocity;
    private float rotation;

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

    private Vector3 predict()
    {
        Vector2 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        float prediction;
        float speed = velocity.magnitude; 

        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        return new Vector2(target.transform.position.x, target.transform.position.y) + (prediction * target.GetComponent<dynamicEvade>().velocity);
    }
    private Vector2 arrive()
    {
        Vector3 targetPos = predict();
        Vector2 direction = targetPos - transform.position;
        float distance = direction.magnitude;
        float targetSpeed;

        if(distance < targetRadius)
        {
            // do something else.
        }

        if( distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * distance / slowRadius;
        }

        Vector2 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        Vector2 returnVelocity = targetVelocity - velocity;
        returnVelocity /= timeToTarget; 

        if(returnVelocity.magnitude > maxAcceleration)
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
        rotation += angularAcceleration * time;
    }

    private void updateSteering()
    {
         
        linearAcceleration = arrive();
        //angularAcceleration = 0f;
    }



    private void Update()
    {

        if (setWander)
        {
            wander();
            updateKinematics(Time.deltaTime);
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.Euler(0,0, orientation);
            if((target.transform.position - transform.position).magnitude <= detectDistance)
            {
                setWander = false;
            }
        }
        else
        {
            updateSteering();
            updateKinematics(Time.deltaTime);
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, orientation);
        }
               
    }
}
