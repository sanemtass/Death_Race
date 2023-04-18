using UnityEngine;

namespace UmbrellaUtils.Pool.Factory
{
	public class MonoBehaviourFactory<T> : IFactory<T> where T : MonoBehaviour 
	{
		private string name;
		private int index = 0;
		
		public MonoBehaviourFactory(string name = "GameObject") 
		{
			this.name = name;
		}

		public T Create()
		{
			GameObject tempGameObject = new GameObject
			{
				name = name + index
			};
			T objectOfType = tempGameObject.AddComponent<T>();
			index++;
			return objectOfType;
		}
	}
}