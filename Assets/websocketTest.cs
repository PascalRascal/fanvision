using UnityEngine;
using System.Collections;
using WebSocketSharp;
using UnityEngine.UI;
using fanVision;

public class websocketTest : MonoBehaviour {
    public GameObject cam;
    public GameObject POIPrefab;
    public GameObject strikeOut;
    public Text txt;
    Network_Socket ns;

    Animation animate;
    GameObject poi;



    float longi;
    float lati;
    LongLatSpawner lls;

    POIFactory poif;


    // Use this for initialization
    void Start () {
        //Start location services
        ns = new Network_Socket();
        Input.location.Start();
        setLLS();
        // GameObject anim = (GameObject)Instantiate(strikeOut, cam.transform.position + Vector3.forward, Quaternion.identity);
        //animate = anim.GetComponentInChildren<Animation>();
        //anim.transform.LookAt(cam.transform);
        //animate.Play();
        WebSocket ws = new WebSocket("ws://stoh.io:6969");

    }

    // Update is called once per frame
    void Update () {
        Debug.Log(ns.msg);
        txt.text = ns.msg;


        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //HERE WE GO!
                animate.Play();
            }
        }
    }

    private void setLLS()
    {
        lati = Input.location.lastData.latitude;
        longi = Input.location.lastData.longitude;
        lls = new LongLatSpawner(lati, longi, cam, POIPrefab);

    }

    private void createPOI(float lat, float longit, string title)
    {
        Vector3 RenderVector = lls.spawnModel(lat, longit);
        poif = new POIFactory(POIPrefab, title, title);
        GameObject POI = (GameObject)Instantiate(poif.getPOI(), cam.transform.position + RenderVector, Quaternion.identity);
        POI.transform.LookAt(cam.transform);
    }





}
