using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class testPicture : MonoBehaviour {
    RawImage playerPic;

    // Use this for initialization
    void Start () {
        playerPic = GetComponent<RawImage>();
        StartCoroutine(setPicture("http://mlb.mlb.com/mlb/images/players/head_shot/458015.jpg"));

    }
	
	// Update is called once per frame
	void Update () {
	
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
