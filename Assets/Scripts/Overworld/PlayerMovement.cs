using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform transform;
    private bool isMoving = false;

    public Node currentNode;

    private void Update()
    {
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

        if (direction != Vector2.zero)
        {
            KeyValuePair<Node, List<Vector2>>? targetNode = currentNode.FindTargetNodeWithPath(direction);
            if (targetNode.HasValue)
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
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

        }
        transform.position = new Vector3(node.transform.position.x, node.transform.position.y); // Snap to final position
        currentNode = node;
        isMoving = false;
    }

}
