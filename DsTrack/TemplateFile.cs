using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DsTrack
{
	public class TemplateFile
	{
		public TemplateFile( string path )
			: this( File.ReadAllLines( path ) )
		{ }

		public TemplateFile( IEnumerable<string> lines )
		{
			Lines = lines.Select( s => new Line( s ) ).ToArray();
		}

		public Line[] Lines { get; private set; }

		public string EntryFor( Result result, int start )
		{
			var addressOffset = start + result.BitOffset();
			for( var i = 0; i < Lines.Length - 1; i++ )
			{
				if( addressOffset >= Lines[i].Address && addressOffset < Lines[i + 1].Address )
				{
					return Lines[i].Type;
				}
			}
			if( Lines == null || Lines.Length == 0 )
				return string.Empty;
			return Lines.Last().Type;
		}
	}
}