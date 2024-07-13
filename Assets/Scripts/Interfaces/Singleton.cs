using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
	private static T _instance;

	public static T Instance {
		get {
			if (_instance == null) {
				// Find existing instance
				_instance = FindObjectOfType<T>();

				// Create new instance if one doesn't already exist
				if (_instance == null) {
					var singletonObject = new GameObject();
					_instance = singletonObject.AddComponent<T>();
					singletonObject.name = typeof(T).ToString() + " (Singleton)";

					// Make instance persistent
					DontDestroyOnLoad(singletonObject);
				}
			}

			return _instance;
		}
	}

	protected Singleton() { }

	public virtual void Initialize() { }
}
