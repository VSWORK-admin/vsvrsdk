using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FpsDisplay : MonoBehaviour {

	float updateInterval = 0.5f;

	private float accum = 0.0f; // FPS accumulated over the interval
	private float frames = 0; // Frames drawn over the interval
	private float timeleft =0; // Left time for current interval
	
	private Text label;

	void Start()
	{
		label=GetComponent<Text>();
		//if( !GetComponent<GUIText>() )
		//{
		//	print ("FramesPerSecond needs a GUIText component!");
		//	enabled = false;
		//	return;
		//}
		timeleft = updateInterval; 
	}

	void Update()
	{
		if(label==null) return;
		
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
	   
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			//GetComponent<GUIText>().text = "FrameRate = " + (accum/frames).ToString("f2");
			label.text = "FrameRate = " + (accum/frames).ToString("f2");
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}

}
