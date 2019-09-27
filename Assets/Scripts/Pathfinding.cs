using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	private GameObject[] noeuds;

	public List<Transform> pathfinding(Transform depart, Transform arrivee, bool algo)
	{
		noeuds = GameObject.FindGameObjectsWithTag("Noeud");
		List<Transform> chemin = new List<Transform>();

		Transform noeud;
		Noeud courant;

		if (algo)
			noeud = dijkstra(depart, arrivee);
		else
			noeud = aEtoile(depart, arrivee);

		// Remonte le chemin grâce au parent de chaque noeud

		while (noeud != null)
		{
			chemin.Add(noeud);
			courant = noeud.GetComponent<Noeud>();
			noeud = courant.getParent();
		}

		// Inverse le chemin pour l'avoir dans l'ordre

		chemin.Reverse();
		return chemin;
	}

	private Transform dijkstra(Transform depart, Transform arrivee)
	{
		Debug.Log("Algorithme Dijkstra");

		List<Transform> inexplores = new List<Transform>();

		// Réinitialise chaque noeud et l'ajoute à la liste des noeuds inexplorés

		foreach (GameObject obj in noeuds)
		{
			Noeud n = obj.GetComponent<Noeud>();
			if (n.isLibre())
			{
				n.reset();
				inexplores.Add(obj.transform);
			}
		}

		// Met le poids du noeud de départ à zéro

		Noeud noeudDepart = depart.GetComponent<Noeud>();
		noeudDepart.setPoids(0);

		// Tant qu'il reste des noeuds dans la liste des inexplorés

		while (inexplores.Count > 0)
		{
			// On prend celui de poids minimum

			inexplores.Sort((x, y) => x.GetComponent<Noeud>().getPoids().CompareTo(y.GetComponent<Noeud>().getPoids()));
			Transform courant = inexplores[0];

			// Si c'est la fin on arrête

			if (courant == arrivee) return arrivee;

			// On l'enlève de la liste

			inexplores.Remove(courant);
			Noeud noeudCourant = courant.GetComponent<Noeud>();

			// On récupère les voisins et on calcul leur poids et leur parent en fonction de notre nouvelle position

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
			Noeud noeudVoisin;

			foreach (Transform voisin in voisins)
			{
				// Les ajoutes à la liste ouverte sous certaines conditions

				noeudVoisin = voisin.GetComponent<Noeud>();

				if (noeudVoisin.isLibre() && !listeOuverte.Contains(voisin) && !listeFermee.Contains(voisin))
				{
					float distance = Vector3.Distance(voisin.position, courant.position) + courant.GetComponent<Noeud>().getPoids();
					if (distance < noeudVoisin.getPoids())
					{
						noeudVoisin.setPoids(distance);
						noeudVoisin.setParent(courant);
					}
					listeOuverte.Add(voisin);
				}
			}
		}
		return arrivee;
	}

}
