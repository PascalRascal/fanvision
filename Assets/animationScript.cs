using UnityEngine;
using System.Collections;

public class animationScript : MonoBehaviour {

    public Animation anim;
    private float timePlaying = 0;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        anim.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.isPlaying)
        {
            timePlaying += Time.deltaTime;
        }
        if(timePlaying >= 5.2)
        {
            Debug.Log("Animation stopped");
            this.gameObject.SetActive(false);
        }
	}
}
