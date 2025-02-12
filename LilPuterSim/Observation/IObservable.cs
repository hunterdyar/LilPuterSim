namespace LilPuter
{
	public interface IObservable<T>
	{
		public delegate void OnValueChangeDelegate(T newData);


		public void Subscribe(OnValueChangeDelegate subscriber);

		public void Unubscribe(OnValueChangeDelegate subscriber);
		public int SubscriberCount();
		/// <summary>
		/// Read the current value of the component.
		/// </summary>
		public T ReadValue();
	}
}