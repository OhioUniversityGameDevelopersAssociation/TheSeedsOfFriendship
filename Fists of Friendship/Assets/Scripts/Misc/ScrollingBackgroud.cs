using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgroud : MonoBehaviour
{
    public bool scrolling, parallex;

    public float backgroundSize;
    public float parallaxSpeed;

    Transform cameraTransfom;
    Transform[] layers;
    float viewZone = 10;
    int leftIndex;
    int rightIndex;
    float lastCameraX;

    Vector3 holdPositioning;

    void Start()
    {
        cameraTransfom = Camera.main.transform;
        lastCameraX = cameraTransfom.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    void Update()
    {
        if (parallex)
        {
            float deltaX = cameraTransfom.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
            lastCameraX = cameraTransfom.position.x;
        }

        if (scrolling)
        {
            if (cameraTransfom.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();   //when camera moves left, the background scroll will move left

            if (cameraTransfom.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();  //when camera moves right, the background scroll will move right
        }


    }

    void ScrollLeft()   //When camera moves left, the image on the right will be moved to the left.
    {
        //int lastRight = rightIndex;     //overrides lastRight int to rightIndex
        //layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);  //when moving too far right, take the image on the right and put it on the left

        holdPositioning = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        holdPositioning.y = layers[rightIndex].position.y;
        layers[rightIndex].position = holdPositioning;

        leftIndex = rightIndex;     //int of rightIndex equals int of leftIndex
        rightIndex--;   //subtract one from right index
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    void ScrollRight()  //When camera moves right, the image on the left will be moved to the right.
    {
        //int lastLeft = leftIndex;   //overides lastLeft int to leftindex
        //layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);  //when moving too far left, take the image on the left and put it on the right

        holdPositioning = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        holdPositioning.y = layers[leftIndex].position.y;
        layers[leftIndex].position = holdPositioning;

        rightIndex = leftIndex;     //int of leftIndex equals int of rightIndex
        leftIndex++;    //add one to the leftIndex
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
}
