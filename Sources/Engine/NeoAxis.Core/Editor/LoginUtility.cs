﻿//#if !DEPLOY
// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
//using Microsoft.Win32;

namespace NeoAxis.Editor
{
	public static class LoginUtility
	{
		const string registryPath = @"HKEY_CURRENT_USER\SOFTWARE\NeoAxis";
		//const string registryPath = @"SOFTWARE\NeoAxis";

		static volatile string requestedFullLicenseInfo_License = "";
		static volatile ESet<string> requestedFullLicenseInfo_PurchasedProducts = new ESet<string>();
		//static volatile string requestedFullLicenseInfo_TokenTransactions = "";
		static volatile string requestedFullLicenseInfo_Error = "";
		//static double licenseInfoLastUpdateTime;

		static string licenseCached;

		//static string machineId;

		//

		public static bool GetCurrentLicense( out string email, out string hash )
		{
#if !DEPLOY
			try
			{
				email = PlatformSpecificUtility.Instance.GetRegistryValue( registryPath, "LoginEmail", "" ) as string;
				hash = PlatformSpecificUtility.Instance.GetRegistryValue( registryPath, "LoginHash", "" ) as string;
				if( !string.IsNullOrEmpty( hash ) )
					hash = EncryptDecrypt( hash );

				if( !string.IsNullOrEmpty( email ) && !string.IsNullOrEmpty( hash ) )
					return true;

				////opening the subkey  
				//var key = Registry.CurrentUser.OpenSubKey( registryPath );

				////if it does exist, retrieve the stored values  
				//if( key != null )
				//{
				//	email = ( key.GetValue( "LoginEmail" ) ?? "" ).ToString();
				//	var p = key.GetValue( "LoginHash" );
				//	if( p != null )
				//		hash = EncryptDecrypt( p.ToString() );
				//	else
				//		hash = "";
				//	//hash = ( key.GetValue( "LoginHash" ) ?? "" ).ToString();
				//	key.Close();
				//	return true;
				//}
			}
			catch { }
#endif

			email = "";
			hash = "";
			return false;
		}

		internal static string EncryptDecrypt( string input )
		{
			char[] key = { 'K', 'C', 'Q' }; //Any chars will work, in an array of any size
			char[] output = new char[ input.Length ];

			for( int i = 0; i < input.Length; i++ )
				output[ i ] = (char)( input[ i ] ^ key[ i % key.Length ] );

			return new string( output );
		}

		public static void SetCurrentLicense( string email, string hash )
		{
			try
			{
				PlatformSpecificUtility.Instance.SetRegistryValue( registryPath, "LoginEmail", email );
				PlatformSpecificUtility.Instance.SetRegistryValue( registryPath, "LoginHash", EncryptDecrypt( hash ) );

				//var key = Registry.CurrentUser.CreateSubKey( registryPath );
				//key.SetValue( "LoginEmail", email );
				//key.SetValue( "LoginHash", EncryptDecrypt( hash ) );
				//key.Close();
			}
			catch( Exception e )
			{
				EditorMessageBox.ShowWarning( e.Message );
				return;
			}

			RequestFullLicenseInfo();
		}

