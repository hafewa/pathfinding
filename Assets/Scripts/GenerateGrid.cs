using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
	public int ligne = 10;
	public int colonne = 10;
	public Transform noeud;

	public List<Transform> grille = new List<Transform>();

	void Start()
	{
		this.genererGrille();
		this.genererVoisins();
	}

	private void genererGrille()
	{
		for (int i = 0; i < ligne; i++)
			for (int j = 0, k = 0; j < colonne; j++, k++)
			{
				Transform n = Instantiate(noeud, new Vector3(j + gameObject.transform.position.x, gameObject.transform.position.y, i + gameObject.transform.position.z), Quaternion.identity);
				n.name = "noeud (" + k + ")";
				grille.Add(n);
			}
	}

	private void genererVoisins()
	{
		for (int i = 0, j = 1; i < grille.Count; i++, j++)
		{
			Noeud n = grille[i].GetComponent<Noeud>();
			if (j % colonne == 0) // S'il n'y a pas de noeuds à droite
			{
				if (i + colonne < colonne * ligne) n.addVoisin(grille[i + colonne]); // Noeud du haut
				if (i - colonne >= 0) n.addVoisin(grille[i - colonne]); // Noeud du bas
				n.addVoisin(grille[i - 1]); // Noeud de gauche
			}
			else if (j % colonne == 1) // S'il n'y a pas de noeuds à gauche
			{
				if (i + colonne < colonne * ligne) n.addVoisin(grille[i + colonne]); // Noeud du haut
				if (i - colonne >= 0) n.addVoisin(grille[i - colonne]); // Noeud du bas
				n.addVoisin(grille[i + 1]); // Noeud à droite
			}
			else
			{
				if (i + colonne < colonne * ligne) n.addVoisin(grille[i + colonne]); // Noeud du haut
				if (i - colonne >= 0) n.addVoisin(grille[i - colonne]); // Noeud du bas
				n.addVoisin(grille[i - 1]); // Noeud de gauche
				n.addVoisin(grille[i + 1]); // Noeud de droite
			}
		}
	}
}
