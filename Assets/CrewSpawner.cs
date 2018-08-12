﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewSpawner : MonoBehaviour {

	public int maxCrewSpawn = 5;
	public float timeBetweenSpawns = 30f;

	public GameObject crewPrefab;

	public Sprite[] possibleHair;
	public Sprite[] possibleHead;
	public Sprite[] possibleBody;

	Waypoint[] spawnWaypoints;
	int numberOfSpawns = 0;
	float timeSinceLastSpawn;

	// Use this for initialization
	void Start () {
		spawnWaypoints = FindObjectsOfType<Waypoint>();
	}
	
	// Update is called once per frame
	void Update () {
		 if (Time.time - timeSinceLastSpawn > timeBetweenSpawns)
        {
            timeSinceLastSpawn = Time.time;
			Waypoint waypoint = GetRandomWaypoint();
			SpawnCrewMember(waypoint.transform.position);
		}
	}

    private void SpawnCrewMember(Vector3 position)
    {
        GameObject newCrewMemberObject = Instantiate(crewPrefab, position, Quaternion.identity, transform);
		newCrewMemberObject.GetComponentInChildren<HairRenderer>().GetComponent<SpriteRenderer>().sprite = 
			possibleHair[UnityEngine.Random.Range(0, possibleHair.Length)];
		newCrewMemberObject.GetComponentInChildren<HeadRenderer>().GetComponent<SpriteRenderer>().sprite = 
			possibleHead[UnityEngine.Random.Range(0, possibleHead.Length)];
		newCrewMemberObject.GetComponentInChildren<BodyRenderer>().GetComponent<SpriteRenderer>().sprite = 
			possibleBody[UnityEngine.Random.Range(0, possibleBody.Length)];
    }

    private Waypoint GetRandomWaypoint()
    {
        List<Waypoint> waypoints = new List<Waypoint>(FindObjectsOfType<Waypoint>());
        
        if (waypoints.Count > 0){
            return waypoints[UnityEngine.Random.Range(0, waypoints.Count)];
        }
        Debug.LogError("No waypoints found");
        return null;
    }
}