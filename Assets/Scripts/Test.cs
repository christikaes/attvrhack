﻿using UnityEngine;using System.Collections;using System.Threading;public class Test : MonoBehaviour {    string getarg = "origin";	// Use this for initialization	void Start () {        // Prepared for HTTP requests        string url = "www.httpbin.org/get";        WWW www = new WWW(url);        StartCoroutine(WaitForRequest(www));

        // Select the object by script example
        //GameObject s = GameObject.Find("Sphere");
        //s.GetComponent<Renderer>().enabled = true;    }    // Update is called once per frame    void Update () {		}    // Handle the HTTP request    IEnumerator WaitForRequest(WWW www)    {        yield return www;        // Check for errors        if (www.error == null)        {            // Handle the HTTP Get data            string data = www.text;            JSONObject jobj = new JSONObject(data);            // Convert it into a JSONObject            jobj.GetField(getarg, delegate (JSONObject u)            {                Debug.Log("Godammit " + u.ToString());            });        }        else        {            Debug.Log("WWW Error: " + www.error);        }    }}