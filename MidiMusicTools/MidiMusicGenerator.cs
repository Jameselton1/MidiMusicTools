using MidiMusicTools.Enum;
using NAudio.Midi;
using System;
using System.ComponentModel.DataAnnotations;

namespace MidiMusicTools {

  public static class MusicConstants {
    public static int NUM_OF_NOTES = System.Enum.GetValues(typeof(Note)).Length;
  }
  
  public struct Song {
    public List<Track> tracks;

    public Mode Mode;
    public Note[] Scale;
    public char[] SongStructure;
    public char[] TrackStructure;
  }
  
  public struct Track {
    public IList<MidiEvent> events;
    public int instrument;
  }

  public class SongGenerator {

    public Song NewSong() {
      var song = new Song();

      song = SetSongProperties(song);
      return song;
    }

    private Song SetSongProperties(Song song) {
      Random random = new Random();

      // Generate random mode
      int root = random.Next(MusicConstants.NUM_OF_NOTES);
      Array modeValues = System.Enum.GetValues(typeof(Mode));
      Mode randomMode = (Mode)modeValues.GetValue(random.Next(modeValues.Length));
      
      // Generate random song structure
      Array songStructureValues = System.Enum.GetValues(typeof(SongStructure));
      SongStructure randomSongStructure = (SongStructure)songStructureValues.GetValue(random.Next(songStructureValues.Length));

      //Generate random track structure
      Array trackStructureValues = System.Enum.GetValues(typeof(TrackStructure));
      TrackStructure randomTrackStructure = (TrackStructure)trackStructureValues.GetValue(random.Next(trackStructureValues.Length));

      // Assign values
      song.Mode = randomMode;
      song.Scale = CreateScale(root, song.Mode);
      song.SongStructure = randomSongStructure.ToString().ToCharArray();
      song.TrackStructure = randomTrackStructure.ToString().ToCharArray();
      return song;
    }

    // Return an array of musical notes, based on a scale template (mode) and a root note
    private Note[] CreateScale(int root, Mode mode) {
      int[] template = ScaleData.GetScaleTemplate(mode);
      Note[] scale = new Note[template.Length];

      for (int i = 0; i < template.Length; i++) {
        int index = (root + template[i]) % 12;
        scale[i] = (Note)System.Enum.GetValues(typeof(Note)).GetValue(index);
      }
      return scale;
    }

  }

  public class MidiMusicGenerator {
    // MIDI File Types:
    // 0 = Flattening: all tracks are combined into 1 track
    // 1 = Exploding:  all tracks are kept separate
    private int midiFileType = 1;

    private static int deltaTicksPerBar = 768;
    private int deltaTicksPerQuarterNote = deltaTicksPerBar / 4;

    public MidiEventCollection GenerateMidi() {
      MidiEventCollection events = new MidiEventCollection(midiFileType, deltaTicksPerQuarterNote);
      
      MidiEvent midiEvent = new MidiEvent(2, 0, MidiCommandCode.NoteOn);
      events.AddEvent(midiEvent, 0);
    }
  }
}