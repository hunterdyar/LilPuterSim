namespace LilPuter;

public interface IObservable 
{
	//Called when the value is set to a new value.
	public Action<byte[]> OnValueChange { get; }
	/// <summary>
	/// Read the current value of the component.
	/// </summary>
	public byte[] ReadValue();
	/// <summary>
	/// How should this value be treated?
	/// </summary>
	public Type ValueType { get; }
}