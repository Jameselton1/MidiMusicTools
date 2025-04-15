namespace MidiMusicTools.Enum {
  public enum ChordType {
    Power,
    Seventh,
    Triad
  }

  public static class ChordData {
    // Values represent intervals from chord's root note
    public static readonly Dictionary<ChordType, int[]> ChordFormulas = new()
    {
        { ChordType.Power,  new[] { 0, 4 } },
        { ChordType.Seventh, new[] { 0, 2, 4, 6 } },
        { ChordType.Triad,  new[] { 0, 2, 4 } }
    };

    public static int[] GetChordFormula(ChordType type) {
      return ChordFormulas.TryGetValue(type, out var formula) ? formula : null;
    }
  }
}
