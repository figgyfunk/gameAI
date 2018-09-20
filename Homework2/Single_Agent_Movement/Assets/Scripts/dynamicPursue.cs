using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class dynamicPursue : MonoBehaviour {

    public GameObject target;
    public Sprite targetSprite;
    public float maxAcceleration;
    public float targetRadius;
    public float maxSpeed;
    public float slowRadius;
    public float timeToTarget;
    public float maxPrediction;
    private Vector2 linearAcceleration;
    private float angularAcceleration = 3f;
    private Vector2 position;
    private float orientation = 0f;
    private Vector2 velocity;
    private float rotation = 3f;

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

    private void OnGUI()
    {
        if (setWander)
        {
            GUI.Label(new Rect(0, 30, 250, 50), "Hunter: Wandering");
        }
        else
        {
            GUI.Label(new Rect(0, 30, 250, 50), "Hunter: Pursuing");
            Vector3 targetPos = predict();
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);
            GUI.Label(new Rect(pos.x, Screen.height - pos.y, 20, 20), targetSprite.texture);
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
            SceneManager.LoadScene("red_and_wolf");
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
        //orientation = Mathf.Atan2(-velocity.x, velocity.y);
        orientation += rotation * time + .5f * angularAcceleration * time * time;

        velocity += linearAcceleration * time;
        rotation += angularAcceleration * time;
        
    }

    private void updateSteering()
    {
         
        linearAcceleration = arrive();
        //angularAcceleration = 0f;
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
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.Euler(0,0, Mathf.Rad2Deg * updateOrientation());

            if(target.GetComponent<dynamicEvade>().velocity != new Vector2(0, 0))
            {
                if ((target.transform.position - transform.position).magnitude <= detectDistance)
                {
                    setWander = false;
                }
            }
            
        }
        else
        {
            updateSteering();
            updateKinematics(Time.deltaTime);
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * updateOrientation());
        }
               
    }
}
