using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string levelName;
    public Transform transform;

    public bool _locked;
    public bool locked
    {

        get => _locked;
        set
        {
            _locked = value;
            ToggleLock();
        }
    }

    public Vector2 position { get; private set; }
    public Dictionary<Node, List<Vector2>> paths { get; private set; }

    public Node()
    {
        paths = new Dictionary<Node, List<Vector2>>();
    }

    public void Connect(Node targetNode, List<Vector2> path)
    {
        if (!paths.ContainsKey(targetNode))
        {
            paths[targetNode] = new List<Vector2>(path);
            targetNode.paths[this] = new List<Vector2>(InvertPath(path));
        }
    }
    public List<Vector2> InvertPath(List<Vector2> path)
    {
        List<Vector2> invertedList = new List<Vector2>();

        foreach (Vector2 vector in path)
        {
            invertedList.Add(new Vector2(-vector.x, -vector.y));
        }
        invertedList.Reverse();

        return invertedList;
    }

    public KeyValuePair<Node, List<Vector2>>? FindTargetNodeWithPath(Vector2 direction)
    {
        foreach (KeyValuePair<Node, List<Vector2>> path in paths)
        {
            if (path.Value[0] == direction)
            {
                return path;
            }
        }
        return null; 
    }

    private void OnValidate()
    {
        ToggleLock();
    }

    public void ToggleLock()
    {
        this.GetComponent<SpriteRenderer>().enabled = _locked;
    }
}
