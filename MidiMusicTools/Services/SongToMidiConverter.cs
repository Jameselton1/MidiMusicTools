using MidiMusicTools;
using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using NAudio.Midi;

/// <summary>
/// Converts a Song object into a MIDI file using NAudio.
/// Handles instrument changes, note event generation, and MIDI export.
/// </summary>
public class SongToMidiConverter
{
  // MIDI File Types:
  // 0 = Flattening: all tracks are combined into 1 track
  // 1 = Exploding:  all tracks are kept separate
  private int midiFileType = 1;
  private static int deltaTicksPerBar = 768;
  private int deltaTicksPerQuarterNote = deltaTicksPerBar / 4;

  /// <summary>
  /// Converts a Song object to a MidiEventCollection for export.
  /// </summary>
  /// <param name="song">The Song to convert.</param>
  /// <returns>A MidiEventCollection representing the song.</returns>
  public MidiEventCollection GenerateMidi(Song song)
  {
    // List of MIDI events for each track
    var listEvents = new List<List<MidiEvent>>();

    // Add instrument change events at the start of each track
    listEvents = AddChangeInstrumentsEvent(song.Tracks, listEvents);

    // Generate note events for each track
    for (int t = 0; t < song.Tracks.Count; t++)
    {
      var events = new List<MidiEvent>();
      int timeElapsed = 0;
      // Iterate through all beats in the track
      foreach (Beat beat in song.Tracks[t].Beats)
      {
        // Iterate through all subdivisions in the beat
        foreach (Subdivision subdivision in beat.Subdivisions)
        {
          int numOfSubdivisions = beat.Subdivisions.Count;
          // Calculate note length based on subdivisions
          int noteLength = (deltaTicksPerQuarterNote / numOfSubdivisions);
          // Generate MIDI events for these notes
          events.AddRange(GenerateBeatNoteEvents(subdivision.MidiNotes, timeElapsed, noteLength));
          // Advance the time for the next subdivision
          timeElapsed += noteLength;
        }
      }
      // Add all events for this track to the list
      listEvents.Add(events);
    }

    // Create the MIDI event collection and add all tracks
    var eventCollection = new MidiEventCollection(midiFileType, deltaTicksPerQuarterNote);
    foreach (List<MidiEvent> events in listEvents)
    {
      eventCollection.AddTrack(events);
    }
    eventCollection.PrepareForExport();
    return eventCollection;
  }

  /// <summary>
  /// Exports the MidiEventCollection to a MIDI file.
  /// </summary>
  /// <param name="exportFilePath">The file path to export to.</param>
  /// <param name="events">The MidiEventCollection to export.</param>
  public void Export(string exportFilePath, MidiEventCollection events)
  {
    MidiFile.Export(exportFilePath, events);
  }

  /// <summary>
  /// Adds a MIDI instrument change event at the start of each track.
  /// </summary>
  /// <param name="tracks">List of tracks in the song.</param>
  /// <param name="listEvents">List of MIDI event lists for each track.</param>
  /// <returns>The updated list of MIDI event lists.</returns>
  private List<List<MidiEvent>> AddChangeInstrumentsEvent(List<Track> tracks, List<List<MidiEvent>> listEvents)
  {
    for (int i = 0; i < tracks.Count; i++)
    {
      var events = new List<MidiEvent>();
      int instrument = tracks[i].MidiInstrument;

      // Create a PatchChangeEvent to set the instrument
      int timeInTicks = 0;
      int channel = 1;
      var instrumentEvent = new PatchChangeEvent(timeInTicks, channel, instrument);
      events.Add(instrumentEvent);
      listEvents.Add(events);
    }
    return listEvents;
  }

  /// <summary>
  /// Generates MIDI note on/off events for a list of notes at a given time and length.
  /// </summary>
  /// <param name="notes">List of MIDI note numbers.</param>
  /// <param name="startTime">Start time in ticks.</param>
  /// <param name="noteLength">Length of the note in ticks.</param>
  /// <returns>List of MIDI events (note on and note off).</returns>
  private List<MidiEvent> GenerateBeatNoteEvents(List<int> notes, int startTime, int noteLength)
  {
    var events = new List<MidiEvent>();
    int channel = 1;
    int velocity = 100;
    for (int i = 0; i < notes.Count; i++)
    {
      // Create note on event
      var noteEvent = new NoteOnEvent(startTime, channel, notes[i], velocity, noteLength);
      events.Add(noteEvent);
      // Add corresponding note off event
      events.Add(noteEvent.OffEvent);
    }
    return events;
  }
}