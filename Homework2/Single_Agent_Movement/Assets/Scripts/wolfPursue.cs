using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfPursue : MonoBehaviour {

    public Sprite targetSprite;
    public float talkingTimer;
    public GameObject target;
    public GameObject house;
    public GameObject startGame;
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
    private bool predictBool = true; 

    private bool talking = false; 


    private void Start()
    {
        position = transform.position;
        velocity = new Vector2(0, 0);
    }

    private void OnGUI()
    {
        if (talking)
        {
            GUI.Label(new Rect(0, 0, 250, 50), "Wolf: Talking");
        }
        else
        {
            GUI.Label(new Rect(0, 0, 250, 50), "Wolf: Pursuing");
            if (predictBool)
            {
                Vector3 targetPos = predict();
                Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);
                GUI.Label(new Rect(pos.x, Screen.height - pos.y, 20, 20), targetSprite.texture);
            }
            
        }
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

        return new Vector2(target.transform.position.x, target.transform.position.y) + (prediction * target.GetComponent<redMovement>().velocity);
    }
    private Vector2 arrive()
    {
        Vector3 targetPos;
        if (predictBool)
        {
            targetPos = predict();
        }
        else
        {
            targetPos = target.transform.position;
            //Debug.Log("here");
        }
        
        Vector2 direction = targetPos - transform.position;
        float distance = direction.magnitude;
        float targetSpeed;

        if ((target.transform.position - transform.position).magnitude <= targetRadius)
        {
            if (predictBool)
            {
                target.gameObject.SendMessage("talk");
                talking = true;
            }
            else
            {
                startGame.SendMessage("wolfHouse");
                Destroy(this.gameObject);
            }
            
        }

        if ((target.transform.position - transform.position).magnitude > slowRadius)
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

        if(!talking)
        {
            updateSteering();
            updateKinematics(Time.deltaTime);
            gameObject.transform.position = position;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, updateOrientation() * Mathf.Rad2Deg);
        }
        else
        {
            talkingTimer += -1 * Time.deltaTime;
            if(talkingTimer <= 0)
            {
                target.gameObject.SendMessage("talk");
                target = house; 
                talking = false;
                predictBool = false;
            }
        }

    }
}
