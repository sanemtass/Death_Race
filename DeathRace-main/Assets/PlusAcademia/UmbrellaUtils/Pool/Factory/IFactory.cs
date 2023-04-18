namespace UmbrellaUtils.Pool.Factory
{
	public interface IFactory<T>
	{
		T Create();
	}
}