		static void ThreadGetLicense( object param )
		{
			try
			{
				var param2 = (Dictionary<string, string>)param;
				var email = param2[ "Email" ];
				var hash = param2[ "Hash" ];

				var email64 = StringUtility.EncodeToBase64URL( email );
				var hash64 = StringUtility.EncodeToBase64URL( hash );
				//var email64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( email/*.ToLower()*/ ) ).Replace( "=", "" );
				//var hash64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( hash ) ).Replace( "=", "" );

				string data = $"email={email64}&hash={hash64}";
				byte[] dataStream = Encoding.UTF8.GetBytes( data );

				{
					WebRequest request = WebRequest.Create( EngineInfo.StoreAddress + @"/api/get_user_info2/" );
					request.Method = "POST";
					request.ContentType = "application/x-www-form-urlencoded";
					request.ContentLength = dataStream.Length;
					Stream newStream = request.GetRequestStream();
					newStream.Write( dataStream, 0, dataStream.Length );
					newStream.Close();

					string xml;
					using( var response = (HttpWebResponse)request.GetResponse() )
					using( var stream = response.GetResponseStream() )
					using( var reader = new StreamReader( stream ) )
						xml = reader.ReadToEnd();

					if( !string.IsNullOrEmpty( xml ) )
					{
						var xDoc = new XmlDocument();
						xDoc.LoadXml( xml );

						requestedFullLicenseInfo_Error = "";

						if( xDoc.DocumentElement != null )
						{
							foreach( XmlNode child in xDoc.DocumentElement.ChildNodes )
							{
								if( child.Name == "license" )
									requestedFullLicenseInfo_License = child.InnerText;

								if( child.Name == "purchased_products" )
								{
									var products = child.InnerText.Trim().Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
									foreach( var product in products )
										requestedFullLicenseInfo_PurchasedProducts.AddWithCheckAlreadyContained( product );
								}
							}
						}
						//foreach( XmlNode child in xDoc.ChildNodes )
						//{
						//	if( child.Name == "license" )
						//		requestedFullLicenseInfo_License = child.InnerText;
						//}
					}
					else
					{
						requestedFullLicenseInfo_License = "";
						requestedFullLicenseInfo_Error = "Invalid username or password.";
					}
				}

				//{
				//	WebRequest request = WebRequest.Create( @"https://www.neoaxis.com/api/get_license/" );
				//	//WebRequest request = WebRequest.Create( @"https://store.neoaxis.com/api/check_login/" );
				//	request.Method = "POST";
				//	request.ContentType = "application/x-www-form-urlencoded";
				//	request.ContentLength = dataStream.Length;
				//	Stream newStream = request.GetRequestStream();
				//	newStream.Write( dataStream, 0, dataStream.Length );
				//	newStream.Close();

				//	string text;
				//	using( var response = (HttpWebResponse)request.GetResponse() )
				//	using( var stream = response.GetResponseStream() )
				//	using( var reader = new StreamReader( stream ) )
				//		text = reader.ReadToEnd();

				//	requestedFullLicenseInfo_Error = "";
				//	if( !string.IsNullOrEmpty( text ) )
				//		requestedFullLicenseInfo_License = text;
				//	//if( text.Contains( "Pro" ) )
				//	//	requestedFullLicenseInfo_License = "Pro";
				//	//else if( text.Contains( "Personal" ) )
				//	//	requestedFullLicenseInfo_License = "Personal";
				//	//else if( !string.IsNullOrEmpty( text ) )
				//	//	requestedFullLicenseInfo_License = text;
				//	//if( text.Contains( "True" ) )
				//	//	requestedFullLicenseInfo_License = "Registered";
				//	else
				//	{
				//		requestedFullLicenseInfo_License = "";
				//		requestedFullLicenseInfo_Error = "Invalid username or password.";
				//	}
				//}

				////get purchased products
				//{
				//	WebRequest request = WebRequest.Create( @"https://store.neoaxis.com/api/get_purchased_products/" );
				//	request.Method = "POST";
				//	request.ContentType = "application/x-www-form-urlencoded";
				//	request.ContentLength = dataStream.Length;
				//	Stream newStream = request.GetRequestStream();
				//	newStream.Write( dataStream, 0, dataStream.Length );
				//	newStream.Close();

				//	string text;
				//	using( var response = (HttpWebResponse)request.GetResponse() )
				//	using( var stream = response.GetResponseStream() )
				//	using( var reader = new StreamReader( stream ) )
				//		text = reader.ReadToEnd();

				//	var products = text.Trim().Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
				//	foreach( var product in products )
				//		requestedFullLicenseInfo_PurchasedProducts.AddWithCheckAlreadyContained( product );
				//}

				////get token transactions
				//{
				//	WebRequest request = WebRequest.Create( @"https://store.neoaxis.com/api/get_token_transactions/" );
				//	request.Method = "POST";
				//	request.ContentType = "application/x-www-form-urlencoded";
				//	request.ContentLength = dataStream.Length;
				//	Stream newStream = request.GetRequestStream();
				//	newStream.Write( dataStream, 0, dataStream.Length );
				//	newStream.Close();

				//	string text;
				//	using( var response = (HttpWebResponse)request.GetResponse() )
				//	using( var stream = response.GetResponseStream() )
				//	using( var reader = new StreamReader( stream ) )
				//		text = reader.ReadToEnd();

				//	requestedFullLicenseInfo_TokenTransactions = text;
				//}

				//SaveLicenseCertificate2();
				//EngineApp.needReadLicenseCertificate = true;


				licenseCached = null;
			}
			catch //(Exception e)
			{
				//Log.Warning( e.Message );
			}
		}

		public static void RequestFullLicenseInfo()
		{
			requestedFullLicenseInfo_License = "";
			requestedFullLicenseInfo_PurchasedProducts.Clear();
			//requestedFullLicenseInfo_TokenTransactions = "";

			if( !GetCurrentLicense( out var email, out var hash ) )
				return;

			var param = new Dictionary<string, string>();
			param[ "Email" ] = email;
			param[ "Hash" ] = hash;

			var thread1 = new Thread( ThreadGetLicense );
			thread1.IsBackground = true;
			thread1.Start( param );
		}

		public static bool GetRequestedFullLicenseInfo( out string license, out ESet<string> purchasedProducts, /*out string tokenTransactions, */out string error )
		{
			if( !string.IsNullOrEmpty( requestedFullLicenseInfo_License ) || !string.IsNullOrEmpty( requestedFullLicenseInfo_Error ) )
			{
				license = requestedFullLicenseInfo_License;
				purchasedProducts = requestedFullLicenseInfo_PurchasedProducts;
				//tokenTransactions = requestedFullLicenseInfo_TokenTransactions;
				error = requestedFullLicenseInfo_Error;
				//pro = requestedFullLicenseInfo_License.Contains( "Pro" );
				return true;
			}
			else
			{
				license = "";
				purchasedProducts = new ESet<string>();
				//tokenTransactions = "";
				error = "";
				//pro = false;
				return false;
			}
		}

