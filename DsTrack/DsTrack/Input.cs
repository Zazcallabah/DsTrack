namespace DsTrack
{
	public class Input
	{
		public uint[] Data { get; private set; }

		public Input( uint[] data, string label )
		{
			Data = data;
			Label = label;
		}

		public string Label { get; set; }
	}
}