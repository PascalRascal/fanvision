using UnityEngine;
using UnityEngine.UI;
using fanVision;
using GeoUtil;
using System.Collections;
using WebSocketSharp;

public class testLongLat : MonoBehaviour {

    public GameObject POIPrefab;
    public GameObject cam;
    GameObject poi;
    Text txt;

    GameObject town;
    GameObject lookAt;
    //Actually up
    Vector3 up = new Vector3(0, 1);

    //Actually north
    Vector3 north = new Vector3(-1, 0);


    //Actually east
    Vector3 east = new Vector3(0, 0, 1);

    float townHallEasting = 462975.30f;
    float townHallNorthing = 4557416.43f;

    float homeEasting = 462704.71f;
    float homeNorthing = 4557309.34f;

    float longi;
    float lati;
    LongLatSpawner lls;

    POIFactory poif;



    // Use this for initialization
    void Start()
    {
        txt = GetComponent<Text>();
        Input.location.Start();
        WebSocket ws = new WebSocket("ws://stoh.io:6969");
        Debug.Log("Hello FUCK");
        ws.OnMessage += (sender, e) =>
        {
            txt.text = e.Data;
            Debug.Log(e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            txt.text = "There was an error";
        };
        ws.ConnectAsync();





    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {



            }
        }
    }

    private void createPOI(float lat, float longi, string title)
    {
        Vector3 RenderVector = lls.spawnModel(lat, longi);
        poif = new POIFactory(POIPrefab, title, title);
        GameObject POI = (GameObject)Instantiate(poif.getPOI(), cam.transform.position + RenderVector, cam.transform.rotation);
        POI.transform.LookAt(cam.transform);
    }
}
