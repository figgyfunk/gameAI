using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class redMovement : MonoBehaviour {

    public float targetRadius;
    public float maxAcceleration;
    public float switchRadius;
    public float slowRadius;
    public float maxSpeed;
    public float timeToTarget;
    private List<Vector3> checkPoints;
    private Vector2 position;
    private Vector2 linearAcceleration;
    private Vector2 currentPoint;
    private int pointIndex = 0;
    private float orientation;
    public Vector2 velocity;
    private float rotation;
    private float angularAcceleration;
    private bool talking = false;


    private void Start()
    {
        position = transform.position;
    }

    public void setPoints(List<Vector3> points)
    {
        checkPoints = points;
        currentPoint = checkPoints[0];
    }

    private void OnGUI()
    {
       
        if (talking)
        {
            GUI.Label(new Rect(0, 30, 250, 50), "Red: Talking");
        }
        else
        {
            GUI.Label(new Rect(0, 30, 250, 50), "Red: Path following");
        }

        
    }

    private void seek()
    {
        Vector2 direction = currentPoint - position;
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

        if ((currentPoint - position).magnitude <= switchRadius)
        {
           
            if (pointIndex != checkPoints.Count - 1)
            {
                pointIndex += 1;
                currentPoint = checkPoints[pointIndex];
            }
            
        }
        
    }

    public void talk()
    {
        talking = !talking;
        if(talking == false)
        {
            Destroy(this.gameObject);
        }
    }

    private float updateOrientation()
    {
        if(velocity.magnitude > 0)
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
        if (!talking)
        {
            seek();
            updateKinematics(Time.deltaTime);
            transform.position = position;
            updateOrientation();
            gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * updateOrientation());
        }
        
        
    }
}
