namespace MidiMusicTools.Enum {
  /*
   * Modes are the blueprint for constructing the scale.
   * This is explained at https://ledgernote.com/columns/music-theory/musical-modes-explained/
   */
  public enum Mode {
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

  public static class ScaleData {
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

    public static int[] GetScaleTemplate(Mode mode) {
      return ScaleTemplates.TryGetValue(mode, out var intervals) ? intervals : null;
    }
  }
}