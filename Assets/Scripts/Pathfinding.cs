using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	private GameObject[] noeuds;

	public List<Transform> pathfinding(Transform depart, Transform arrivee)
	{
		noeuds = GameObject.FindGameObjectsWithTag("Noeud");
		List<Transform> chemin = new List<Transform>();

		Transform noeud = dijkstra(depart, arrivee);

		while (noeud != null)
		{
			chemin.Add(noeud);
			Noeud courrant = noeud.GetComponent<Noeud>();
			noeud = courrant.getParent();
		}
		chemin.Reverse();
		return chemin;
	}

	private Transform dijkstra(Transform start, Transform end)
	{
		// Nodes that are unexplored
		List<Transform> unexplored = new List<Transform>();

		// We add all the nodes we found into unexplored.
		foreach (GameObject obj in noeuds)
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
		return end;
	}

}
