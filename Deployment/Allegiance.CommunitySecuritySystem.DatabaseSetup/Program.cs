/******************************************************************************************************************************
 * BT - 10/21/2011
 * This was used to restore my local DB after I reinstalled my computer. 
 * Normally, you wouldn't run this application unless you are setting up a local development database for the first
 * time, and there are a lot of changes in the change scripts directory.
 ******************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;

namespace Allegiance.CommunitySecuritySystem.DatabaseSetup
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine("Usage: DatabaseSetup <create scripts directory> <change scripts directory>");
				Console.WriteLine(@"Example: DatabaseSetup ""C:\Source\Allegiance\CSS\Data\Allegiance.CommunitySecuritySystem.Database\Create Scripts"", ""C:\Source\Allegiance\CSS\Data\Allegiance.CommunitySecuritySystem.Database\Change Scripts""");

				return -1;
			}

			SqlConnection conn = new SqlConnection(Configuration.Default.ConnectionString);
			Server server = new Server(new ServerConnection(conn));

			try
			{
				Console.WriteLine("Dropping  database.");
				int results = server.ConnectionContext.ExecuteNonQuery(@"
USE MASTER
GO

ALTER DATABASE CSS
SET Single_USER
WITH ROLLBACK IMMEDIATE;


ALTER DATABASE CSS
SET Multi_USER
WITH ROLLBACK IMMEDIATE;


DROP DATABASE CSS;

create database CSS;

ALTER DATABASE CSSStats
SET Single_USER
WITH ROLLBACK IMMEDIATE;


ALTER DATABASE CSSStats
SET Multi_USER
WITH ROLLBACK IMMEDIATE;


DROP DATABASE CSSStats;

create database CSSStats;

-- ----------------------
-- CSSStats
-- ----------------------

USE CSSStats;
GO

sp_adduser 'css_server', 'css_server';
GO

sp_addrolemember 'db_datareader', 'css_server'
GO

sp_addrolemember 'db_datawriter', 'css_server'
GO

-- ----------------------
-- CSS
-- ----------------------
use CSS;
GO

sp_adduser 'css_server', 'css_server';
GO

sp_addrolemember 'db_datareader', 'css_server'
GO

sp_addrolemember 'db_datawriter', 'css_server'
GO


			", ExecutionTypes.Default);
			}
			catch (ExecutionFailureException ex)
			{
				Console.WriteLine(ex.InnerException);
			}

			ProcessDirectory(server, args[0]);
			ProcessDirectory(server, args[1]);

			return 0;
		}

		private static void ProcessDirectory(Server server, string directory)
		{
			foreach (string file in Directory.GetFiles(directory))
			{
				Console.WriteLine("Executing: " + file);
				string commandText = File.ReadAllText(file);

				try
				{
					// Always reset to default database.
					server.ConnectionContext.ExecuteNonQuery("USE CSS");

					server.ConnectionContext.ExecuteNonQuery(commandText);
				}
				catch (ExecutionFailureException ex)
				{
					Console.WriteLine(ex.InnerException);
					throw;
				}
			}
		}
	}
}
