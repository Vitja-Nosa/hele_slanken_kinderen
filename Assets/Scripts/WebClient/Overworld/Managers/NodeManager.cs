using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    void Awake()
    {
        // If there's already an instance, destroy the new one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Set the instance to this object
            Instance = this;
        }
    }

    public List<Node> allNodes;

    void Start()
    {
        // Connection level 1 -> level 2
        allNodes[0].Connect(allNodes[1], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 7 ),
            new KeyValuePair<Vector2, int>( Vector2.down, 4 ),
            new KeyValuePair<Vector2, int>( Vector2.right, 4 )
        }));

        // Connection level 2 -> level 3
        allNodes[1].Connect(allNodes[2], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 9 )
        }));

        // Connection level 3 -> level 4
        allNodes[2].Connect(allNodes[3], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 6 ),
            new KeyValuePair<Vector2, int>( Vector2.up, 4 )
        }));


    }

    public List<Vector2> MakePath(List<KeyValuePair<Vector2, int>> template)
    {
        List<Vector2> path = new List<Vector2>();

        foreach (KeyValuePair<Vector2, int> direction in template)
        {
            for (int i = 0; i < direction.Value; i++)
            {
                path.Add(direction.Key);
            }
        }
        
        return path;
    }
}
