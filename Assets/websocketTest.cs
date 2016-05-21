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
    public Text inningText;
    public Text ballsText;
    public Text outsText;
    public Text atBatText;
    public RawImage playerPic;


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
        //createPOI(41.496042f, -81.683381f, "this is a test", "red");


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
        string color = newPOI["color"];
        createPOI(latit, longit, ttle, color);
    }
    private void handleGameUpdate(JSONNode gameUpdate)
    {
        Debug.Log(gameUpdate);
        int count = gameUpdate.Count;
        for (int i = 0; i < count; i++)
        {
            string item = gameUpdate[i]["item"];

            switch (item)
            {
                case "strikes":
                    string strStrikes = "";
                    int numStrikes = int.Parse(gameUpdate[i]["new_value"]);
                    switch (numStrikes)
                    {
                        case 0:
                            strStrikes = "";
                            break;
                        case 1:
                            strStrikes = "X";
                            break;
                        case 2:
                            strStrikes = "XX";
                            break;
                        case 3:
                            strStrikes = "XXX";
                            showStrikeout();
                            break;

                    }
                    strikes.text = strStrikes;
                    break;
                case "home_score":
                    home_score.text = gameUpdate[i]["new_value"];
                    break;
                case "away_score":
                    away_score.text = gameUpdate[i]["new_value"];
                    break;
                case "inning":
                    int inningNumber = int.Parse(gameUpdate[i]["new_value"]);
                    string strInning = "ERROR";
                    switch (inningNumber)
                    {
                        case 1:
                            strInning = "FIRST";
                            break;
                        case 2:
                            strInning = "SECOND";
                            break;
                        case 3:
                            strInning = "THIRD";
                            break;
                        case 4:
                            strInning = "FOURTH";
                            break;
                        case 5:
                            strInning = "FIFTH";
                            break;
                        case 6:
                            strInning = "SIXTH";
                            break;
                        case 7:
                            strInning = "SEVENTH";
                            break;
                        case 8:
                            strInning = "EIGTH";
                            break;
                        case 9:
                            strInning = "NINTH";
                            break;
                        case 10:
                            strInning = "TENTH";
                            break;
                        default:
                            break;

                    }
                    inningText.text = strInning;
                    break;
                case "balls":
                    int ballNumber = int.Parse(gameUpdate[i]["new_value"]);
                    string strBall = "";
                    switch (ballNumber)
                    {
                        case 0:
                            strBall = "";
                            break;
                        case 1:
                            strBall = "0";
                            break;
                        case 2:
                            strBall = "0 0";
                            break;
                        case 3:
                            strBall = "0 0 0";
                            break;
                        case 4:
                            strBall = "O O O O";
                            break;
                        default:
                            break;

                    }
                    ballsText.text = strBall;
                        
                    break;
                case "outs":
                    int outNumber = int.Parse(gameUpdate[i]["new_value"]);
                    string strOut = "";
                    switch (outNumber)
                    {
                        case 0:
                            strOut = "";
                            break;
                        case 1:
                            strOut = "-";
                            break;
                        case 2:
                            strOut = "- -";
                            break;
                        case 3:
                            strOut = "- - -";
                            break;
                        default:
                            break;

                    }
                    outsText.text = strOut;
                    break;

                case "at_bat":
                    atBatText.text = gameUpdate[i]["new_value"];
                    break;
                case "photo":
                    StartCoroutine(gameUpdate[i]["new_value"]);
                    break;

                default:
                    break;
            }

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


    private void createPOI(float lat, float longit, string title, string color)
    {
        Vector3 RenderVector = lls.spawnModel(lat, longit);
        poif = new POIFactory(POIPrefab, title, title, color);
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
        anim.GetComponentInChildren<animationScript>().enabled = true;
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

    IEnumerator setPicture(string picURL)
    {
        WWW imageUrl = new WWW(picURL);
        while (!imageUrl.isDone)
        {
            Debug.Log("Loading Picture");
        }
        playerPic.texture = imageUrl.texture;
        yield return true;
    }





}
