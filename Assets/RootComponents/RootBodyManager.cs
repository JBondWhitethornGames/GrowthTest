using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBodyManager : MonoBehaviour
{
    public List<Root> rootList = new List<Root>();

    private void FixedUpdate()
    {
        UpdateRootList();
    }

    public void UpdateRootList()
    {
        rootList.Add(new Root(transform.position, transform.rotation));
    }

    public void ClearRootList()
    {
        rootList.Clear();
        //rootList.Add(new Root(transform.position, transform.rotation));
        UpdateRootList();
    }
}

public class Root
{
    public Vector3 position;
    public Quaternion rotation;

    public Root(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
