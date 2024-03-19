using System;
using UnityEngine;

namespace NeoFPS
{
	public interface IDamageHandler
	{
		DamageFilter inDamageFilter 
		{
			get;
			set;
		}

		IHealthManager healthManager
        {
			get;
        }

		DamageResult AddDamage(float damage);
        DamageResult AddDamage(float damage, RaycastHit hit);
        DamageResult AddDamage(float damage, IDamageSource source);
        DamageResult AddDamage(float damage, RaycastHit hit, IDamageSource source);

		// Add monobehaviour methods to remove need for casting if required
		GameObject gameObject { get; }
		Transform transform { get; }
		T GetComponent<T>();
		T GetComponentInChildren<T>();
		T GetComponentInParent<T>();
		T[] GetComponents<T>();
		T[] GetComponentsInChildren<T>(bool includeInactive = false);
		T[] GetComponentsInParent<T>(bool includeInactive = false);
		Component GetComponent(Type t);
		Component GetComponentInChildren(Type t);
		Component GetComponentInParent(Type t);
		Component[] GetComponents(Type t);
		Component[] GetComponentsInChildren(Type t, bool includeInactive = false);
		Component[] GetComponentsInParent(Type t, bool includeInactive = false);
	}

	public enum DamageResult
	{
		Standard,
		Critical,
		Ignored,
        Blocked
	}
}