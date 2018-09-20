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

    private List<Vector3> checkPoints;
    private int i = 0;

	void Start ()
    {
       
        checkPoints = new List<Vector3>();
  
        for(i = 0; i < 10; i++)
        {
            Vector2 pos = new Vector2(Random.value, Random.value);
            pos = Camera.main.ViewportToWorldPoint(pos);
            checkPoints.Add(pos);
            GetComponent<LineRenderer>().SetPosition(i, new Vector3(pos.x, pos.y, 0));
            //Debug.Log(pos);
        }
        checkPoints.Add(grandmaHouse.transform.position);
        red.GetComponent<redMovement>().setPoints(checkPoints);
        GetComponent<LineRenderer>().SetPosition(i, new Vector3(grandmaHouse.transform.position.x, grandmaHouse.transform.position.y, 0));

    }

    public void wolfHouse()
    {
        Vector2 pos = new Vector2(Random.value, Random.value);
        pos = Camera.main.ViewportToWorldPoint(pos);
        gameHunter = Instantiate(hunter, pos, Quaternion.identity);
        gameHunter.GetComponent<hunterSeek>().target = grandmaHouse;
        grandmaHouse.GetComponent<houseCircle>().radius = gameHunter.GetComponent<hunterSeek>().slowRadius;
        grandmaHouse.GetComponent<houseCircle>().DoRenderer();
    }
    private void Update()
    {
        if (!wolfInstantiated)
        {
            wolfTimer -= Time.deltaTime;
            if( wolfTimer <= 0)
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
