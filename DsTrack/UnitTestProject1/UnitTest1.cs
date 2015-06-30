using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DsTrack;
using NUnit.Framework;

namespace UnitTestProject1
{

	[TestFixture]
	public class UnitTest1
	{
		static uint Test( uint[] _bytes )
		{
			return Track.GetFlags( _bytes );
		}

		[Test]
		public void metatest()
		{
			var path = @"C:\src\git\dsfp\DsTrack\DsTrack\templatedata.txt";
			if( File.Exists( path ) )
			{
				var l = File.ReadAllLines( path );
				var lines = l.Select( s => new Line( s ) ).ToArray();
				Result.Lines = lines;
				realfiles();
			}
		}

		[Test]
		public void realfiles()
		{
			var handler = new Handler();
			handler.AddFolder( @"C:\src\git\dsfp\templates\d" );

			var res = handler.Make();
			foreach( var r in res.Take( 500 ) )
			{
				Debug.WriteLine( r.ToString() );
			}

		}

		[Test]
		public void candoFiles()
		{
			var h = new Handler();
			h.Add( "DRAKS0005.sl2" );
			h.Add( "DRAKS0005 (2).sl2" );

			var res = h.Make();
			Assert.AreNotEqual( 0, res.Length );
		}

		[Test]
		public void cantrack()
		{
			var data = new uint[] { 1, 0 };
			var data2 = new uint[] { 0, 1 };
			int address = 0x0;
			var inputarr = new[] { new Input( data, "l1" ), new Input( data2, "l2" ) };
			Track.Labels = inputarr.Select( i => i.Label ).ToArray();
			var t = new Track( inputarr, address );
			var t2 = new Track( inputarr, 0x4 );

			Assert.AreEqual( 0, t.Make().Length );
			Assert.AreEqual( 1, t2.Make().Length );
			Assert.AreEqual( 4, t2.Make()[0].Address );
			Assert.AreEqual( 0, t2.Make()[0].Bit );
			Assert.AreEqual( 1, t2.Make()[0].LabelIndex );
			Assert.AreEqual( "l2", Track.Labels[t2.Make()[0].LabelIndex] );
		}
		[Test]
		public void willonlytrackonceforeachtrack()
		{
			var data = new uint[] { 1, 0 };
			var data2 = new uint[] { 0, 1 };
			var data3 = new uint[] { 0, 1 };
			int address = 0x0;
			var inputarr = new[] { new Input( data, "l1" ), new Input( data2, "l2" ), new Input( data3, "l3" ) };
			Track.Labels = inputarr.Select( i => i.Label ).ToArray();
			var t = new Track( inputarr, address );
			var t2 = new Track( inputarr, 0x4 );

			Assert.AreEqual( 0, t.Make().Length );
			Assert.AreEqual( 1, t2.Make().Length );
			Assert.AreEqual( 4, t2.Make()[0].Address );
			Assert.AreEqual( 0, t2.Make()[0].Bit );
			Assert.AreEqual( 1, t2.Make()[0].LabelIndex );
			Assert.AreEqual( "l2", Track.Labels[t2.Make()[0].LabelIndex] );
		}

		[Test]
		public void CanHandleEntire32Bits()
		{
			Assert.AreEqual( 0x00000F00, Test( new uint[] { 0xFFFF0000, 0xFF0000FF, 0xF0FF0F00, 0xF0F00F0F } ) );
		}

		[Test]
		public void IfStartedAs1_report0()
		{
			Assert.AreEqual( 0, Test( new uint[] { 1, 0, 0, 1 } ) );
		}

		[Test]
		public void IfToggleTwiceChangedReport0()
		{
			Assert.AreEqual( 0, Test( new uint[] { 0, 1, 0, 1 } ) );
		}

		[Test]
		public void IfToggleOnceChangedReport1()
		{
			Assert.AreEqual( 1, Test( new uint[] { 0, 1 } ) );
		}

		[Test]
		public void IfNothingChangedReport0()
		{
			Assert.AreEqual( 0, Test( new uint[] { 0, 0 } ) );
		}


		[Test]
		public void MustBeAtLeastSize2()
		{
			Assert.AreEqual( 0, Test( new uint[] { 0 } ) );
		}
	}


	/*
	public class Tracker
	{
		readonly int _length;

		readonly List<Track> _entries;
		public Tracker( int length )
		{
			_length = length;
			_entries = new List<Track>();
		}

		public void Add( byte[] bytes, string label )
		{
			if( _length != bytes.Length )
				throw new Exception();

			_entries.Add( new Track( bytes, label ) );
		}

		public Track[] Compute()
		{
			var track = new Track[_length];
		}
	}

	public static class EntriesExt
	{
		public static TrackHolder ExtractTracks( this Entry[] entries )
		{
			var length = entries[0].Bytes.Length;
			var l = new List<Track>();
			for( var i = 0; i < length; i++ )
			{
				var b = new byte[entries.Length];
				for( var j = 0; j < b.Length; j++ )
				{
					b[j] = entries[j].Bytes[i];
				}
				l.Add( new Track( b ) );
			}
			return new TrackHolder( entries.Select( e => e.Label ).ToArray() ) { Tracks = l.ToArray() };
		}
	}

	public class TrackHolder
	{
		public TrackHolder( string[] labels )
		{
			Labels = labels;
		}

		public string[] Labels { get; private set; }

		public Track[] Tracks { get; set; }
	}

	public class Track
	{
		readonly byte[] _bytes;

		public Track( byte[] bytes )
		{
			_bytes = bytes;
		}

		public byte Status( string[] labels )
		{
			if( _bytes.Length < 2 )
				return;

			for( var i = 0; i < 8; i++ )
			{
				byte status = 0;
				for( var b = 0; b < _bytes.Length; b++ )
				{

				}
			}

		}
	}

	public class Entry
	{
		public string Label { get; private set; }
		public byte[] Bytes { get; private set; }

		public Entry( byte[] bytes, string label )
		{
			Label = label;
			Bytes = bytes;
		}
	}


	public class Store
	{
		readonly string _filepath;
		List<StoreEntry> entries;

		public Store( string filepath )
		{
			_filepath = filepath;
			if( File.Exists( filepath ) )
			{
				var ser = new JsonSerializer();
				var internalstorage = ser.Deserialize( new StreamReader( File.OpenRead( filepath ) ), typeof( StoreEntry[] ) );
			}
			else
			{		/*readonly byte[] data;
		public Tracker( Stream memoryStream, int startindex, int length )
		{
			data = new byte[length];
			memoryStream.Seek( startindex, SeekOrigin.Current );
			memoryStream.Read( data, 0, length );
		}
				entries = new List<StoreEntry>();
			}
		}
	}

	public class StoreEntry
	{
		int address;

	}*/



}
