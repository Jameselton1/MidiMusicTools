using MidiMusicTools.Enum;
using NAudio.Midi;

namespace MidiMusicTools
{

  public static class MusicConstants
  {
    public static int NUM_OF_NOTES = System.Enum.GetValues(typeof(Note)).Length;
  }


  public struct TimeSignature
  {
    public int BeatsPerBar;
    public int Denominator;
  }

  public struct Track
  {
    public List<MidiEvent> Events = new List<MidiEvent>();
    public int Instrument;
    public char Type;
    public List<Note[,]> notes;

    public Track()
    {
    }
  }
  
}
