﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class StudentAI : MonoBehaviour {
    

    public enum States
    {
        Roam,
        Chill,
        Socialize

    }
    public enum SocialIntent
    {
        Friendly,
        Romantic,
        Hostile

    }
    public Sprite sprite;
    private Rigidbody RB;
    private NavMeshAgent Agent;

    public Vector3 Target;

    public SchoolManager School;


    public StudentAI SocialTarget;

    public States CurrentState;
    public SocialIntent CurrentIntent;
    public bool WorkTime;


    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    void NewDestination()
    {
        Agent.SetDestination(RandomNavmeshLocation(10));
    }
    // Use this for initialization
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        Agent = GetComponent<NavMeshAgent>();
        School = GameObject.FindObjectOfType<SchoolManager>();
        CurrentState = States.Roam;
    }

    public float ChillTime;
    public void DoRoam()
    {
        if (Agent.remainingDistance <= 10)
        {
            CurrentState = States.Chill;
            NewDestination();
            ChillTime = Random.Range(0, 10);
        }


    }

    public StudentAI FindClosetSocialStudent()
    {
        StudentAI CloseSocial = null;
        float Dist = 1000;
        for (int i = 0; i < School.Students.Length; i++)
        {
            float newDist = Vector3.Distance(transform.position, School.Students[i].transform.position);
            if (newDist < Dist && School.Students[i] != this)
            {
                Dist = newDist;
                CloseSocial = School.Students[i];
            }
        }
        return CloseSocial;
    }

    public bool ChatEngaged = false;

    public void DoChill()
    {
        ChillTime -= Time.deltaTime;
        if(ChatEngaged)
        {
            CurrentState = States.Socialize;
        }

        if(ChillTime <= 0)
        {
            //SocialTarget = FindClosetSocialStudent();
            //Agent.SetDestination(SocialTarget.transform.position);
            CurrentState = States.Roam;
            ChatCooldown = 5;
            SocialTarget = null;
        }
    }

    float tempTime;
    public void DoSocial()
    {
        Agent.SetDestination(SocialTarget.transform.position);

        if (ChatEngaged == false && Agent.remainingDistance < 5)
        {
            ChatEngaged = true;
        }
        else if(ChatEngaged == true)
        {
            tempTime -= Time.deltaTime;
        }
       

        if(tempTime <= 0)
        {
            CurrentState = States.Roam;
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        switch (CurrentState)
        {
            case States.Roam:
                DoRoam();
                break;
            case States.Chill:
                DoChill();
                break;
            case States.Socialize:
                DoSocial();
                break;
        }

        if(ChatEngaged == true && CurrentState != States.Socialize && ChatCooldown <= 0)
        {
            CurrentState = States.Socialize;
        }

        ChatCooldown -= Time.deltaTime;
    }

    public float ChatCooldown;
}