		//public static bool SaveLicenseCertificate( string realFileName, string email, string engineVersion, string license, string machineId, DateTime expirationDate, out string error )
		//{
		//	//!!!!на сервере генерировать

		//	var email2 = email.ToLower();

		//	//!!!!не так. GetHashCode может поменяться
		//	//CRC32
		//	var verification = "Check";
		//	//var verification = email2.GetHashCode() ^ engineVersion.GetHashCode() ^ license.GetHashCode() ^ expirationDate.GetHashCode();

		//	string str = "NeoAxis Certificate version 1";
		//	str += "\n" + email2;
		//	str += "\n" + engineVersion;
		//	str += "\n" + license;
		//	str += "\n" + machineId;
		//	str += "\n" + expirationDate.ToString( "MM/dd/yyyy" );
		//	str += "\n" + verification;

		//	var base64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( str ) );

		//	try
		//	{
		//		File.WriteAllText( realFileName, base64 );
		//	}
		//	catch( Exception e )
		//	{
		//		error = e.Message;
		//		return false;
		//	}

		//	error = "";
		//	return true;
		//}

		//		public static string GetMachineId()
		//		{
		//#if !DEPLOY
		//			if( string.IsNullOrEmpty( machineId ) )
		//			{
		//				try
		//				{
		//					var mc = new System.Management.ManagementClass( "win32_processor" );
		//					foreach( System.Management.ManagementObject mo in mc.GetInstances() )
		//					{
		//						var cpuInfo = mo.Properties[ "processorID" ].Value.ToString();
		//						machineId = cpuInfo;
		//						break;
		//					}
		//				}
		//				catch { }
		//			}
		//#endif

		//			return machineId;
		//		}

		//static PhysicalAddress GetMacAddress2()
		//{
		//	foreach( NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces() )
		//	{
		//		// Only consider Ethernet network interfaces
		//		if( nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up )
		//			return nic.GetPhysicalAddress();
		//	}
		//	return null;
		//}

		//public static string GetMacAddress()
		//{
		//	try
		//	{
		//		return GetMacAddress2().ToString();
		//	}
		//	catch
		//	{
		//		return "";
		//	}
		//}

		//static bool SaveLicenseCertificate2()
		//{
		//	var fileName = Path.Combine( VirtualFileSystem.Directories.Project, "License.cert" );

		//	if( File.Exists( fileName ) )
		//		File.Delete( fileName );

		//	if( !GetCurrentLicense( out var email, out _ ) )
		//		return false;
		//	if( !GetRequestedFullLicenseInfo( out var license, out var error2 ) )
		//		return false;

		//	var date = DateTime.UtcNow;
		//	//add 5 days
		//	date = date.Add( new TimeSpan( 5, 0, 0, 0, 0 ) );

		//	if( !SaveLicenseCertificate( fileName, email, EngineInfo.Version, license, GetMachineId(), date, out var error ) )
		//	{
		//		EditorMessageBox.ShowWarning( "Unable to save 'License.cert'. " + error );
		//		return false;
		//	}

		//	return true;
		//}

		//public static bool ReadLicenseCertificate( string realFileName, out string error, out string email, out string engineVersion, out string license, out string machineId, out DateTime expirationDate )
		//{
		//	error = "";

		//	try
		//	{
		//		var base64 = File.ReadAllText( realFileName );
		//		var text = Encoding.UTF8.GetString( Convert.FromBase64String( base64 ) );
		//		var lines = text.Split( new char[] { '\n' }, StringSplitOptions.None );

		//		var format = lines[ 0 ];

		//		email = lines[ 1 ];
		//		engineVersion = lines[ 2 ];
		//		license = lines[ 3 ];
		//		machineId = lines[ 4 ];
		//		expirationDate = DateTime.ParseExact( lines[ 5 ], "MM/dd/yyyy", null );

		//		var verification = lines[ 6 ];

		//		//!!!!verification
		//		//!!!!!error

		//		return true;
		//	}
		//	catch( Exception e )
		//	{
		//		error = e.Message;
		//		email = "";
		//		expirationDate = new DateTime( 0 );
		//		engineVersion = "";
		//		license = "";
		//		machineId = "";
		//		return false;
		//	}
		//}

		public static string GetLicenseCached()
		{
			if( licenseCached == null )
			{
				licenseCached = "";
				if( GetCurrentLicense( out _, out _ ) )
				{
					if( GetRequestedFullLicenseInfo( out var license, out _, /*out _,*/ out _ ) )
						licenseCached = license;
				}
			}

			var result = licenseCached;
			if( result == null )
				result = "";
			return result;
		}

	}
}
//#endif