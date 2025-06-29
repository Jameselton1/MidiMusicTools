namespace MidiMusicTools.Models
{
	/// <summary>
	/// Represents a complete song, containing all tracks.
	/// </summary>
	public struct Song
	{
		/// <summary>List of tracks in the song.</summary>
		public List<Track> Tracks { get; set; }
	}
}