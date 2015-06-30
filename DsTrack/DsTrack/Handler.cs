using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DsTrack
{
	public class Handler
	{
		List<Input> _inputs = new List<Input>();

		public void AddFolder( string folderpath, int start = 0x2C0, int length = 0x60020 )
		{
			Result.InitialOffset = start;
			DirectoryInfo info = new DirectoryInfo( folderpath );
			foreach( var f in info.GetFiles().OrderBy( f => f.LastWriteTimeUtc ) )
			{
				Add( f.FullName, start, length );
			}
		}

		public void Add( string filepath )
		{
			Add( filepath, 0x2C0, 0x60020 );
		}

		public void Add( string filepath, int start, int length )
		{
			if( length % 4 != 0 )
				throw new Exception();
			var b = File.ReadAllBytes( filepath );
			uint[] uintData = new uint[length / 4];
			using( var ms = new MemoryStream( b ) )
			{
				ms.Position = start;
				using( var reader = new BinaryReader( ms ) )
					for( int i = 0; i < uintData.Length; i++ )
						uintData[i] = reader.ReadUInt32();
			}
			_inputs.Add( new Input( uintData, Path.GetFileNameWithoutExtension( filepath ) ) );
		}

		public Result[] Make()
		{
			Track.Labels = _inputs.Select( i => i.Label ).ToArray();
			var arr = _inputs.ToArray();
			var max = arr[0].Data.Length;
			var res = new List<Result>();
			for( var i = 0; i < max; i++ )
			{
				var t = new Track( arr, i * 4 );
				res.AddRange( t.Make() );
			}
			return res.ToArray();
		}
	}
}