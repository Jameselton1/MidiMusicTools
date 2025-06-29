namespace MidiMusicTools.Models
{
	/// <summary>
	/// Represents a single beat in a bar, containing one or more subdivisions.
	/// </summary>
	public class Beat
	{
		/// <summary>List of subdivisions within this beat.</summary>
		public List<Subdivision> Subdivisions { get; set; } = new List<Subdivision>();
	}
}