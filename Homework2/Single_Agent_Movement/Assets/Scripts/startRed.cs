using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startRed : MonoBehaviour {

    public GameObject grandmaHouse;
    public GameObject red;
    public GameObject wolf;
    public GameObject hunter;
    private GameObject gameHunter;
    private GameObject gameWolf;
    public float wolfTimer;
    private bool wolfInstantiated = false;

    private List<Vector2> checkPoints;
    private int i = 0;

	void Start ()
    {
        checkPoints = new List<Vector2>();
        for(i = 0; i < 10; i++)
        {
            Vector2 pos = new Vector2(Random.value, Random.value);
            pos = Camera.main.ViewportToWorldPoint(pos);
            checkPoints.Add(pos);
            //Debug.Log(pos);
        }
        checkPoints.Add(grandmaHouse.transform.position);
        red.GetComponent<redMovement>().setPoints(checkPoints);
	}

    public void wolfHouse()
    {
        Vector2 pos = new Vector2(Random.value, Random.value);
        pos = Camera.main.ViewportToWorldPoint(pos);
        gameHunter = Instantiate(hunter, pos, Quaternion.identity);
        gameHunter.GetComponent<hunterSeek>().target = grandmaHouse;
    }
    private void Update()
    {
        if (!wolfInstantiated)
        {
            if(Time.time >= wolfTimer)
            {
                Vector2 pos = new Vector2(Random.value, Random.value);
                pos = Camera.main.ViewportToWorldPoint(pos);
                gameWolf = Instantiate(wolf, pos, Quaternion.identity);
                wolfInstantiated = true;
                gameWolf.GetComponent<wolfPursue>().target = red;
                gameWolf.GetComponent<wolfPursue>().house = grandmaHouse;
                gameWolf.GetComponent<wolfPursue>().startGame = this.gameObject;

            }
        }
    }




}
