using UnityEngine;
using System.Collections;

public class calibrationScript : MonoBehaviour {
    public GameObject redBall;
    public GameObject blueBall;
    public GameObject greenBall;
    public GameObject cam;

    //Actually north
    Vector3 north = new Vector3(-1, 0, 0);

    //Actually east
    Vector3 east = new Vector3(0, 0, 1);

    Vector3 up = new Vector3(0, 1, 0);

    // Use this for initialization
    void Start () {
        Instantiate(redBall, cam.transform.position + north, cam.transform.rotation);
        Instantiate(blueBall, cam.transform.position + up, cam.transform.rotation);
        Instantiate(greenBall, cam.transform.position + east, cam.transform.rotation);


    }

    // Update is called once per frame
    void Update () {
	
	}
}
