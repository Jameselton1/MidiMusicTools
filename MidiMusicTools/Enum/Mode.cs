namespace MidiMusicTools.Enum
{
  /// <summary>
  /// Represents musical modes, which are blueprints for constructing scales.
  /// Each mode defines a unique pattern of intervals within an octave.
  /// </summary>
  public enum Mode
  {
    Major,
    Minor,
    Dorian,
    Harmonic,
    Hungarian,
    Locrian,
    Lydian,
    Melodic,
    Mixolydian,
    Phrygian
  }

  /// <summary>
  /// Provides scale templates (interval patterns) for each musical mode.
  /// </summary>
  public static class ScaleData
  {
    /// <summary>
    /// Maps each Mode to its corresponding scale template (intervals from the root note).
    /// </summary>
    public static readonly Dictionary<Mode, int[]> ScaleTemplates = new()
    {
        { Mode.Major,       new[] { 0, 2, 4, 5, 7, 9, 11 } },
        { Mode.Minor,       new[] { 0, 2, 3, 5, 7, 8, 10 } },
        { Mode.Dorian,      new[] { 0, 2, 3, 5, 7, 9, 10 } },
        { Mode.Harmonic,    new[] { 0, 2, 3, 5, 7, 8, 11 } },
        { Mode.Hungarian,   new[] { 0, 2, 3, 6, 7, 8, 11 } },
        { Mode.Locrian,     new[] { 0, 1, 3, 5, 6, 8, 10 } },
        { Mode.Lydian,      new[] { 0, 2, 4, 6, 7, 9, 11 } },
        { Mode.Melodic,     new[] { 0, 2, 3, 5, 7, 9, 11 } },
        { Mode.Mixolydian,  new[] { 0, 2, 4, 5, 7, 9, 10 } },
        { Mode.Phrygian,    new[] { 0, 1, 3, 5, 7, 8, 10 } }
    };

    /// <summary>
    /// Gets the scale template (intervals) for the specified mode.
    /// </summary>
    /// <param name="mode">The musical mode.</param>
    /// <returns>An array of intervals representing the scale, or null if not found.</returns>
    public static int[] GetScaleTemplate(Mode mode)
    {
      return ScaleTemplates.TryGetValue(mode, out var intervals) ? intervals : null;
    }
  }
}