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
        Root newRoot = new Root(transform.position, transform.rotation);

        foreach(Root root in rootList)
        {
            if (root.position == newRoot.position || root.rotation == newRoot.rotation)
            {
                return;
            }
        }

        rootList.Add(newRoot);
    }

    public void ClearRootList()
    {
        rootList.Clear();
        UpdateRootList();
    }
}

[System.Serializable]
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
