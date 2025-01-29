namespace LilPuter;

public interface IObservable 
{
	/// <summary>
	/// Read the current value of the component.
	/// </summary>
	public byte[] ReadValue();
	/// <summary>
	/// How should this value be treated?
	/// </summary>
	public Type ValueType { get; }
}