using UnityEngine;
using System.Collections;

public class Fireworks : MonoBehaviour {

	public Transform firework; // the firewark prefab to instantiate
	

	// Range from the camera forward to back to place the firework
	public float closestDistance = 8.0f;      
	public float furthestDistance = 12.0f;
	
	// Use this for initialization
	void Start () 
	{
		
		if ( Application.isEditor )
		{
			// This is a small hack to improve the precision of input events coming
			// from Unity Remote to the editor.
			Time.fixedDeltaTime = 0.01f;
		}
		
		Input.multiTouchEnabled = false;
	}




	IEnumerator ShootFirework(Vector2 touchPosition)
	{
		// Lets set the range from the touch origin fron a randomm distance from closestDistance to furthestDistance
		Ray ray = Camera.main.ScreenPointToRay (touchPosition); 
		Transform fireworkPrefab = (Transform) Instantiate(firework, ray.GetPoint (Random.Range(closestDistance,furthestDistance)), Quaternion.identity);
		yield return true;
	}
    

  

	
	// We use FixedUpdate() instead of Update(), so that we are not framerate dependent.
	void FixedUpdate() 
	{
		
		int count = Input.touchCount;
		if(count>0) {
			Touch touch  = Input.GetTouch(0);   // only interested in the first touch
			if(touch.phase==TouchPhase.Began)
			{
		 		StartCoroutine(ShootFirework(touch.position )); // Start a new thread to fire off the firework
			}
		}
		
	}
}
