using MidiMusicTools;
using MidiMusicTools.Enum;
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
    Track[] tracks = song.Tracks;
    tracks = AddChangeInstrumentsEvent(tracks);
    // generate note events
    for (int t = 0; t < tracks.Length; t++)
    {
      int timeElapsed = 0;
      List<int[,]> midiNotes = GetMidiNotesFromTrack(tracks[t]);
      foreach (int[,] notes in midiNotes)
      {
        for (int x = 0; x < notes.GetLength(0); x++)
        {
          int[] barNotes = new int[notes.GetLength(1)];
          for (int y = 0; y < notes.GetLength(1); y++)
          {
            barNotes[y] = notes[x, y];
          }
          int noteLength = (deltaTicksPerQuarterNote / notes.GetLength(0));
          tracks[t] = AddNoteEvent(tracks[t], barNotes, timeElapsed, noteLength);
          timeElapsed += noteLength;
        }
      }
    }

    var events = new MidiEventCollection(midiFileType, deltaTicksPerQuarterNote);
    foreach (Track track in tracks)
    {
      events.AddTrack(track.Events);
    }
    events.PrepareForExport();
    return events;
  }

  public void Export(string exportFilePath, MidiEventCollection events)
  {
    MidiFile.Export(exportFilePath, events);
  }

  private Track[] AddChangeInstrumentsEvent(Track[] tracks)
  {
    // Change instruments and then return values.
    for (int i = 0; i < tracks.Length; i++)
    {
      // Change instrument midievent
      int timeInTicks = 0;
      int channel = 1;
      int instrument = 50;
      var instrumentEvent = new PatchChangeEvent(timeInTicks, channel, instrument);
      tracks[0].Events.Add(instrumentEvent);
    }
    return tracks;
  }

  private Track AddNoteEvent(Track track, int[] notes, int startTime, int noteLength)
  {
    int channel = 1;
    int velocity = 100;
    for (int i = 0; i < notes.Length; i++)
    {
      var noteEvent = new NoteOnEvent(startTime, channel, notes[i], velocity, noteLength);
      track.Events.Add(noteEvent);
      track.Events.Add(noteEvent.OffEvent);
    }
    return track;
  }

  private List<int[,]> GetMidiNotesFromTrack(Track track)
  {
    List<Note[,]> notes = track.notes;
    List<int[,]> intNotes = new List<int[,]>();
    int octave;
    switch (track.Type)
    {
      case 'C': octave = 4; break;
      case 'B': octave = 3; break;
      case 'M': octave = 6; break;
      default: octave = 5; break;
    }

    for (int i = 0; i < notes.Count; i++)
    {
      int dimensionOne = notes[i].GetLength(0);
      int dimensionTwo = notes[i].GetLength(1);
      intNotes.Add(new int[dimensionOne, dimensionTwo]);

      for (int x = 0; x < notes[i].GetLength(0); x++)
      {
        for (int y = 0; y < notes[i].GetLength(1); y++)
        {
          var allNotes = Note.GetValues(typeof(Note));
          int noteIndex = Array.IndexOf(allNotes, notes[i][x, y]);
          intNotes[i][x, y] = noteIndex + (12 * octave);
        }
      }
    }

    return intNotes;
  }
}