using System.Windows.Input;

namespace StageLink.Views.Components;

public partial class FilterChipView : ContentView
{
	public static readonly BindableProperty CommandProperty =
		BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FilterChipView));

	public ICommand? Command
	{
		get => (ICommand?)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public FilterChipView()
	{
		InitializeComponent();
	}
}
