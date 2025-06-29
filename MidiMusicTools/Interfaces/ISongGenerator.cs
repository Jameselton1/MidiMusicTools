using MidiMusicTools.Models;

namespace MidiMusicTools.Interfaces
{
	/// <summary>
	/// Interface for generating a complete Song object.
	/// Implementations define how a song is procedurally or custom generated.
	/// </summary>
	public interface ISongGenerator
	{
		/// <summary>
		/// Generates a new Song instance with tracks, structure, and notes.
		/// </summary>
		/// <returns>A generated Song object.</returns>
		public Song GenerateSong();
	}
}