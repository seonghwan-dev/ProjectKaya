using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
	{
		var component = gameObject.GetComponent<T>();
		if (component == null)
		{
			component = gameObject.AddComponent<T>();
		}

		return component;
	}

	public static void SetLayerRecursive(this GameObject gameObject, int layer)
	{
		gameObject.layer = layer;

		int childCount = gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			var child = gameObject.transform.GetChild(i);
			child.gameObject.SetLayerRecursive(layer);
		}
	}

	public static void SetLayerRecursive(this Component component, int layer)
	{
		component.gameObject.SetLayerRecursive(layer);
	}

	public static void Shuffle<T>(this List<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
				
			int k = Random.Range(0, n + 1);
				
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static Transform FindRecursive(this Transform transform, string name)
	{
		var found = transform.Find(name);
		if (found == null)
		{
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				found = transform.GetChild(i).FindRecursive(name);

				if (found != null)
				{
					break;
				}
			}
		}

		return found;
	}
}