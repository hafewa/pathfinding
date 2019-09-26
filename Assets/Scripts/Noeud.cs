using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noeud : MonoBehaviour
{
	[SerializeField] private float poids = int.MaxValue;
	[SerializeField] private Transform parent = null;
	[SerializeField] private List<Transform> voisins;
	[SerializeField] private bool libre = true;

	void Start()
	{
		this.reset();
	}

	public void reset()
	{
		parent = null;
		poids = int.MaxValue;
	}

	public void setParent(Transform n)
	{
		this.parent = n;
	}

	public void setPoids(float p)
	{
		this.poids = p;
	}

	public void setLibre(bool b)
	{
		this.libre = b;
	}

	public void addVoisin(Transform n)
	{
		this.voisins.Add(n);
	}

	public List<Transform> getVoisins()
	{
		return this.voisins;
	}

	public float getPoids()
	{
		return this.poids;
	}

	public Transform getParent()
	{
		return this.parent;
	}

	public bool isLibre()
	{
		return this.libre;
	}
}
