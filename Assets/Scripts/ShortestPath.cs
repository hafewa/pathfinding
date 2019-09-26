using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{

    private GameObject[] nodes;

    /// <summary>
    /// Finding the shortest path and return in a List
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>A List of transform for the shortest path</returns>
    public List<Transform> findShortestPath(Transform start, Transform end)
    {

        nodes = GameObject.FindGameObjectsWithTag("Noeud");

        List<Transform> result = new List<Transform>();
        Transform node = DijkstrasAlgo(start, end);

        // While there's still previous node, we will continue.
        while (node != null)
        {
            result.Add(node);
            Noeud currentNode = node.GetComponent<Noeud>();
            node = currentNode.getParent();
        }

        // Reverse the list so that it will be from start to end.
        result.Reverse();
        return result;
    }

    /// <summary>
    /// Dijkstra Algorithm to find the shortest path
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>The end node</returns>
    private Transform DijkstrasAlgo(Transform start, Transform end)
    {
        double startTime = Time.realtimeSinceStartup;

        // Nodes that are unexplored
        List<Transform> unexplored = new List<Transform>();

        // We add all the nodes we found into unexplored.
        foreach (GameObject obj in nodes)
        {
            Noeud n = obj.GetComponent<Noeud>();
            if (n.isLibre())
            {
                n.reset();
                unexplored.Add(obj.transform);
            }
        }

        // Set the starting node weight to 0;
        Noeud startNode = start.GetComponent<Noeud>();
        startNode.setPoids(0);

        while (unexplored.Count > 0)
        {
            // Sort the explored by their weight in ascending order.
            unexplored.Sort((x, y) => x.GetComponent<Noeud>().getPoids().CompareTo(y.GetComponent<Noeud>().getPoids()));

            // Get the lowest weight in unexplored.
            Transform current = unexplored[0];

            // Note: This is used for games, as we just want to reduce compuation, better way will be implementing A*
            /*
            // If we reach the end node, we will stop.
            if(current == end)
            {   
                return end;
            }*/

            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            Noeud currentNode = current.GetComponent<Noeud>();
            List<Transform> neighbours = currentNode.getVoisins();
            foreach (Transform neighNode in neighbours)
            {
                Noeud node = neighNode.GetComponent<Noeud>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.isLibre())
                {
                    // Get the distance of the object.
                    float distance = Vector3.Distance(neighNode.position, current.position);
                    distance = currentNode.getPoids() + distance;

                    // If the added distance is less than the current weight.
                    if (distance < node.getPoids())
                    {
                        // We update the new distance as weight and update the new path now.
                        node.setPoids(distance);
                        node.setParent(current);
                    }
                }
            }

        }

        double endTime = (Time.realtimeSinceStartup - startTime);
        print("Compute time: " + endTime);

        print("Path completed!");

        return end;
    }

}
