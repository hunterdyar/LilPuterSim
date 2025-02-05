namespace LilPuter;

//Todo: This is not built out of nand gates. A multiplexer is basically an AND gate to select.
//Then we add a few of these and the selector bit chooses the correct AND operation. 
public class Multiplexer
{
	public Pin Select;
	public Pin[] Inputs;
	public Pin Output;
	public readonly int SelectorSize;
	public Multiplexer(WireManager manager, int size)
	{
		SelectorSize = PinUtility.SizeToRequiredBits(size);
		Select = new Pin(manager, "Multiplexer Select", SelectorSize);
		Inputs = new Pin[size];
		Output = new Pin(manager, "Multiplexer Output");
		Output.DependsOn(Select);
		for (int i = 0; i < size; i++)
		{
			Inputs[i] = new Pin(manager, $"Multiplexer Input {i}");
			manager.RegisterSystemAction(Inputs[i], AnyInputChanged);	
			Output.DependsOn(Inputs[i]);
		}
		manager.RegisterSystemAction(Select, SelectionChanged);
		
	}

	private void SelectionChanged(ISystem obj)
	{
		//If we are floating. TODO: I want to minize the need for catching these in output by catching them and preventing propagation.
		if (Select.Value[0] == 2)
		{
			return;
		}
		int val = PinUtility.ByteArrayToInt(Select.Value);
		if (val >= Inputs.Length)
		{
			throw new Exception($"Invalid Selection for Multiplexer. Selected {val}, max {Inputs.Length}");
		}
		Output.Set(Inputs[val].Value);
		
	}

	private void AnyInputChanged(ISystem system)
	{
		//whats a more efficient way to do this?
		if(Select.Value.Any(x=>x == (byte)WireSignal.Floating))
		{
			return;
		}
		int val = PinUtility.ByteArrayToInt(Select.Value);
		if (val >= Inputs.Length)
		{
			throw new Exception("Invalid Selection for Multiplexer");
		}
		//if the OR changes and we are set to an And, we won't set it. 
		//I think wrapping the escape here is the same (if not worse) than the .Set call, which will also check the if the value actually changes or not.
		//Because we might get lots of false calls here from the non-selected items. I want to just know that we aren't setting the output from this system. 
		//Debugging and stack tracing will get simpler and more accurate. Set's are called with intention.
		
		// if (Output.Value != Inputs[val].Value)
		// {
		// 	Output.Set(Inputs[val].Value);
		// }

		//Option B is to get the index of the changed system, like this:
		int index = Array.IndexOf(Inputs, system);
		if (index == val)
		{
			Output.Set(Inputs[val].Value);
		}
	}
}