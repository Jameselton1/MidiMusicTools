using MidiMusicTools.Services;
namespace MidiMusicTools.Enum
{
  public enum ChordType
  {
    Power,
    Seventh,
    Triad
  }

  public static class ChordData
  {
    /* Values represent intervals from chord's root note within the scale
     *
     * E.G. C Minor is C - D - E♭ - F - G - A♭ - B♭ - C,
     * 
     * a triad chord with the root note of c in the C Minor scale is:
     * C - E♭ - G
     * 
     * intervals from root:
     * 0 - 2  - 4
    */
    public static readonly Dictionary<ChordType, int[]> ChordTemplates = new()
    {
        { ChordType.Power,  new[] { 0, 4 } },
        { ChordType.Seventh, new[] { 0, 2, 4, 6 } },
        { ChordType.Triad,  new[] { 0, 2, 4 } }
    };

    public static List<Note> GetChordNotes(ChordType type, Note chordRoot, List<Note> scale)
    {
      var notes = new List<Note>();
      int[] chordIntervals = ChordTemplates.TryGetValue(type, out var template) ? template : null;

      // Find the position of the chord root note within the given scale
      int chordRootIndex = scale.FindIndex(n => n == chordRoot);
      // Build the chord by selecting notes from the scale at intervals defined by the chord template
      for (int i = 0; i < chordIntervals.Length; i++)
      {
        // Calculate the note's index in the scale by adding the chord interval to the root's index,
        // then wrap around using modulo to stay within the scale's bounds.
        int noteIndex = (chordRootIndex + chordIntervals[i]) % scale.Count;
        // Add 
        notes.Add(scale[noteIndex]);
      }

      return notes;
    }
  }
}
