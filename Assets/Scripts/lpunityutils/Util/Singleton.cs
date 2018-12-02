// Copyright Olli Etuaho 2018.

using UnityEngine;

namespace LPUnityUtils {

/// <summary>
/// Singleton. To use this, inherit from Singleton&lt;T&gt; from your class "T" and then do:
/// if (!EnforceSingleton(false)) { return; } at the beginning of your Awake() function.
/// </summary>
public class Singleton<T> : MonoBehaviour
{
	public static T instance { get; private set; } //Static instance of the singleton which allows it to be accessed by any other script.
	
	// When inheriting, make sure to call this at the start of Awake!
	// Returns false if trying to initialize more than one singleton.
	protected bool EnforceSingleton(bool destroyOnLoad)
	{
		if (instance == null) {
			instance = (T)(object)this;
		} else {
			Destroy (gameObject);
			return false;
		}
		
		if (!destroyOnLoad) {
			DontDestroyOnLoad (gameObject);
		}
		return true;
	}
}

}  // namespace LPUnityUtils
