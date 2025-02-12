namespace LilPuter
{
	public class Combiner
	{
		public readonly Pin Output;
		public Pin[] Inputs => _ins;
		private readonly Pin[] _ins;
		private readonly WireManager _manager;
	
		public Combiner(ComputerBase comp, int width)
		{
			_manager = comp.WireManager;
			_ins = new Pin[width];
			Output = new Pin(comp.WireManager, "Combiner Out", width);
			for (int i = 0; i < width; i++)
			{
				_ins[i] = new Pin(comp.WireManager, $"Combiner In {i}");
				Output.DependsOn(_ins[i]);
				int i1 = i;
				//The action is a wrapper for a function so we get the index passed in
				comp.WireManager.RegisterSystemAction(_ins[i], (p) => { OnInputChange(i1);});
			}
		}

		private void OnInputChange(int i)
		{
			Output.SetBit(i, (WireSignal)_ins[i].Value);
			//We are not calling Output.Set, we are basically calling Output.SetSilently some number of times (we expect numerous inputs to change at once)
			//so we manually call change, which sets a bool flag true to ensure output gets processed. Setting it multiple times is fine.
			_manager.Changed(Output,Output.Value);
		}
	}
}