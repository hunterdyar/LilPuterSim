using LilPuter;
using Terminal.Gui;

namespace LilPuterTerm;

public class PinView : Terminal.Gui.Button
{
	private Pin _pin;
	private string _name;
	private bool _triggerAble;
	public PinView(Pin pin, string name, bool triggerAble = true)
	{
		_pin = pin;
		_name = name;
		_triggerAble = triggerAble;
		
		_pin.Subscribe(OnPinChange);
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		this.Text = _name+" ";
		this.CanFocus = _triggerAble;
		
	}

	public override void OnClicked()
	{
		if (_triggerAble)
		{
			var val = _pin.ReadValue();
			var flip = WireUtility.Invert(val);
			_pin.SetAndImpulse(flip);
		}
	}


	private void OnPinChange(byte[] val)
	{
		var d = (WireSignal)val[0];
		if (d == WireSignal.High)
		{
			this.ColorScheme = Colors.Dialog;
			this.Text = _name+"!";
		}else if (d == WireSignal.Low)
		{
			this.ColorScheme = Colors.Base;
			this.Text = _name+" ";
		}
		else
		{
			//floating
			this.Text = _name+"?";
			this.ColorScheme = Colors.Error;
		}

		this.SetNeedsDisplay();
	}
}