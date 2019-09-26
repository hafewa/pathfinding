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
			Noeud courant = noeud.GetComponent<Noeud>();
			noeud = courant.getParent();
		}
		chemin.Reverse();
		return chemin;
	}

	private Transform dijkstra(Transform depart, Transform arrivee)
	{
		List<Transform> inexplores = new List<Transform>();
		
		foreach (GameObject obj in noeuds)
		{
			Noeud n = obj.GetComponent<Noeud>();
			if (n.isLibre())
			{
				n.reset();
				inexplores.Add(obj.transform);
			}
		}

		Noeud noeudDepart = depart.GetComponent<Noeud>();
		noeudDepart.setPoids(0);

		while (inexplores.Count > 0)
		{
			inexplores.Sort((x, y) => x.GetComponent<Noeud>().getPoids().CompareTo(y.GetComponent<Noeud>().getPoids()));
			Transform courant = inexplores[0];

			if (courant == arrivee) return arrivee;

			inexplores.Remove(courant);
			Noeud noeudCourant = courant.GetComponent<Noeud>();
			List<Transform> voisins = noeudCourant.getVoisins();

			foreach (Transform noeudVoisin in voisins)
			{
				Noeud noeud = noeudVoisin.GetComponent<Noeud>();
				if (inexplores.Contains(noeudVoisin) && noeud.isLibre())
				{
					float distance = Vector3.Distance(noeudVoisin.position, courant.position) + noeudCourant.getPoids();
					if (distance < noeud.getPoids())
					{
						noeud.setPoids(distance);
						noeud.setParent(courant);
					}
				}
			}
		}
		return arrivee;
	}

	// private Transform aStar(Transform depart, Transform arrivee)
	// {

	// }

}
