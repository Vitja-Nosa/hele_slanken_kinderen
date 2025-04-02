using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Transform transform;
    public float playerSpeed = 5f;
    private bool isMoving = false;
    public NodeManager nodeManager;
    public bool isInMenu = false;

    private void Start()
    { 
        Node node = nodeManager.currentNode;
        transform.position = new Vector3(node.transform.position.x, node.transform.position.y);
    }
    private void Update()
    {
        if (isInMenu) return;
        Vector2 direction = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isMoving) return;
            direction = Vector2.up;
        } 
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (isMoving) return;
            direction = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            if (isMoving) return;
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (isMoving) return;
            direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isMoving) return;
            SceneManager.LoadScene(nodeManager.currentNode.levelName);
        }

        if (direction != Vector2.zero)
        {
            KeyValuePair<Node, List<Vector2>>? targetNode = nodeManager.currentNode.FindTargetNodeWithPath(direction);
            if (targetNode.HasValue && !targetNode.Value.Key.locked)
            {
                isMoving = true;
                StartCoroutine(Move(targetNode.Value.Key, targetNode.Value.Value));
            }
        }
    }
    private IEnumerator Move(Node node, List<Vector2> path)
    {
        isMoving = true;
        foreach (Vector2 direction in path)
        {
            Vector3 targetPos = transform.position + new Vector3(direction.x, direction.y, 0);

            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

        }
        transform.position = new Vector3(node.transform.position.x, node.transform.position.y); // Snap to final position
        nodeManager.currentNode = node;
        StatusManager.Instance.currentNodeIndex = nodeManager.allNodes.IndexOf(node);
        isMoving = false;
    }

}
