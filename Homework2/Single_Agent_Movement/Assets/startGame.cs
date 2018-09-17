using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGame : MonoBehaviour {

    public GameObject hunter;
    public GameObject wolf;
    public float hunterTimer;
    public float wolfTimer;
    private bool wolfInstantiated = false;
    private bool hunterInstantiated = false;
    private GameObject gameHunter;
    private GameObject gameWolf;

    private void Update()
    {
        if(!hunterInstantiated)
        {
            if (Time.time >= hunterTimer)
            {
                Vector2 pos = new Vector2(Random.value, Random.value);
                pos = Camera.main.ViewportToWorldPoint(pos);
                gameHunter = Instantiate(hunter, pos, Quaternion.identity);
                
                hunterInstantiated = true;
            }
        }

        if (!wolfInstantiated)
        {
            if(Time.time >= wolfTimer)
            {
                Vector2 pos = new Vector2(Random.value, Random.value);
                pos = Camera.main.ViewportToWorldPoint(pos);
                gameWolf = Instantiate(wolf, pos, Quaternion.identity);
                wolfInstantiated = true;
                gameWolf.GetComponent<dynamicEvade>().target = gameHunter;
                gameHunter.GetComponent<dynamicPursue>().target = gameWolf;
            }
        }   
    }
}
