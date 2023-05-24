using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    // Old Shit
    [SerializeField] private float distanceBetween = .2f;
    [SerializeField] private float speed = 280f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> rootBody = new List<GameObject>();

    float countUp = 0;
    [SerializeField] private float rootBodyOffset = 1f;
    bool movingHead = false;
    bool followingHead = true;

    // New Shit
    [SerializeField] private Vector3 startingCoordaintes = Vector3.zero;
    float distanceBetweenNodes = 1f;
    List<Node> nodes = new List<Node>();

    private void Start()
    {
        CreateBodyParts();
    }

    private void FixedUpdate()
    {
        if(bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        RootMovement();
    }

    private void CreateBodyParts()
    {
        if (rootBody.Count == 0)
        {
            CreatePart();
        }

        RootBodyManager rootBodyManager = rootBody[rootBody.Count - 1].GetComponent<RootBodyManager>();
        if (countUp == 0)
        {
            rootBodyManager.ClearRootList();
        }
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject temp = CreatePart();
            temp.GetComponent<RootBodyManager>().ClearRootList();
            countUp = 0;
        }
    }

    

    private GameObject CreatePart()
    {
        Node newNode = new Node(startingCoordaintes);

        // Need to make nodes after a specific unit of distance.
        // Need to assign the previous node value for every nod eafter the first. 
        // Need to create "brush stroke" from previous node to current node
        // Need to make "brush stroke" slowly appear from left to right in animation?

        GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
        if (!temp.GetComponent<RootBodyManager>())
        {
            temp.AddComponent<RootBodyManager>();
        }
        if (!temp.GetComponent<Rigidbody2D>())
        {
            temp.AddComponent<Rigidbody2D>();
            temp.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        rootBody.Add(temp);
        bodyParts.RemoveAt(0);
        return temp;
    }

    private void RootMovement()
    {
        HandleRootTipMovement();

        HandleRootBodyPieceMovement();
    }

    private void HandleRootTipMovement()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            rootBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal")));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            movingHead = true;
            followingHead = true;
            rootBody[0].GetComponent<Rigidbody2D>().velocity = rootBody[0].transform.up * speed * Time.deltaTime;
        }
        else
        {
            movingHead = false;
            rootBody[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void HandleRootBodyPieceMovement()
    {
        if (rootBody.Count > 1 && followingHead)
        {
            for (int index = 1; index < rootBody.Count; index++)
            {
                RootBodyManager rootBodyManager = rootBody[index - 1].GetComponent<RootBodyManager>();
                if (IsWithinOffset(rootBodyManager.rootList[0].position, rootBody[index - 1].transform.position))
                {
                    followingHead = false;
                    return;
                }

                rootBody[index].transform.position = rootBodyManager.rootList[0].position;
                rootBody[index].transform.rotation = rootBodyManager.rootList[0].rotation;
                rootBodyManager.rootList.RemoveAt(0);
            }
        }
    }

    private bool IsWithinOffset(Vector3 firstPosition, Vector3 secondPosition)
    {
        float distance = Vector3.Distance(firstPosition, secondPosition);
        return distance <= rootBodyOffset;
    }
}
