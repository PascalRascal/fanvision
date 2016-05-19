using UnityEngine;
using System.Collections;
using WebSocketSharp;
using UnityEngine.UI;
using fanVision;

public class websocketTest : MonoBehaviour {
    public GameObject cam;
    public GameObject POIPrefab;
    public GameObject strikeOut;

    Animation animate;
    GameObject poi;
    Text txt;



    float longi;
    float lati;
    LongLatSpawner lls;

    POIFactory poif;


    // Use this for initialization
    void Start () {
        //Start location services
        Input.location.Start();
        GameObject anim = (GameObject)Instantiate(strikeOut, cam.transform.position + Vector3.forward, cam.transform.rotation);
        animate = anim.GetComponent<Animation>();
        anim.transform.LookAt(cam.transform);
        animate.Play();

    }
	
	// Update is called once per frame
	void Update () {
        if (animate.isPlaying)
        {
            Debug.Log("The animation is playing");
        }

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //HERE WE GO!
                GameObject anim = (GameObject)Instantiate(strikeOut, cam.transform.position + Vector3.forward, cam.transform.rotation);
                anim.GetComponent<Animation>().Play();
                anim.transform.LookAt(cam.transform);
            }
        }
    }

    private void setLLS()
    {
        lati = Input.location.lastData.latitude;
        longi = Input.location.lastData.longitude;
        lls = new LongLatSpawner(lati, longi, cam, POIPrefab);

    }

    private void createPOI(float lat, float longi, string title)
    {
        Vector3 RenderVector = lls.spawnModel(lat, longi);
        poif = new POIFactory(POIPrefab, title, title);
        GameObject POI = (GameObject)Instantiate(poif.getPOI(), cam.transform.position + RenderVector, cam.transform.rotation);
        POI.transform.LookAt(cam.transform);
    }





}
