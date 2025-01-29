namespace LilPuter;

public interface IObservable
{
	public delegate void OnValueChangeDelegate(byte[] newData);


	public void Subscribe(OnValueChangeDelegate subscriber);

	public void Unubscribe(OnValueChangeDelegate subscriber);
	public int SubscriberCount();
	/// <summary>
	/// Read the current value of the component.
	/// </summary>
	public byte[] ReadValue();
	/// <summary>
	/// How should this value be treated?
	/// </summary>
	public Type ValueType { get; }
}