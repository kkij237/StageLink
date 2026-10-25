namespace StageLink.Views.Components;

public partial class AppHeader : ContentView
{
	public static readonly BindableProperty SubtitleProperty =
		BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(AppHeader), string.Empty);

	public string Subtitle
	{
		get => (string)GetValue(SubtitleProperty);
		set => SetValue(SubtitleProperty, value);
	}

	public AppHeader()
	{
		InitializeComponent();
	}
}
