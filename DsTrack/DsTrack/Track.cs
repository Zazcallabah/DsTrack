using System.Collections.Generic;

namespace DsTrack
{
	public class Track
	{
		readonly uint[] _data;
		readonly int _address;
		public static string[] Labels;
		readonly uint _flags;

		public Track( Input[] inputarr, int address )
		{
			_data = new uint[inputarr.Length];
			for( var i = 0; i < inputarr.Length; i++ )
			{
				_data[i] = inputarr[i].Data[address / 4];
			}
			_address = address;
			_flags = GetFlags( _data );
		}


		public static uint GetFlags( uint[] data )
		{
			if( data.Length < 2 )
				return 0;

			uint init = ~( data[0] ); // only bits that start as 0 are allowed
			for( var i = 0; i < data.Length - 1; i++ )
			{
				var b = data[i];
				var bn = data[i + 1];

				var hasswitchd = b ^ bn;
				var opregress = hasswitchd & b;
				init = init & ~opregress; // update init, only bits that didnt regress are allowed now
			}
			return data[data.Length - 1] & init; // only allow bits that started on 0, end on 1 and never regressed
		}

		public Result[] Make()
		{
			var l = new List<Result>();
			uint bitmask = 0x1;
			for( var i = 0; i < 32; i++ )
			{
				if( ( bitmask & _flags ) != 0 )
				{
					for( var j = 0; j < _data.Length; j++ )
					{
						if( ( bitmask & _data[j] ) != 0 )
						{
							l.Add( new Result( _address, i, j ) );
							break;
						}
					}
				}
				bitmask = bitmask << 1;
			}
			return l.ToArray();
		}
	}
}