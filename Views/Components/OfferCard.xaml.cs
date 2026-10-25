using System.Windows.Input;

namespace StageLink.Views.Components;

public partial class OfferCard : ContentView
{
	public static readonly BindableProperty OpenCommandProperty =
		BindableProperty.Create(nameof(OpenCommand), typeof(ICommand), typeof(OfferCard));

	public static readonly BindableProperty BookmarkCommandProperty =
		BindableProperty.Create(nameof(BookmarkCommand), typeof(ICommand), typeof(OfferCard));

	public ICommand? OpenCommand
	{
		get => (ICommand?)GetValue(OpenCommandProperty);
		set => SetValue(OpenCommandProperty, value);
	}

	public ICommand? BookmarkCommand
	{
		get => (ICommand?)GetValue(BookmarkCommandProperty);
		set => SetValue(BookmarkCommandProperty, value);
	}

	public OfferCard()
	{
		InitializeComponent();
	}
}
