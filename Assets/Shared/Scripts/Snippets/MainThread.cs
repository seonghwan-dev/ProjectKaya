using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class MainThread : MonoBehaviour
{
	public const int QUEUE_CAPACITY = 32;
	
	private static MainThread instance;
	private static Thread unityMainThread;
	
	private readonly Queue<Action> actions = new Queue<Action>(QUEUE_CAPACITY);

#if UNITY_EDITOR
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void OnDomainReset()
	{
		Clear();
	}
#endif

	private static void Clear()
	{
		instance = default(MainThread);
		unityMainThread = default(Thread);
	}
	
	public static void Setup(bool visible = true)
	{
		// Avoid creating multiple instances.
		if (instance != null)
		{
			Debug.LogError("MainThread already created");
			return;
		}
		
		// Create the instance.
		var container = new GameObject("[MainThread]", typeof(MainThread));
		if (!visible)
		{
			container.hideFlags = HideFlags.HideAndDontSave;
		}
		
		DontDestroyOnLoad(container);
		instance = container.GetComponent<MainThread>();
	}

	private void Awake()
	{
		// Cache the main thread.
		unityMainThread = Thread.CurrentThread;
	}

	/// <summary>
	/// Consumes queued actions on main thread.
	/// </summary>
	private void Update()
	{
		lock (actions)
		{
			while (actions.Count > 0)
			{
				actions.Dequeue().Invoke();
			}
		}
	}

	private void OnDestroy()
	{
		Clear();
	}

	/// <summary>
	/// Executes method if it is called from main thread.
	/// If not, it will be added to the queue.
	/// </summary>
	/// <param name="action"></param>
	public static void ExecuteIfBound(Action action)
	{
		Assert.IsNotNull(action);
		
		if (IsBound())
		{
			action.Invoke();
		}
		else
		{
			lock (instance.actions)
			{
				if (instance.actions.Count < QUEUE_CAPACITY)
				{
					instance.actions.Enqueue(action);
				}
				else
				{
					Debug.LogWarning("[MainThread] Queue is full, dropping action");
				}
			}
		}
	}

	/// <summary>
	/// Add action to the queue for execution on main thread.
	/// This will not be validated whether it is called from main thread or not.
	/// </summary>
	/// <param name="action"></param>
	public static void Dispatch(Action action)
	{
		Assert.IsNotNull(action);
		
		lock (instance.actions)
		{
			if (instance.actions.Count < QUEUE_CAPACITY)
			{
				instance.actions.Enqueue(action);
			}
			else
			{
				Debug.LogWarning("[MainThread] Queue is full, dropping action");
			}
		}
	}
	
	/// <summary>
	/// Returns true if the current thread is the main thread.
	/// </summary>
	/// <returns></returns>
	public static bool IsBound()
	{
		return Thread.CurrentThread == unityMainThread;
	}
}