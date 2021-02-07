using UnityEngine;
using System.Collections;


public class DemoSceneTarget : MonoBehaviour{
	
	public float minX=-5;
	public float maxX=8;
	public float minY=-4;
	public float maxY=4;
	
	public ParticleSystem hitEffect;
	
	//called when hit
	IEnumerator OnTriggerEnter(){
		//place the hitEffect at the object position and assign a random color to it
		hitEffect.transform.position=transform.position;
		
		var main = hitEffect.main;
        main.startColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //hitEffect.startColor=new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
		
		//emit a set number of particle
		hitEffect.Emit(30);
		
		yield return null;
		
		//place the target at a new position
		Vector3 pos=new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
		transform.position=pos;
	}
	
}