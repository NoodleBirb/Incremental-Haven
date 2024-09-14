using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform Player = null;
    public float speed;

    private Vector3 start;
    private Vector3 end;

    private float travelDistance;
    private float timeInterval;
    private float currentTime = 1;
     
    void Update () {
        // if player LMB is pressed
        if (Input.GetMouseButtonDown(0)) {
            // create container to receive hit results
            RaycastHit hit;
            // create ray according to mouse cursot position on the screen
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            // cast ray
            if (Physics.Raycast (ray, out hit)) {
                // check if ray is far enough from the player
                if (Vector3.Distance(Player.position, hit.point) < 1) return;
                // save start and final destination poit
                start = Player.position;
                end = hit.point;
                end.x = (float)(Math.Floor(end.x) +0.5);
                end.z = (float)(Math.Floor(end.z) +0.5);
                // fix y axis
                end.y = Player.position.y;

                currentTime = 0;
                travelDistance = Vector3.Distance(start, end);
                timeInterval = speed / travelDistance;
            }
        }
        // Check if character should be moving
        if (currentTime < 1) {
            // Edge case to make sure it doesn't move too little or too much
            if (currentTime + timeInterval >= 1) {
                currentTime = 1;
            }
            else{
                // change the time position the player should be at
                currentTime += timeInterval;
            }
            // Move the player according to the current time interval
            Player.position = Vector3.Lerp(start, end, currentTime);
        }
    }
}
