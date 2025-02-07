namespace LilPuter;

public interface IObservable
{
	public delegate void OnValueChangeDelegate(int newData);


	public void Subscribe(OnValueChangeDelegate subscriber);

	public void Unubscribe(OnValueChangeDelegate subscriber);
	public int SubscriberCount();
	/// <summary>
	/// Read the current value of the component.
	/// </summary>
	public int ReadValue();
}