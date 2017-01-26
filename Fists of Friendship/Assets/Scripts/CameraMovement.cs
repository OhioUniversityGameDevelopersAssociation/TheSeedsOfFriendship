using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Rigidbody2D playerRB;
    Vector3 adjustedPos;

    public float cameraLerpSpeed = 1f;

    public bool freeToMove;

    public float currentLeftBound, currentRightBound;

    private void FixedUpdate()
    {
        if((freeToMove || (!freeToMove && playerRB.transform.position.x > currentLeftBound && playerRB.transform.position.x < currentRightBound)) && playerRB.transform.position.x >= 10f) // Locked in arena
        {

            adjustedPos = playerRB.position;
            adjustedPos.y = transform.position.y;
            adjustedPos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, adjustedPos, Time.fixedDeltaTime * cameraLerpSpeed);
        }
    }

    public void EnterEncounter(float leftBound, float rightBound)
    {
        currentLeftBound = leftBound;
        currentRightBound = rightBound;
        freeToMove = false;
    }

    public void ExitEncounter()
    {
        freeToMove = true;
        if(FindObjectOfType<GameManager>().currentEncounter == 2)
        {
            currentRightBound = 103;
        }
    }
}
