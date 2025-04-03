using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeManager : MonoBehaviour
{

    public Node currentNode;

    public List<Node> allNodes;
    public ChildLevelCompletionApiClient childLevelCompletionApiClient;
    public ChildApiClient childApiClient;
    public PlayerMovement playerMovement;

    public TileManager tileManager;

    public async void Awake()
    {
        if (StatusManager.Instance.completedLevels.Count == 0)
        {
            await SetChildTreatmentPath();
            Debug.Log(StatusManager.Instance.treatmentPath);
        }
        ConnectNodes();
        Debug.Log(allNodes.Count);
        //alleen eerste keer inladen
        if (StatusManager.Instance.completedLevels.Count == 0)
        {
            await GetCompletedLevels();
        }
        foreach (int completedLevel in StatusManager.Instance.completedLevels)
        {
            allNodes[completedLevel].locked = false;
            tileManager.NodeCompleted(allNodes[completedLevel - 1]);
        }

        // Set the not selected path locked
        //if (StatusManager.Instance.treatmentPath == "B")
        //{
        //    for (int i = 6; i <= 11; i++)
        //    {
        //        allNodes[i].locked = true;
        //    }
        //}
        //else if (StatusManager.Instance.treatmentPath == "A")
        //{
        //    allNodes.RemoveRange(11, 6);
        //    for (int i = 11; i <= 17; i++)
        //    {
        //        allNodes[i].locked = true;
        //    }
        //}

        currentNode = allNodes[StatusManager.Instance.currentNodeIndex];
        playerMovement.SetupPlayer();

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

    public void ConnectNodes()
    {
        // Connection level 1 -> level 2
        allNodes[0].Connect(allNodes[1], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 7 ),
        }));

        // Connection level 2 -> level 3
        allNodes[1].Connect(allNodes[2], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.down, 7 )
        }));

        // Connection level 3 -> level 4
        allNodes[2].Connect(allNodes[3], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 7 ),
            new KeyValuePair<Vector2, int>( Vector2.up, 5 )
        }));


        // Connection level 4 -> level 5
        allNodes[3].Connect(allNodes[4], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 5 ),
            new KeyValuePair<Vector2, int>( Vector2.down, 5 ),
            new KeyValuePair<Vector2, int>( Vector2.right, 4 ),
        }));

        // Connection level 5 -> level 6
        allNodes[4].Connect(allNodes[5], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.up, 3 ),
            new KeyValuePair<Vector2, int>( Vector2.right, 3 ),
            new KeyValuePair<Vector2, int>( Vector2.up, 4 ),
        }));

        if (StatusManager.Instance.treatmentPath == "A")
        {
            // >>>>>>>> ROUTE A >>>>>>>>>>>>>>>>

            // Connection level 6 -> level 7
            allNodes[5].Connect(allNodes[6], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.up, 10 ),
            }));

            // Connection level 7 -> level 8
            allNodes[6].Connect(allNodes[7], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.up, 7),
                new KeyValuePair<Vector2, int>( Vector2.right, 4),
            }));

            // Connection level 8 -> level 9
            allNodes[7].Connect(allNodes[8], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.down, 6),
                new KeyValuePair<Vector2, int>( Vector2.right, 5),
            }));


            // Connection level 9 -> level 10
            allNodes[8].Connect(allNodes[9], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 3),
                new KeyValuePair<Vector2, int>( Vector2.up, 6),
                new KeyValuePair<Vector2, int>( Vector2.right, 4),
            }));

            // Connection level 10 -> level 11
            allNodes[9].Connect(allNodes[10], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 4),
                new KeyValuePair<Vector2, int>( Vector2.down, 5),
            }));

            // Connection level 11 -> level 18
            allNodes[10].Connect(allNodes[17], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 7),
                new KeyValuePair<Vector2, int>( Vector2.up, 3),
                new KeyValuePair<Vector2, int>( Vector2.right, 3),
            }));


            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ////////
        }


        else if (StatusManager.Instance.treatmentPath == "B")
        {
            // >>>>>>>> ROUTE B >>>>>>>>>>>>>>>>

            // Connection level 6 -> level 12
            allNodes[5].Connect(allNodes[11], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 9 ),
            }));

            // Connection level 12 -> level 13
            allNodes[11].Connect(allNodes[12], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 4 ),
                new KeyValuePair<Vector2, int>( Vector2.down, 7 ),
            }));

            // Connection level 13 -> level 14
            allNodes[12].Connect(allNodes[13], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 4 ),
                new KeyValuePair<Vector2, int>( Vector2.up, 7 ),
            }));

            // Connection level 14 -> level 15
            allNodes[13].Connect(allNodes[14], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 4 ),
                new KeyValuePair<Vector2, int>( Vector2.down, 6 ),
            }));

            // Connection level 15 -> level 16
            allNodes[14].Connect(allNodes[15], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.right, 6 ),
                new KeyValuePair<Vector2, int>( Vector2.up, 4 ),
            }));

            // Connection level 16 -> level 17
            allNodes[15].Connect(allNodes[16], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.up, 6 ),
            }));

            // Connection level 17 -> level 18
            allNodes[16].Connect(allNodes[17], MakePath(new List<KeyValuePair<Vector2, int>>()
            {
                new KeyValuePair<Vector2, int>( Vector2.up, 11 ),
                new KeyValuePair<Vector2, int>( Vector2.right, 3),
            }));
            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ////////
        }

        // Connection level 18 -> level 19
        allNodes[17].Connect(allNodes[18], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.up, 7),
        }));

        // Connection level 19 -> level 20
        allNodes[18].Connect(allNodes[19], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 3 ),
            new KeyValuePair<Vector2, int>( Vector2.down, 2 ),
            new KeyValuePair<Vector2, int>( Vector2.right, 3 ),
        }));

        // Connection level 20 -> level 21
        allNodes[19].Connect(allNodes[20], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.down, 6 ),
            new KeyValuePair<Vector2, int>( Vector2.right, 3 ),
        }));

        // Connection level 21 -> level 22
        allNodes[20].Connect(allNodes[21], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.up, 3 ),
            new KeyValuePair<Vector2, int>( Vector2.right, 3 ),
            new KeyValuePair<Vector2, int>( Vector2.up, 5 ),
        }));

        // Connection level 22 -> level 23
        allNodes[21].Connect(allNodes[22], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 3 ),
            new KeyValuePair<Vector2, int>( Vector2.down, 9 ),
        }));

        // Connection level 23 -> level 24
        allNodes[22].Connect(allNodes[23], MakePath(new List<KeyValuePair<Vector2, int>>()
        {
            new KeyValuePair<Vector2, int>( Vector2.right, 4 ),
            new KeyValuePair<Vector2, int>( Vector2.up, 6 ),
        }));

        //if (StatusManager.Instance.treatmentPath == "B")
        //    allNodes.RemoveRange(6, 5);
        //else if (StatusManager.Instance.treatmentPath == "A")
        //    allNodes.RemoveRange(11, 6);
    }

    public async Task GetCompletedLevels()
    {
        List<int> completedLevels = new();
        IWebRequestReponse result = await childLevelCompletionApiClient.GetAllLevelCompletions();
        if (result is WebRequestData<List<ChildLevelCompletion>> data)
        {
            var completions = data.Data;
            foreach (ChildLevelCompletion completion in completions)
            {
                completedLevels.Add(completion.levelId);
            }
            StatusManager.Instance.CompleteLevel(completedLevels);

            // TODO: verbeter dit, beide statements in 1 method voor hergebruik
            StatusManager.Instance.currentNodeIndex = completedLevels.Max();
            // ------------
        }
        else
        {
            Debug.LogError("Server error");
        }
    }

    public async Task SetChildTreatmentPath()
    {
        IWebRequestReponse result = await childApiClient.GetChild();
        if (result is WebRequestData<Child> data)
        {
            var child = data.Data;
            StatusManager.Instance.treatmentPath = child.treatmentPath;
        }
        else
        {
            Debug.LogError("Server Error");
        }
    }
}
