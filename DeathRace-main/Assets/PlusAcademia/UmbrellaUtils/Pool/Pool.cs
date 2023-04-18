using System.Collections;
using System.Collections.Generic;
using UmbrellaUtils.Pool.Factory;

namespace UmbrellaUtils.Pool 
{
	public class Pool<T> : IEnumerable where T : IResettable
	{
		private readonly List<T> members = new List<T>();
		private readonly HashSet<T> unavailable = new HashSet<T>();
		private readonly IFactory<T> factory;

		public Pool(IFactory<T> factory, int poolSize = 5)
		{
			this.factory = factory;

			for (int i = 0; i < poolSize; i++)
				Create();
		}

		public T Allocate() 
		{
			for(var i = 0; i < members.Count; i++) 
			{
				if(!unavailable.Contains(members[i])) 
				{
					unavailable.Add(members[i]);
					return members[i];
				}
			}
			T newMember = Create();
			unavailable.Add(newMember);
			return newMember;
		}

		public void Release(T member)
		{
			member.Reset();
			unavailable.Remove(member);
		}

		private T Create() 
		{
			T member = factory.Create();
			members.Add(member);
			return member;
		}

		public IEnumerator GetEnumerator()
		{
			return members.GetEnumerator();
		}
	}
}