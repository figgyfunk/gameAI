using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toStart : MonoBehaviour {

	public void buttonPress()
    {
        SceneManager.LoadScene("pursue_and_evade");
    }
}
