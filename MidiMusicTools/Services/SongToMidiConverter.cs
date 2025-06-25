using MidiMusicTools;
using MidiMusicTools.Enum;
using MidiMusicTools.Models;
using NAudio.Midi;

public class SongToMidiConverter
{
  // MIDI File Types:
  // 0 = Flattening: all tracks are combined into 1 track
  // 1 = Exploding:  all tracks are kept separate
  private int midiFileType = 1;
  private static int deltaTicksPerBar = 768;
  private int deltaTicksPerQuarterNote = deltaTicksPerBar / 4;

  // MidiEventCollection contains a list of tracks. 
  // A track contains a list of MidiEvents.
  // This means I need to create a list of MidiEvents for 
  // each track and then add each to MidiEventCollections
  public MidiEventCollection GenerateMidi(Song song)
  {
    var listEvents = new List<List<MidiEvent>>();
    listEvents = AddChangeInstrumentsEvent(song.Tracks, listEvents);

    // generate note events
    for (int t = 0; t < song.Tracks.Count; t++)
    {
      var events = new List<MidiEvent>();
      int timeElapsed = 0;
      foreach (Beat beat in song.Tracks[t].Beats)
      {
        foreach (Subdivision subdivision in beat.Subdivisions)
        {
          int numOfSubdivisions = beat.Subdivisions.Count;
          var beatNotes = new List<int>();
          foreach (int note in subdivision.MidiNotes)
          {
            beatNotes.Add(note);
          }
          int noteLength = (deltaTicksPerQuarterNote / numOfSubdivisions);
          events.AddRange(GenerateBeatNoteEvents(beatNotes, timeElapsed, noteLength));
          timeElapsed += noteLength;
        }
      }
      listEvents.Add(events);
    }

    var eventCollection = new MidiEventCollection(midiFileType, deltaTicksPerQuarterNote);
    foreach (List<MidiEvent> events in listEvents)
    {
      eventCollection.AddTrack(events);
    }
    eventCollection.PrepareForExport();
    return eventCollection;
  }

  public void Export(string exportFilePath, MidiEventCollection events)
  {
    MidiFile.Export(exportFilePath, events);
  }

  private List<List<MidiEvent>> AddChangeInstrumentsEvent(List<Track> tracks, List<List<MidiEvent>> listEvents)
  {
    // Change instruments and then return values.
    for (int i = 0; i < tracks.Count; i++)
    {
      var events = new List<MidiEvent>();
      int instrument = tracks[i].MidiInstrument;

      // Change instrument midievent
      int timeInTicks = 0;
      int channel = 1;
      var instrumentEvent = new PatchChangeEvent(timeInTicks, channel, instrument);
      events.Add(instrumentEvent);
      listEvents.Add(events);
    }
    return listEvents;
  }

  private List<MidiEvent> GenerateBeatNoteEvents(List<int> notes, int startTime, int noteLength)
  {
    var events = new List<MidiEvent>();

    int channel = 1;
    int velocity = 100;
    for (int i = 0; i < notes.Count; i++)
    {
      var noteEvent = new NoteOnEvent(startTime, channel, notes[i], velocity, noteLength);
      events.Add(noteEvent);
      events.Add(noteEvent.OffEvent);
    }
    return events;
  }
}