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

		Transform noeud = aEtoile(depart, arrivee);

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
		Debug.Log("Algorithme Dijkstra");
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

	private Transform aEtoile(Transform depart, Transform arrivee)
	{
		Debug.Log("Algorithme A*");
		foreach (GameObject obj in noeuds)
		{
			Noeud n = obj.GetComponent<Noeud>();
			if (n.isLibre()) n.reset();
		}

		List<Transform> listeOuverte = new List<Transform>();
		List<Transform> listeFermee = new List<Transform>();

		// Ajoute le départ à la liste ouverte

		depart.GetComponent<Noeud>().setPoids(0);
		listeOuverte.Add(depart);

		while (listeOuverte.Count > 0)
		{
			// Choisis le noeud de poids minimum dans la liste ouverte

			listeOuverte.Sort((x, y) => x.GetComponent<Noeud>().getPoids().CompareTo(y.GetComponent<Noeud>().getPoids()));
			Transform courant = listeOuverte[0];

			// Si c'est l'arrivée on a fini

			if (courant == arrivee) return arrivee;

			// Sinon déplace le noeud de la liste ouverte à la liste fermée

			listeFermee.Add(courant);
			listeOuverte.Remove(courant);

			// Récupère les voisins du noeud

			List<Transform> voisins = courant.GetComponent<Noeud>().getVoisins();
			for (int i = 0; i < voisins.Count; i++)
			{
				// Les ajoutes à la liste ouverte sous certaines conditions

				if (voisins[i].GetComponent<Noeud>().isLibre() && !listeOuverte.Contains(voisins[i]) && !listeFermee.Contains(voisins[i]))
				{
					float distance = Vector3.Distance(voisins[i].position, courant.position) + courant.GetComponent<Noeud>().getPoids();
					if (distance < voisins[i].GetComponent<Noeud>().getPoids())
					{
						voisins[i].GetComponent<Noeud>().setPoids(distance);
						voisins[i].GetComponent<Noeud>().setParent(courant);
					}
					listeOuverte.Add(voisins[i]);
				}
			}
		}
		return arrivee;
	}

}
