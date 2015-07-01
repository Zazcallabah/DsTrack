using System;

namespace DsTrack
{
	public class Result
	{
		/// <summary>
		/// word-aligned
		/// </summary>
		public int Address { get; private set; }

		/// <summary>
		/// 0 is lsb
		/// 31 is msb
		/// </summary>
		public int Bit { get; private set; }

		public int LabelIndex { get; private set; }

		public Result( int address, int bit, int labelIndex )
		{
			Address = address;
			Bit = bit;
			LabelIndex = labelIndex;
		}

		/// <summary>
		/// Address correction due to bigendian uints
		/// </summary>
		public int BitOffset()
		{
			return Address + ( Bit / -8 ) + 3;
		}
	}
}