namespace StageLink.Models;

/// <summary>Kind : "category", "duration" ou "skill".</summary>
public record SearchFilter(string Kind, string Label);
