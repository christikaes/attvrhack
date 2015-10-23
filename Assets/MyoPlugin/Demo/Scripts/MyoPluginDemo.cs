using UnityEngine;
using System.Collections;
using MyoUnity;

public class MyoPluginDemo : MonoBehaviour 
{
    public static string debugMessage;
	public Transform objectToRotate;
    public GameObject camera;
    public GameObject pointRotate; //To rotate the camera round this point
    //pointRotate.transform.position
    // Spin the object around the world origin at 20 degrees/second.
    //transform.RotateAround (Vector3.zero, Vector3.up, 20 * Time.deltaTime);
    private Quaternion myoRotation;
	private MyoPose myoPose = MyoPose.UNKNOWN;
    private MyoPose lastPose = MyoPose.UNKNOWN;
    Vector3 lastVector = Vector3.zero;

    void Start () 
	{		
		MyoManager.Initialize ();
		MyoManager.PoseEvent += OnPoseEvent;
	}

	void OnPoseEvent( MyoPose pose )
	{
        lastPose = myoPose;
		myoPose = pose;
	}
	
	void Update()
	{
        if (myoPose.ToString().Equals("FIST")) {
            camera.transform.RotateAround(pointRotate.transform.position, myoRotation * Vector3.forward, 20 * Time.deltaTime);
        } else if (myoPose.ToString().Equals("FINGERS_SPREAD")) {
            //if (Application.loadedLevelName.ToString().Equals("SceneTwo")) {
                //camera.transform.position = Vector3.MoveTowards(camera.transform.position, pointRotate.transform.position, 10 * Time.deltaTime);
            //} else {
                camera.transform.position = Vector3.MoveTowards(camera.transform.position, pointRotate.transform.position, 20 * Time.deltaTime);
            //}
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, pointRotate.transform.position, 20 * Time.deltaTime);
        } else if (myoPose.ToString().Equals("WAVE_OUT")) {
            Vector3 v1 = Vector3.MoveTowards(camera.transform.position, pointRotate.transform.position, 20 * Time.deltaTime);
            v1.y = v1.y + 1;
            camera.transform.position = v1;
        }
        /*if (Cardboard.SDK.Triggered) {
            if (Application.loadedLevelName.ToString().Equals("SceneTwo")) {
                Application.LoadLevel("DemoScene");
            } else {
                Application.LoadLevel("SceneTwo");
            }
        }*/
        
        /*if (Input.GetKey("x"))
            camera.transform.RotateAround(pointRotate.transform.position, Vector3.forward, 20 * Time.deltaTime);
        else if (Input.GetKey("y"))
            camera.transform.RotateAround(pointRotate.transform.position, Vector3.right, 20 * Time.deltaTime);
        else if (Input.GetKey("z"))
            camera.transform.RotateAround(pointRotate.transform.position, Vector3.up, 20 * Time.deltaTime);
        else if (Input.GetKey("a")) {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, pointRotate.transform.position, 20 * Time.deltaTime);
        }
        else if (Input.GetKey("s")) {
            Vector3 v1 = Vector3.MoveTowards(camera.transform.position, pointRotate.transform.position, 20 * Time.deltaTime);
            v1.y = v1.y + 1;
            camera.transform.position = v1;
        }
        else if (Input.GetKeyDown("q"))
        {
            if (Application.loadedLevelName.ToString().Equals("SceneTwo"))
            {
                Application.LoadLevel("DemoScene");
            }
            else
            {
                Application.LoadLevel("SceneTwo");
            }
        } */

        if (MyoManager.GetIsAttached()) {
            debugMessage = "successfully attached";	
			myoRotation = MyoManager.GetQuaternion ();
			objectToRotate.rotation = myoRotation;
		}
	}
	
	void OnGUI()
	{
		GUI.BeginGroup( new Rect( 10, 10, 300, 500 ) );
		
		if (GUILayout.Button ( "Attach to Adjacent" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.AttachToAdjacent();
		}
		
		if (GUILayout.Button ( "Vibrate Short" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.VibrateForLength( MyoVibrateLength.SHORT );
		}
		
		if (GUILayout.Button ( "Vibrate Medium" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.VibrateForLength( MyoVibrateLength.MEDIUM );
		}
		
		if (GUILayout.Button ( "Vibrate Long" , GUILayout.MinWidth(300), GUILayout.MinHeight(50) ) )
		{
			MyoManager.VibrateForLength( MyoVibrateLength.LONG );
		}

		if (!MyoManager.GetIsInitialized()) {
			if (GUILayout.Button ("Initialize MyoPlugin", GUILayout.MinWidth (300), GUILayout.MinHeight (50))) {
				MyoManager.Initialize ();
			}
		} else {
			if (GUILayout.Button ("Uninitialize MyoPlugin", GUILayout.MinWidth (300), GUILayout.MinHeight (50))) {
				MyoManager.Uninitialize ();
			}
		}
		
		GUILayout.Label ( "Myo Quaternion: " + myoRotation.ToString(), GUILayout.MinWidth(300), GUILayout.MinHeight(30) );
		
		GUILayout.Label ( "Myo Pose: " + myoPose.ToString(), GUILayout.MinWidth(300), GUILayout.MinHeight(30) );

		GUILayout.Label ( "Initialized: " + MyoManager.GetIsInitialized(), GUILayout.MinWidth(300), GUILayout.MinHeight(30) );

        GUILayout.Label( "Attached: " + MyoManager.GetIsAttached(), GUILayout.MinWidth(300), GUILayout.MinHeight(30));

        GUILayout.Label( "Your output : " + debugMessage, GUILayout.MinWidth(300), GUILayout.MinHeight(100));
        

        GUI.EndGroup();
	}
}