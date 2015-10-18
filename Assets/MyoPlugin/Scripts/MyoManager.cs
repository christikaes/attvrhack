using UnityEngine;
using System.Collections;
using System;
using MyoUnity;

public class MyoManager : MonoBehaviour {
	public static MyoManager instance;
	private static AndroidJavaObject jo;

	private static bool isInitialized = false;
	private static bool isAttached = false;

	public static event Action<MyoPose> PoseEvent;
	public static event Action AttachEvent, DetachEvent, ConnectEvent, DisconnectEvent,
								ArmSyncEvent, ArmUnsyncEvent, LockEvent, UnlockEvent;

	
	private static Quaternion quaternion;

	void Awake(){
        //Singelton from UnityPatterns.com (non existing anymore)
        if (instance == null)
		{
            instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
            if (this != instance)
				Destroy(this.gameObject);
		}
    }

	#region PluginCalls
	public static void Initialize()
	{
        MyoPluginDemo.debugMessage = "initialize";
        #if UNITY_ANDROID && !UNITY_EDITOR
		if(!isInitialized){
            MyoPluginDemo.debugMessage += " isNotInitialized ";
			jo = new AndroidJavaObject ("com.strieg.myoplugin.MyoStarter");
			using (AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject ajo = ajc.GetStatic<AndroidJavaObject>("currentActivity")) {
					jo.Call ("launchService", ajo);
				}
			}
			if (jo != null)
				isInitialized = true;
		}
        MyoPluginDemo.debugMessage += " initializedFinished";
        #endif
    }

	public static void Uninitialize()
	{
        MyoPluginDemo.debugMessage = "";
        #if UNITY_ANDROID && !UNITY_EDITOR
		if (isInitialized) {
			using (AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject ajo = ajc.GetStatic<AndroidJavaObject>("currentActivity")) {
					jo.Call ("stopService", ajo);
				}
			}
			jo = null;
			isInitialized = false;
			isAttached = false;
		}
#endif
    }

	public static void AttachToAdjacent(){
        MyoPluginDemo.debugMessage = " attachtoadjacent";
        Debug.Log("AttachToAdjacent");
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(isInitialized) jo.Call ("attachToAdjacent");
		#endif
	}

	//Vibrates the Myo device for the specified length, Short, Medium, or Long.
	public static void VibrateForLength( MyoVibrateLength length ){
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(isInitialized) jo.Call ("vibrateForLength", (int)length);
		#endif
	}
	#endregion

	#region DelegateCalls
	//this are the methods which are called from java
	public void OnAttach(string message){
        MyoPluginDemo.debugMessage = " onattach";
        if (AttachEvent != null)
			AttachEvent();
		isAttached = true;
	}

	public void OnDetach(string message){
        MyoPluginDemo.debugMessage = " ondetach";
        if (DetachEvent != null)
			DetachEvent();
		isAttached = false;
	}

	public void OnConnect(string message){
        MyoPluginDemo.debugMessage = " onconnect";
        if (ConnectEvent != null)
			ConnectEvent();
		isAttached = true;
	}

	public void OnDisconnect(string message){
        MyoPluginDemo.debugMessage = " ondisconnect";
        if (DisconnectEvent != null)
			DisconnectEvent();
		isAttached = false;
	}

	public void OnArmSync(string message){
        MyoPluginDemo.debugMessage = " onarmsync";
        if (ArmSyncEvent != null)
			ArmSyncEvent();
	}
	
	public void OnArmUnsync(string message){
        MyoPluginDemo.debugMessage = " onarmunsync";
        if (ArmUnsyncEvent != null)
			ArmUnsyncEvent();
	}

	public void OnUnlock(string message){
        MyoPluginDemo.debugMessage = " onunlock";
        if (UnlockEvent != null)
			UnlockEvent();
	}
	
	public void OnLock(string message){
        MyoPluginDemo.debugMessage = " onlock";
        if (LockEvent != null)
			LockEvent();
	}
	
	public void OnPose(string message)
	{
        MyoPluginDemo.debugMessage = " onpose";
        MyoPose pose = (MyoPose) Enum.Parse(typeof(MyoPose), message, true);
		if (PoseEvent != null) PoseEvent( pose );
	}

	public void OnOrientationData(string message)
	{
        MyoPluginDemo.debugMessage = " onorientationdata";
        string[] tokens = message.Split(',');
		float x=0, y=0, z=0, w=0;
		float.TryParse( tokens[0], out x );
		float.TryParse( tokens[1], out y );
		float.TryParse( tokens[2], out z );
		float.TryParse( tokens[3], out w );
	
		quaternion = new Quaternion( y, z, -x, -w );
	}
	#endregion

	#region Public Getter
	public static Quaternion GetQuaternion(){
        MyoPluginDemo.debugMessage = " getquaternion";
        return quaternion;
	}

	public static bool GetIsInitialized(){
        MyoPluginDemo.debugMessage = " getisinitialized";
        return isInitialized;
	}

	public static bool GetIsAttached(){
        MyoPluginDemo.debugMessage = " getisattached";
        return isAttached;
	}
	#endregion
}

