using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Node
{
    public Dictionary<string, Node> neighbours = new Dictionary<string, Node>();
    public string id;
    public Node parent;
    public Vector2 position;
    public bool closed;
    public Waypoint waypoint;

    float _cost = 0f;
    public float cost
    {
        get { return _cost; }
    }

    float _heuristicCost = 0f;
    public float heuristicCost
    {
        get { return _heuristicCost; }
    }

    public float totalCost
    {
        get
        {
            return _cost + _heuristicCost;
        }
    }

    public void CalculateCost(Node from)
    {
        _cost = from.cost + Vector2.Distance(from.position, position);
    }

    public void CalculateHeuristicCost(Node end)
    {
        _heuristicCost = Vector2.Distance(position, end.position);
    }
}

public static class AStar
{
    public static List<Waypoint> FindPath(Waypoint start, Waypoint end)
    {
        Dictionary<string, Node> open = new Dictionary<string, Node>();
        List<Waypoint> path = new List<Waypoint>();
        
        Dictionary<string, Node> allNodes = CopyGraph(GameController.instance.GetAllWaypoints(), start);
        Node startNode = allNodes[start.id];
        Node endNode = allNodes[end.id];

        open.Add(startNode.id, startNode);
        while (open.Count > 0)
        {
            Node current = open.OrderBy(x => x.Value.totalCost).First().Value;
            if (current.id == endNode.id)
                break;

            open.Remove(current.id);

            foreach (Node neighbour in current.neighbours.Values)
            {
                if (neighbour.closed)
                    continue;

                neighbour.CalculateCost(current);
                neighbour.CalculateHeuristicCost(endNode);
                neighbour.parent = current;
                neighbour.closed = true;

                open.Add(neighbour.id, neighbour);
            }
        }

        if (endNode.totalCost == 0f)
			return null;
		
        Node currentPathNode = endNode;
        while (currentPathNode.id != startNode.id)
        {
            path.Add(currentPathNode.waypoint);
            currentPathNode = currentPathNode.parent;
        }

        path.Reverse();
        path.Add(endNode.waypoint);

        return path;
    }

    static Dictionary<string, Node> CopyGraph(Waypoint[] graph, Waypoint start)
    {
        Queue<Waypoint> queue = new Queue<Waypoint>();
        Dictionary<string, Node> createdNodes = new Dictionary<string, Node>();

        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Waypoint current = queue.Dequeue();
            Node currentNode = null;

            if (createdNodes.ContainsKey(current.id))
                currentNode = createdNodes[current.id];
            else
            {
                currentNode = new Node();
                currentNode.position = (Vector2)current.transform.position;
                currentNode.id = current.id;
                currentNode.waypoint = current;
                createdNodes.Add(currentNode.id, currentNode);
            }

            foreach (GameObject neighbour in current.neighbours)
            {
                Waypoint neighbourWaypoint = neighbour.GetComponent<Waypoint>();

                if (!createdNodes.ContainsKey(neighbourWaypoint.id))
                {
                    Node node = new Node();
                    node.position = (Vector2)neighbour.transform.position;
                    node.id = neighbourWaypoint.id;
                    node.waypoint = neighbourWaypoint;

                    queue.Enqueue(neighbourWaypoint);
                    createdNodes.Add(node.id, node);
                }

                if (!currentNode.neighbours.ContainsKey(neighbourWaypoint.id))
                    currentNode.neighbours.Add(neighbourWaypoint.id, createdNodes[neighbourWaypoint.id]);
            }
        }

        return createdNodes;
    }
}