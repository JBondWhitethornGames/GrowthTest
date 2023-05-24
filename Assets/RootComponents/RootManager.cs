using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class RootManager : MonoBehaviour
{
    [SerializeField] private float distanceBetween = .2f;
    [SerializeField] private float speed = 280f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> rootBody = new List<GameObject>();

    float countUp = 0;
    bool movingHead = false;

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

    private void RootMovement()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            rootBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal")));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            movingHead = true;
            rootBody[0].GetComponent<Rigidbody2D>().velocity = rootBody[0].transform.up * speed * Time.deltaTime;
        }
        else
        {
            movingHead = false;
            StopAllMovement();
            return;
        }
        

        if(rootBody.Count > 1)
        {
            for(int index = 1; index < rootBody.Count; index++)
            {
                RootBodyManager rootBodyManager = rootBody[index -1].GetComponent<RootBodyManager>();
                rootBody[index].transform.position = rootBodyManager.rootList[0].position;
                rootBody[index].transform.rotation = rootBodyManager.rootList[0].rotation;
                rootBodyManager.rootList.RemoveAt(0);
            }
        }
    }

    private void StopAllMovement()
    {
        for (int index = 0; index < rootBody.Count; index++)
        {
            rootBody[index].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void CreateBodyParts()
    {
        if(rootBody.Count == 0)
        {
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
        }

        RootBodyManager rootBodyManager = rootBody[rootBody.Count - 1].GetComponent<RootBodyManager>();
        if(countUp == 0)
        {
            rootBodyManager.ClearRootList();
        }
        countUp += Time.deltaTime;
        if(countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], rootBodyManager.rootList[0].position, rootBodyManager.rootList[0].rotation, transform);
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
            temp.GetComponent<RootBodyManager>().ClearRootList();
            countUp = 0;
        }
    }
}
