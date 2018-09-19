using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hunterSeek : MonoBehaviour {

    public float maxAcceleration;
    public GameObject target;
    private Vector2 linearAcceleration;
    private Vector2 position;
    private Vector2 velocity;
    private float orientation;
    private float rotation;
    private float angularAcceleration;
    public float targetRadius;
    public float slowRadius;
    public float maxSpeed;
    public float timeToTarget;
    // Use this for initialization

    private void Start()
    {
        position = transform.position;
        velocity = new Vector2(0, 0);
    }

    private void seek()
    {
        Vector2 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;
        float targetSpeed;

        if (distance < targetRadius)
        {
            // do something else.
        }

        if (distance > slowRadius)
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

        if (returnVelocity.magnitude > maxAcceleration)
        {
            returnVelocity.Normalize();
            returnVelocity *= maxAcceleration;
        }
        linearAcceleration = returnVelocity;
    }

    private void updateKinematics(float time)
    {
        position += velocity * time + .5f * linearAcceleration * time * time;
        orientation += rotation * time + .5f * angularAcceleration * time * time;

        velocity += linearAcceleration * time;
        rotation += angularAcceleration * time;
    }
    // Update is called once per frame
    void Update ()
    {
        seek();
        updateKinematics(Time.deltaTime);
        transform.position = position;
	}
}
