﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolManager : MonoBehaviour {

    public StudentAI[] Students;
	// Use this for initialization
	void Start () {
        Students = GameObject.FindObjectsOfType<StudentAI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
