using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
	private Transform noeud;
	private Transform noeudDepart;
	private Transform noeudArrivee;
	private List<Transform> obstacles = new List<Transform>();

	void Update()
	{
		inputSouris();
	}

	private void inputSouris()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.couleurObstacles();
			this.updateCouleurNoeud();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Noeud")
			{
				Renderer rend;
				if (noeud != null)
				{
					rend = noeud.GetComponent<Renderer>();
					rend.material.color = Color.white;
				}
				noeud = hit.transform;
				rend = noeud.GetComponent<Renderer>();
				rend.material.color = Color.grey;
			}
		}
	}

	public void btnNoeudDepart()
	{
		if (noeud != null)
		{
			Noeud n = noeud.GetComponent<Noeud>();
			if (n.isLibre())
			{
				if (noeudDepart == null)
				{
					Renderer rend = noeud.GetComponent<Renderer>();
					rend.material.color = Color.red;
				}
				else
				{
					Renderer rend = noeudDepart.GetComponent<Renderer>();
					rend.material.color = Color.white;
					rend = noeud.GetComponent<Renderer>();
					rend.material.color = Color.red;
				}
				noeudDepart = noeud;
				noeud = null;
			}
		}
	}

	public void btnNoeudArrivee()
	{
		if (noeud != null)
		{
			Noeud n = noeud.GetComponent<Noeud>();
			if (n.isLibre())
			{
				if (noeudArrivee == null)
				{
					Renderer rend = noeud.GetComponent<Renderer>();
					rend.material.color = Color.yellow;
				}
				else
				{
					Renderer rend = noeudArrivee.GetComponent<Renderer>();
					rend.material.color = Color.white;
					rend = noeud.GetComponent<Renderer>();
					rend.material.color = Color.yellow;
				}
				noeudArrivee = noeud;
				noeud = null;
			}
		}
	}

	public void btnDijkstra()
	{
		if (noeudDepart != null && noeudArrivee != null)
		{
			Pathfinding finder = gameObject.GetComponent<Pathfinding>();
			List<Transform> paths = finder.pathfinding(noeudDepart, noeudArrivee, true);
			foreach (Transform path in paths)
			{
				Renderer rend = path.GetComponent<Renderer>();
				rend.material.color = Color.green;
			}
		}
	}

	public void btnAEtoile()
	{
		if (noeudDepart != null && noeudArrivee != null)
		{
			Pathfinding finder = gameObject.GetComponent<Pathfinding>();
			List<Transform> paths = finder.pathfinding(noeudDepart, noeudArrivee, false);
			foreach (Transform path in paths)
			{
				Renderer rend = path.GetComponent<Renderer>();
				rend.material.color = Color.green;
			}
		}
	}

	public void btnObstacle()
	{
		if (noeud != null)
		{
			Renderer rend = noeud.GetComponent<Renderer>();
			rend.material.color = Color.black;
			Noeud n = noeud.GetComponent<Noeud>();
			n.setLibre(false);
			obstacles.Add(noeud);
			if (noeud == noeudDepart)
				noeudDepart = null;
			if (noeud == noeudArrivee)
				noeudArrivee = null;
			noeud = null;
		}
		UnitSelectionComponent selection = gameObject.GetComponent<UnitSelectionComponent>();
		List<Transform> selected = selection.getSelectedObjects();
		foreach (Transform nd in selected)
		{
			Renderer rend = nd.GetComponent<Renderer>();
			rend.material.color = Color.black;
			Noeud n = nd.GetComponent<Noeud>();
			n.setLibre(false);
			obstacles.Add(nd);
			if (nd == noeudDepart)
				noeudDepart = null;
			if (nd == noeudArrivee)
				noeudArrivee = null;
		}
		selection.clearSelections();
	}

	public void btnEnleveObstacle()
	{
		if (noeud != null)
		{
			Renderer rend = noeud.GetComponent<Renderer>();
			rend.material.color = Color.white;
			Noeud n = noeud.GetComponent<Noeud>();
			n.setLibre(true);
			obstacles.Remove(noeud);
			noeud = null;
		}
		UnitSelectionComponent selection = gameObject.GetComponent<UnitSelectionComponent>();
		List<Transform> selected = selection.getSelectedObjects();
		foreach (Transform nd in selected)
		{
			Renderer rend = nd.GetComponent<Renderer>();
			rend.material.color = Color.white;
			Noeud n = nd.GetComponent<Noeud>();
			n.setLibre(true);
			obstacles.Remove(nd);
		}
		selection.clearSelections();
	}

	public void btnReset()
	{
		Scene loadedLevel = SceneManager.GetActiveScene();
		SceneManager.LoadScene(loadedLevel.buildIndex);
	}

	private void couleurObstacles()
	{
		foreach (Transform block in obstacles)
		{
			Renderer rend = block.GetComponent<Renderer>();
			rend.material.color = Color.black;
		}
	}

	private void updateCouleurNoeud()
	{
		if (noeudDepart != null)
		{
			Renderer rend = noeudDepart.GetComponent<Renderer>();
			rend.material.color = Color.red;
		}

		if (noeudArrivee != null)
		{
			Renderer rend = noeudArrivee.GetComponent<Renderer>();
			rend.material.color = Color.yellow;
		}
	}

}
