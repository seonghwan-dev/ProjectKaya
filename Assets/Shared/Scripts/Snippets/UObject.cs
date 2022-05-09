using UnityEngine;

public class UObject
{
	public UObject()
	{
		MainThread.Dispatch(Create);

		void Create()
		{
			this.owner = new GameObject();
			this.objectBase = this.owner.AddComponent<UObjectBase>();
		}
	}

	private GameObject owner;
	private UObjectBase objectBase;

	public GameObject Get()
	{
		return this.owner;
	}
}