//------------------------------------------------------------------------------

//  <auto-generated>
//      This code was generated by:
//        TerminalGuiDesigner v1.0.17.0
//      You can make changes to this file and they will not be overwritten when saving.
//  </auto-generated>
// -----------------------------------------------------------------------------

namespace LilPuterTerm
{
	using Terminal.Gui;


	public partial class SystemViewer
	{
		public SystemViewer()
		{
			InitializeComponent();
			button1.Clicked += () => MessageBox.Query("Hello", "Hello There!", "Ok");
		}
	}
}