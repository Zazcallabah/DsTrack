using System;
using System.IO;
using System.Linq;

namespace DsTrack
{
	class Program
	{
		static void Main( string[] args )
		{
			if( args.Length < 1 )
			{
				Console.WriteLine( "Usage: DsTrack.exe pathtofolder\n" );
				return;
			}

			const string path = "templatedata.txt";
			if( File.Exists( path ) )
			{
				Result.Lines = File
					.ReadAllLines( path )
					.Select( s => new Line( s ) )
					.ToArray();
			}

			var handler = new Handler();
			handler.AddFolder( args[0] );

			foreach( var r in handler.Make() )
			{
				Console.WriteLine( r.ToString() );
			}

		}
	}
}
