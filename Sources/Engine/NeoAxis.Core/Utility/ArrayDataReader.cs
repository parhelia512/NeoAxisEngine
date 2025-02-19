// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace NeoAxis
{
	/// <summary>
	/// A class for streaming data from an array.
	/// </summary>
	public class ArrayDataReader
	{
		byte[] data;
		int currentPosition;
		int endPosition;
		bool overflow;

		//

		public ArrayDataReader()
		{
		}

		public ArrayDataReader( byte[] data )
		{
			this.data = data;
			endPosition = data.Length;
		}

		public ArrayDataReader( byte[] data, int startPosition, int length )
		{
			Init( data, startPosition, length );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void Init( byte[] data, int startPosition, int length )
		{
			this.data = data;
			currentPosition = startPosition;
			endPosition = startPosition + length;
			overflow = false;
		}

		//public ArrayDataReader( byte[] data, int byteOffset = 0 )
		//{
		//	Init( data, byteOffset );
		//}

		//public ArrayDataReader( byte[] data )
		//{
		//	Init( data, 0, data.Length );
		//	//Init( data, 0, data.Length * 8 );
		//}

		//public void Init( byte[] data, int byteOffset )
		//{
		//	this.data = data;
		//	this.bytePosition = byteOffset;
		//	//bitPosition = bitOffset;
		//	//startBitPosition = bitPosition;
		//	//endBitPosition = bitPosition + bitLength;
		//	overflow = false;
		//}

		public int CurrentPosition
		{
			get { return currentPosition; }
		}

		public int EndPosition
		{
			get { return endPosition; }
		}

		public bool Overflow
		{
			get { return overflow; }
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public bool Complete()
		{
			return currentPosition == endPosition && !overflow;
		}

		public void ReadSkip( int length )
		{
			currentPosition += length;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public unsafe void ReadBuffer( void* destination, int length )
		{
			int newPosition = currentPosition + length;
			if( overflow || newPosition > endPosition )
			{
				overflow = true;
				return;
			}

			fixed( byte* pData = data )
			{
				byte* p = pData + currentPosition;

				if( length == 8 )
					*(ulong*)destination = *(ulong*)p;
				else if( length == 4 )
					*(uint*)destination = *(uint*)p;
				else if( length == 2 )
					*(ushort*)destination = *(ushort*)p;
				else
					Buffer.MemoryCopy( p, destination, length, length );
			}

			//Marshal.Copy( data, currentPosition, (IntPtr)destination, length );

			currentPosition = newPosition;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadBuffer( byte[] destination, int offset, int length )
		{
			int newPosition = currentPosition + length;
			if( overflow || newPosition > endPosition )
			{
				overflow = true;
				return;
			}
			Array.Copy( data, currentPosition, destination, offset, length );
			currentPosition = newPosition;
		}

		//public void ReadBuffer( byte[] destination, int byteOffset, int byteLength )
		//{
		//	int newPosition = bitPosition + byteLength * 8;
		//	if( overflow || newPosition > endBitPosition )
		//	{
		//		overflow = true;
		//		return;
		//	}
		//	BitWriter.ReadBytes( data, byteLength, bitPosition, destination, byteOffset );
		//	bitPosition = newPosition;
		//}

		//public void ReadBuffer( byte[] destination )
		//{
		//	ReadBuffer( destination, 0, destination.Length );
		//}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public bool ReadBoolean()
		{
			return ReadByte() != 0;

			//int newPosition = bitPosition + 1;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return false;
			//}
			//byte value = BitWriter.ReadByte( data, 1, bitPosition );
			//bitPosition = newPosition;
			//return ( value > 0 ? true : false );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public byte ReadByte()
		{
			int newPosition = currentPosition + 1;
			if( overflow || newPosition > endPosition )
			{
				overflow = true;
				return 0;
			}
			var value = data[ currentPosition ];
			currentPosition = newPosition;
			return value;
		}

		//public sbyte ReadSByte()
		//{
		//   unchecked
		//   {
		//      return (sbyte)ReadByte();
		//   }
		//}

		//public byte ReadSByte( int numberOfBits )

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public short ReadInt16()
		{
			unsafe
			{
				short result = 0;
				ReadBuffer( &result, 2 );
				return result;
			}

			//int newPosition = bitPosition + 16;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}
			//uint value = BitWriter.ReadUInt32( data, 16, bitPosition );
			//bitPosition = newPosition;
			//return (short)value;
		}

		//public byte ReadInt16( int numberOfBits )

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public ushort ReadUInt16()
		{
			unsafe
			{
				ushort result = 0;
				ReadBuffer( &result, 2 );
				return result;
			}

			//int newPosition = bitPosition + 16;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}
			//uint value = BitWriter.ReadUInt32( data, 16, bitPosition );
			//bitPosition = newPosition;
			//return (ushort)value;
		}

		//public byte ReadUInt16( int numberOfBits )

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public int ReadInt32()
		{
			unsafe
			{
				int result = 0;
				ReadBuffer( &result, 4 );
				return result;
			}

			//int newPosition = bitPosition + 32;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}
			//uint value = BitWriter.ReadUInt32( data, 32, bitPosition );
			//bitPosition = newPosition;
			//return (int)value;
		}

		//public int ReadInt32( int numberOfBits )
		//{
		//	int newPosition = bitPosition + numberOfBits;
		//	if( overflow || newPosition > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}

		//	uint value = BitWriter.ReadUInt32( data, numberOfBits, bitPosition );
		//	bitPosition += numberOfBits;

		//	if( numberOfBits == 32 )
		//		return (int)value;

		//	int signBit = 1 << ( numberOfBits - 1 );
		//	if( ( value & signBit ) == 0 )
		//		return (int)value; // positive

		//	// negative
		//	unchecked
		//	{
		//		uint mask = ( (uint)-1 ) >> ( 33 - numberOfBits );
		//		uint tmp = ( value & mask ) + 1;
		//		return -( (int)tmp );
		//	}
		//}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public uint ReadUInt32()
		{
			unsafe
			{
				uint result = 0;
				ReadBuffer( &result, 4 );
				return result;
			}

			//int newPosition = bitPosition + 32;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}
			//uint value = BitWriter.ReadUInt32( data, 32, bitPosition );
			//bitPosition += 32;
			//return value;
		}

		//public UInt32 ReadUInt32( int numberOfBits )
		//{
		//	int newPosition = bitPosition + numberOfBits;
		//	if( overflow || newPosition > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}
		//	uint value = BitWriter.ReadUInt32( data, numberOfBits, bitPosition );
		//	bitPosition += numberOfBits;
		//	return value;
		//}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public long ReadInt64()
		{
			unsafe
			{
				long result = 0;
				ReadBuffer( &result, 8 );
				return result;
			}

			//int newPosition = bitPosition + 64;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}
			//unchecked
			//{
			//	ulong value = ReadUInt64();
			//	long longValue = (long)value;
			//	return longValue;
			//}
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public ulong ReadUInt64()
		{
			unsafe
			{
				ulong result = 0;
				ReadBuffer( &result, 8 );
				return result;
			}

			//int newPosition = bitPosition + 64;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}
			//ulong low = BitWriter.ReadUInt32( data, 32, bitPosition );
			//bitPosition += 32;
			//ulong high = BitWriter.ReadUInt32( data, 32, bitPosition );
			//bitPosition += 32;
			//ulong value = low + ( high << 32 );
			//return value;
		}

		//public ulong ReadUInt64( int numberOfBits )
		//{
		//	int newPosition = bitPosition + numberOfBits;
		//	if( overflow || newPosition > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}

		//	ulong value;
		//	if( numberOfBits <= 32 )
		//	{
		//		value = (ulong)BitWriter.ReadUInt32( data, numberOfBits, bitPosition );
		//	}
		//	else
		//	{
		//		value = BitWriter.ReadUInt32( data, 32, bitPosition );
		//		value |= BitWriter.ReadUInt32( data, numberOfBits - 32, bitPosition ) << 32;
		//	}
		//	bitPosition += numberOfBits;
		//	return value;
		//}

		//public long ReadInt64( int numberOfBits )
		//{
		//	int newPosition = bitPosition + numberOfBits;
		//	if( overflow || newPosition > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}
		//	return (long)ReadUInt64( numberOfBits );
		//}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public float ReadSingle()
		{
			unsafe
			{
				float result = 0;
				ReadBuffer( &result, 4 );
				return result;
			}

			//int newPosition = bitPosition + 32;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}

			////read directly
			//if( ( bitPosition & 7 ) == 0 )
			//{
			//	//endianness is handled inside BitConverter.ToSingle
			//	float value = BitConverter.ToSingle( data, bitPosition >> 3 );
			//	bitPosition += 32;
			//	return value;
			//}

			//byte[] bytes = new byte[ 4 ];
			//ReadBuffer( bytes );
			//if( overflow )
			//	return 0;
			////endianness is handled inside BitConverter.ToSingle
			//return BitConverter.ToSingle( bytes, 0 );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public double ReadDouble()
		{
			unsafe
			{
				double result = 0;
				ReadBuffer( &result, 8 );
				return result;
			}

			//int newPosition = bitPosition + 64;
			//if( overflow || newPosition > endBitPosition )
			//{
			//	overflow = true;
			//	return 0;
			//}

			////read directly
			//if( ( bitPosition & 7 ) == 0 )
			//{
			//	double value = BitConverter.ToDouble( data, bitPosition >> 3 );
			//	bitPosition += 64;
			//	return value;
			//}

			//byte[] bytes = new byte[ 8 ];
			//ReadBuffer( bytes );
			//if( overflow )
			//	return 0;
			////endianness is handled inside BitConverter.ToSingle
			//return BitConverter.ToDouble( bytes, 0 );
		}

		/// <summary>
		/// Reads a UInt32 written using WriteVariableUInt32()
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public uint ReadVariableUInt32()
		{
			if( overflow )
				return 0;

			int num1 = 0;
			int num2 = 0;
			while( true )
			{
				if( num2 == 0x23 )
				{
					overflow = true;
					return 0;
				}

				byte num3 = ReadByte();
				if( overflow )
					return 0;

				num1 |= ( num3 & 0x7f ) << ( num2 & 0x1f );
				num2 += 7;
				if( ( num3 & 0x80 ) == 0 )
					return (uint)num1;
			}
		}

		/// <summary>
		/// Reads a Int32 written using WriteVariableInt32()
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public int ReadVariableInt32()
		{
			if( overflow )
				return 0;

			int num1 = 0;
			int num2 = 0;
			while( true )
			{
				if( num2 == 0x23 )
				{
					overflow = true;
					return 0;
				}

				byte num3 = ReadByte();
				if( overflow )
					return 0;

				num1 |= ( num3 & 0x7f ) << ( num2 & 0x1f );
				num2 += 7;
				if( ( num3 & 0x80 ) == 0 )
				{
					int sign = ( num1 << 31 ) >> 31;
					return sign ^ ( num1 >> 1 );
				}
			}
		}

		/// <summary>
		/// Reads a UInt64 written using WriteVariableUInt64()
		/// </summary>
		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public ulong ReadVariableUInt64()
		{
			if( overflow )
				return 0;

			ulong num1 = 0;
			int num2 = 0;
			while( true )
			{
				if( num2 == 0x77 )
				{
					overflow = true;
					return 0;
				}

				byte num3 = ReadByte();
				if( overflow )
					return 0;

				num1 |= ( (ulong)num3 & 0x7f ) << num2;
				num2 += 7;
				if( ( num3 & 0x80 ) == 0 )
					return num1;
			}
		}

		///// <summary>
		///// Reads a float written using WriteSignedSingle()
		///// </summary>
		//public float ReadSignedSingle( int numberOfBits )
		//{
		//	if( overflow || bitPosition + numberOfBits > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}

		//	uint encodedVal = ReadUInt32( numberOfBits );
		//	int maxVal = ( 1 << numberOfBits ) - 1;
		//	return ( (float)( encodedVal + 1 ) / (float)( maxVal + 1 ) - 0.5f ) * 2.0f;
		//}

		///// <summary>
		///// Reads a float written using WriteUnitSingle()
		///// </summary>
		//public float ReadUnitSingle( int numberOfBits )
		//{
		//	if( overflow || bitPosition + numberOfBits > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}

		//	uint encodedVal = ReadUInt32( numberOfBits );
		//	int maxVal = ( 1 << numberOfBits ) - 1;
		//	return (float)( encodedVal + 1 ) / (float)( maxVal + 1 );
		//}

		///// <summary>
		///// Reads a float written using WriteRangedSingle() using the same MIN and MAX values
		///// </summary>
		//public float ReadRangedSingle( float min, float max, int numberOfBits )
		//{
		//	if( overflow || bitPosition + numberOfBits > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}
		//	float range = max - min;
		//	int maxVal = ( 1 << numberOfBits ) - 1;
		//	float encodedVal = (float)ReadUInt32( numberOfBits );
		//	float unit = encodedVal / (float)maxVal;
		//	return min + ( unit * range );
		//}

		//static int BitsToHoldUInt( uint value )
		//{
		//	int bits = 1;
		//	while( ( value >>= 1 ) != 0 )
		//		bits++;
		//	return bits;
		//}

		///// <summary>
		///// Reads an integer written using WriteRangedInteger() using the same min/max values
		///// </summary>
		//public int ReadRangedInteger( int min, int max )
		//{
		//	uint range = (uint)( max - min );
		//	int numBits = BitsToHoldUInt( range );
		//	if( overflow || bitPosition + numBits > endBitPosition )
		//	{
		//		overflow = true;
		//		return 0;
		//	}
		//	uint rvalue = ReadUInt32( numBits );
		//	return (int)( min + rvalue );
		//}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector2F( out Vector2F result )
		{
			result.X = ReadSingle();
			result.Y = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector2F ReadVector2F()
		{
			return new Vector2F( ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadRangeF( out RangeF result )
		{
			result.Minimum = ReadSingle();
			result.Maximum = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public RangeF ReadRangeF()
		{
			return new RangeF( ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector3F( out Vector3F result )
		{
			result.X = ReadSingle();
			result.Y = ReadSingle();
			result.Z = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector3F ReadVector3F()
		{
			return new Vector3F( ReadSingle(), ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector4F( out Vector4F result )
		{
			result.X = ReadSingle();
			result.Y = ReadSingle();
			result.Z = ReadSingle();
			result.W = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector4F ReadVector4F()
		{
			return new Vector4F( ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadBoundsF( out BoundsF result )
		{
			result.Minimum.X = ReadSingle();
			result.Minimum.Y = ReadSingle();
			result.Minimum.Z = ReadSingle();
			result.Maximum.X = ReadSingle();
			result.Maximum.Y = ReadSingle();
			result.Maximum.Z = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public BoundsF ReadBoundsF()
		{
			return new BoundsF( ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadQuaternionF( out QuaternionF result )
		{
			result.X = ReadSingle();
			result.Y = ReadSingle();
			result.Z = ReadSingle();
			result.W = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public QuaternionF ReadQuaternionF()
		{
			return new QuaternionF( ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle() );
		}

		//public QuaternionF ReadQuatertionF( int bitsPerElement )
		//{
		//	return new QuaternionF(
		//		ReadRangedSingle( -1, 1, bitsPerElement ),
		//		ReadRangedSingle( -1, 1, bitsPerElement ),
		//		ReadRangedSingle( -1, 1, bitsPerElement ),
		//		ReadRangedSingle( -1, 1, bitsPerElement ) );
		//}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadColorValue( out ColorValue result )
		{
			result.Red = ReadSingle();
			result.Green = ReadSingle();
			result.Blue = ReadSingle();
			result.Alpha = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public ColorValue ReadColorValue()
		{
			return new ColorValue( ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadSphericalDirectionF( out SphericalDirectionF result )
		{
			result.Horizontal = ReadSingle();
			result.Vertical = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public SphericalDirectionF ReadSphericalDirectionF()
		{
			return new SphericalDirectionF( ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector2I( out Vector2I result )
		{
			result.X = ReadInt32();
			result.Y = ReadInt32();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector2I ReadVector2I()
		{
			return new Vector2I( ReadInt32(), ReadInt32() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector3I( out Vector3I result )
		{
			result.X = ReadInt32();
			result.Y = ReadInt32();
			result.Z = ReadInt32();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector3I ReadVector3I()
		{
			return new Vector3I( ReadInt32(), ReadInt32(), ReadInt32() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector4I( out Vector4I result )
		{
			result.X = ReadInt32();
			result.Y = ReadInt32();
			result.Z = ReadInt32();
			result.W = ReadInt32();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector4I ReadVector4I()
		{
			return new Vector4I( ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadRectangleF( out RectangleF result )
		{
			result.Left = ReadSingle();
			result.Top = ReadSingle();
			result.Right = ReadSingle();
			result.Bottom = ReadSingle();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public RectangleF ReadRectangleF()
		{
			return new RectangleF( ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadRectangleI( out RectangleI result )
		{
			result.Left = ReadInt32();
			result.Top = ReadInt32();
			result.Right = ReadInt32();
			result.Bottom = ReadInt32();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public RectangleI ReadRectangleI()
		{
			return new RectangleI( ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public DegreeF ReadDegreeF()
		{
			return new DegreeF( ReadSingle() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public RadianF ReadRadianF()
		{
			return new RadianF( ReadSingle() );
		}

		[MethodImpl( (MethodImplOptions)512 )]
		public string ReadString()
		{
			//int length = ReadVariableInt32();
			//if( length == -1 )
			//	return null;
			//if( length == 0 )
			//	return string.Empty;

			//int newPosition = currentPosition + length;
			//if( overflow || newPosition > endPosition )
			//{
			//	overflow = true;
			//	return "";
			//}
			//var result = Encoding.UTF8.GetString( data, currentPosition, length );
			//currentPosition = newPosition;

			//return result;


			int length = (int)ReadVariableUInt32();
			if( length == 0 )
				return "";

			int newPosition = currentPosition + length;
			if( overflow || newPosition > endPosition )
			{
				overflow = true;
				return "";
			}
			var result = Encoding.UTF8.GetString( data, currentPosition, length );
			currentPosition = newPosition;

			return result;


			////if( overflow || bitPosition + byteLength * 8 > endBitPosition )
			////{
			////	overflow = true;
			////	return "";
			////}

			////if( ( bitPosition & 7 ) == 0 )
			////{
			////	//read directly
			////	string result = System.Text.Encoding.UTF8.GetString( data, bitPosition >> 3, byteLength );
			////	bitPosition += ( byteLength * 8 );
			////	return result;
			////}

			////byte[] bytes = new byte[ byteLength ];
			////ReadBuffer( bytes );
			////if( overflow )
			////	return "";
			////return System.Text.Encoding.UTF8.GetString( bytes, 0, bytes.Length );
		}

		[MethodImpl( (MethodImplOptions)512 )]
		public string ReadStringWithNullSupport()
		{
			int length = ReadVariableInt32();
			if( length == -1 )
				return null;
			if( length == 0 )
				return string.Empty;

			int newPosition = currentPosition + length;
			if( overflow || newPosition > endPosition )
			{
				overflow = true;
				return "";
			}
			var result = Encoding.UTF8.GetString( data, currentPosition, length );
			currentPosition = newPosition;

			return result;
		}

		///// <summary>
		///// Pads data with enough bits to reach a full byte. Decreases cpu usage for subsequent byte writes.
		///// </summary>
		//public void SkipPadBits()
		//{
		//	bitPosition = ( ( bitPosition + 7 ) / 8 ) * 8;
		//}

		///// <summary>
		///// Pads data with the specified number of bits.
		///// </summary>
		//public void SkipPadBits( int numberOfBits )
		//{
		//	bitPosition += numberOfBits;
		//}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector2( out Vector2 result )
		{
			result.X = ReadDouble();
			result.Y = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector2 ReadVector2()
		{
			return new Vector2( ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadRange( out Range result )
		{
			result.Minimum = ReadDouble();
			result.Maximum = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Range ReadRange()
		{
			return new Range( ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector3( out Vector3 result )
		{
			result.X = ReadDouble();
			result.Y = ReadDouble();
			result.Z = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector3 ReadVector3()
		{
			return new Vector3( ReadDouble(), ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadVector4( out Vector4 result )
		{
			result.X = ReadDouble();
			result.Y = ReadDouble();
			result.Z = ReadDouble();
			result.W = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Vector4 ReadVector4()
		{
			return new Vector4( ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadBounds( out Bounds result )
		{
			result.Minimum.X = ReadDouble();
			result.Minimum.Y = ReadDouble();
			result.Minimum.Z = ReadDouble();
			result.Maximum.X = ReadDouble();
			result.Maximum.Y = ReadDouble();
			result.Maximum.Z = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Bounds ReadBounds()
		{
			return new Bounds( ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadQuaternion( out Quaternion result )
		{
			result.X = ReadDouble();
			result.Y = ReadDouble();
			result.Z = ReadDouble();
			result.W = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Quaternion ReadQuaternion()
		{
			return new Quaternion( ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadSphericalDirection( out SphericalDirection result )
		{
			result.Horizontal = ReadDouble();
			result.Vertical = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public SphericalDirection ReadSphericalDirection()
		{
			return new SphericalDirection( ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public void ReadRectangle( out Rectangle result )
		{
			result.Left = ReadDouble();
			result.Top = ReadDouble();
			result.Right = ReadDouble();
			result.Bottom = ReadDouble();
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Rectangle ReadRectangle()
		{
			return new Rectangle( ReadDouble(), ReadDouble(), ReadDouble(), ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Degree ReadDegree()
		{
			return new Degree( ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public Radian ReadRadian()
		{
			return new Radian( ReadDouble() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public DateTime ReadDateTime()
		{
			return new DateTime( ReadInt64() );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public unsafe Vector2H ReadVector2H()
		{
			Vector2H result;
			ReadBuffer( &result, sizeof( Vector2H ) );
			return result;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public unsafe Vector3H ReadVector3H()
		{
			Vector3H result;
			ReadBuffer( &result, sizeof( Vector3H ) );
			return result;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public unsafe Vector4H ReadVector4H()
		{
			Vector4H result;
			ReadBuffer( &result, sizeof( Vector4H ) );
			return result;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public unsafe QuaternionH ReadQuaternionH()
		{
			QuaternionH result;
			ReadBuffer( &result, sizeof( QuaternionH ) );
			return result;
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining | (MethodImplOptions)512 )]
		public unsafe HalfType ReadHalf()
		{
			HalfType result;
			ReadBuffer( &result, sizeof( HalfType ) );
			return result;
		}

		//!!!!more types
	}
}
