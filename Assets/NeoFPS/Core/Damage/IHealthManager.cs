using UnityEngine;
using System;

namespace NeoFPS
{
	public interface IHealthManager
	{
        event HealthDelegates.OnIsAliveChanged onIsAliveChanged;
        event HealthDelegates.OnHealthChanged onHealthChanged;
        event HealthDelegates.OnHealthMaxChanged onHealthMaxChanged;

		bool invincible { get; set; }
        bool isAlive { get; }
		float health { get; set; }
		float healthMax { get; set; }
		float normalisedHealth { get; set; }

		void AddDamage (float damage);
		void AddDamage (float damage, bool critical);
		void AddDamage (float damage, IDamageSource source);
		void AddDamage(float damage, bool critical, RaycastHit hit);
		void AddDamage (float damage, bool critical, IDamageSource source);
		void AddDamage(float damage, bool critical, IDamageSource source, RaycastHit hit);
		void AddHealth (float h);
		void AddHealth (float h, IDamageSource source);

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

    public static class HealthDelegates
    {
        public delegate void OnIsAliveChanged(bool alive);
        public delegate void OnHealthChanged(float from, float to, bool critical, IDamageSource source);
        public delegate void OnHealthMaxChanged(float from, float to);
    }
}