namespace StageLink.Services;

public interface IDemoTourService
{
	/// <summary>Parcourt automatiquement les écrans de l'application en ~27 s (vidéo de démo de 30 s).</summary>
	Task RunAsync(CancellationToken cancellationToken = default);
}
