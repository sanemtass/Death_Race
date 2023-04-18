using UnityEngine;

namespace UmbrellaUtils.Pool.Factory
{
	public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour 
	{
		private GameObject prefab;
		private Transform parent;
		private string name;
		private int index;

		public PrefabFactory(GameObject prefab, Transform parent) : this(prefab, parent, prefab.name) { }

		public PrefabFactory(GameObject prefab, Transform parent, string name) 
		{
			this.prefab = prefab;
			this.parent = parent;
			this.name = name;
		}

		public T Create()
		{
			GameObject tempGameObject = Object.Instantiate(prefab);
			tempGameObject.name = name + index;
			tempGameObject.transform.SetParent(parent);
			var objectOfType = tempGameObject.GetComponent<T>();
			index++;
			tempGameObject.SetActive(false);
			return objectOfType;
		}
	}
}