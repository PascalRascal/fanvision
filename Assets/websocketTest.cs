using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using WebSocketSharp;
using UnityEngine.UI;
using fanVision;

public class websocketTest : MonoBehaviour {
    public GameObject cam;
    public GameObject POIPrefab;
    public GameObject strikeOut;
    public Text home_score;
    public Text away_score;
    public Text strikes;
    Network_Socket ns;

    Animation animate;
    GameObject poi;



    float longi;
    float lati;
    LongLatSpawner lls;

    string lastMSG = "NO DATA =(";
    POIFactory poif;


    // Use this for initialization
    void Start () {
        //Start location services
        ns = new Network_Socket();
        Input.location.Start();
        setLLS();
        WebSocket ws = new WebSocket("ws://stoh.io:6969");
        poif = new POIFactory(POIPrefab, "Chipotle", "reeeee", "red");
        GameObject poi = (GameObject) Instantiate(poif.getPOI(), cam.transform.position + Vector3.forward, Quaternion.identity);
        poi.transform.LookAt(cam.transform);


    }

    // Update is called once per frame
    void Update () {
        
        if (newMSG())
        {
            handleMSG(ns.msg);
        }

        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //HERE WE GO!
                animate.Play();
            }
        }
    }
    private void handleMSG(string msg)
    {
        var n = JSON.Parse(msg);
        string data_type = removeQuotes(n["data_type"].ToString());
        Debug.Log("Got a message");
        if (data_type.Equals("alert"))
        {
            handleAlert(n["data"]);
        }
        else if (data_type.Equals("game_update"))
        {
            handleGameUpdate(n["data"]);
        }
        else if (data_type.Equals("new_poi"))
        {
            handleNewPOI(n["data"]);
        }
        else if (data_type.Equals("remove_poi"))
        {
            handleRemovePOI(n["data"]);
        }
    }
    private void handleRemovePOI(JSONNode removePOI)
    {
        
    }
    private void handleNewPOI(JSONNode newPOI)
    {
        float latit = newPOI["latitude"].AsFloat;
        float longit = newPOI["longitude"].AsFloat;
        string ttle = newPOI["title"];
        createPOI(latit, longit, ttle);
    }
    private void handleGameUpdate(JSONNode gameUpdate)
    {
        //TODO: Implement HUD
        string item = gameUpdate["item"];
        if (item.Equals("strike"))
        {
            strikes.text = gameUpdate["new_value"];
        }else if (item.Equals("homeScore"))
        {
            home_score.text = gameUpdate["new_value"];
        }else if (item.Equals("awayScore"))
        {
            away_score.text = gameUpdate["new_value"];
        }

    }

    private void handleAlert(JSONNode alrt)
    {
        Debug.Log("An alert was passed");
        string alrt_type = alrt["type"];
        if (alrt_type.Equals("homerun"))
        {
            Debug.Log("Starting FIREWORKS");
            startFireworks();
        }else if (alrt_type.Equals("strike_out"))
        {
            showStrikeout();
        }
        Debug.Log(alrt_type);
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

    private void startFireworks()
    {
        fire_fireworks fireworks = GetComponent<fire_fireworks>();
        fireworks.duration = 10;
        fireworks.enabled = true;
    }

    private void showStrikeout()
    {
        GameObject anim = (GameObject)Instantiate(strikeOut, cam.transform.position + Vector3.forward, Quaternion.identity);
        anim.transform.LookAt(cam.transform);
        //TODO: Remove gameObject after animation and have it render in front of the person every time

    }

    private bool newMSG()
    {
        if (ns.msg.Equals(lastMSG))
        {
            return false;
        }
        else
        {
            lastMSG = ns.msg;
            return true;
        }
    }
    //Removes the quotes from a JSON data field (assumes quotes are at start and end
    private string removeQuotes(string str)
    {
        str = str.Remove(0, 1);
        str = str.Remove(str.Length - 1, 1);
        return str;
    }





}
