#region copyright
/*
Copyright �2009 John Allberg

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
#endregion
// $Id: CDatabaseUpgrade.cs 121 2011-05-28 16:02:14Z smuda $
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;

namespace Allberg.Shooter.Common
{
	class CDatabaseUpgrade
	{
		internal CDatabaseUpgrade(OleDbConnection DbConn, CDatabase DatabaseClass)
		{
			dbconn = DbConn;
			helper = DatabaseClass;
		}

		OleDbConnection dbconn;
		CDatabase helper;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
			"CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
		internal void Upgrade()
		{
			System.Version version = new Version("1.0.0");

			try
			{
				OleDbCommand cmd = new OleDbCommand("select * from DbInfo", dbconn);
				OleDbDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.Default);

				while (reader.Read())
				{
					if (reader.GetString(reader.GetOrdinal("KeyName")) == "Version")
					{
						version = new Version(reader.GetString(reader.GetOrdinal("KeyValue")));
					}
				}
				//version = new Version("1.1.0");
				reader.Close();
			}
			catch (OleDbException exc)
			{
				// Older version than 1.1
				if (exc.Message.IndexOf("cannot find the input table") == -1)
				{
					Trace.WriteLine("Ops, unknown OleDbException while checking for db upgrade:\r\n" +
						exc.ToString());
				}
			}

			if (version < new Version("1.1.0"))
			{
				UpgradeVersion1_1();
			}

			if (version < new Version("1.2.0"))
			{
				UpgradeVersion1_2();
			}


			if (version < new Version("1.4.0"))
			{
				UpgradeVersion1_4();
			}

			if (version < new Version("1.5.6"))
			{
				UpgradeVersion1_5_6();
			}

			if (version < new Version("1.6.0"))
			{
				UpgradeVersion1_6_0();
			}

			if (version < new Version("1.6.2"))
			{
				UpgradeVersion1_6_2();
			}

			// Finished upgrading database
			OleDbCommand SqlCmd = null;
			try
			{
				if (new Version(CDatabase.CurrentDbVersion) > version)
				{
					SqlCmd = new OleDbCommand(
						"update DbInfo set KeyValue='" + 
							CDatabase.CurrentDbVersion + 
							"' where KeyName='Version'",
						dbconn);
					int nrAffected = SqlCmd.ExecuteNonQuery();
					if (nrAffected == 0)
					{
						SqlCmd = new OleDbCommand(
							"insert into DbInfo (KeyName, KeyValue) values ('Version', '" + 
								CDatabase.CurrentDbVersion + "')",
							dbconn);
						nrAffected = SqlCmd.ExecuteNonQuery();
						SqlCmd.Dispose();
					}
				}
			}
			catch (Exception exc)
			{
				Trace.WriteLine("Exception while updating/inserting the version value of the DbInfo table:" +
					exc.ToString());
				throw;
			}
			finally
			{
				if (SqlCmd != null)
				{
					SqlCmd.Dispose();
				}
			}
		}

		#region Upgraders
		#region Version 1.1.0
		private void UpgradeVersion1_1()
		{

			if (checkTableExist("Teams", dbconn))
			{
				dropTable("Teams", dbconn);
			}
			// Create table Teams
			CDatabase.CreateTable(new DatabaseDataset.TeamsDataTable(), dbconn);

			string sqlCreate;
			OleDbCommand SQL;

			// Create restrictions for table Teams
			DatabaseDataset dbtemp = new DatabaseDataset();
			foreach (System.Data.DataRelation rel in dbtemp.Relations)
			{
				if (rel.RelationName.IndexOf("Teams") > -1)
				{
					sqlCreate = "ALTER TABLE " + rel.ChildTable.TableName + " ADD " +
						"CONSTRAINT " + rel.RelationName + " FOREIGN KEY " +
						"(" + rel.ChildColumns[0].ColumnName + ")" +
						" REFERENCES " + rel.ParentTable +
						" (" + rel.ParentColumns[0].ColumnName +
						")";

					// Execute against database
					Trace.WriteLine("CDatabase: Running SQL to create relation: " + sqlCreate);
					SQL = new OleDbCommand(sqlCreate, dbconn);
					SQL.ExecuteNonQuery();
					SQL.Dispose();
				}
			}

			// Create table DbInfo
			CDatabase.CreateTable(new DatabaseDataset.DbInfoDataTable(), dbconn);

			// Change shooter table to include "arrived" column
			addColumnToTable("shooters", "Arrived", "bit null", dbconn);
			addColumnToTable("shooters", "EmailResult", "bit null", dbconn);
		}
		#endregion
		#region Version 1.2.0
		private void UpgradeVersion1_2()
		{
			OleDbCommand SqlCmd = null;
			try
			{
				// Add competition type
				addColumnToTable("competition", "Type", "int null", dbconn);
				string sqlUpdate = "update competition set Type=0";

				Trace.WriteLine("CDatabase: Running SQL to set competition type: " + sqlUpdate);
				SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
				SqlCmd.ExecuteNonQuery();

				// Add stationId to table CompetitorResults
				addColumnToTable("CompetitorResults", "StationId", "int null", dbconn);
				sqlUpdate = "update CompetitorResults set StationId=StationNr";

				Trace.WriteLine("CDatabase: Running SQL to set StationId=StationNr: " + sqlUpdate);
				SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
				SqlCmd.ExecuteNonQuery();

				addConstraint("StationsCompetitorResults", new DatabaseDataset(), dbconn);
			}
			finally
			{
				SqlCmd.Dispose();
			}
		}
		#endregion
		#region Version 1.4.0
		private void UpgradeVersion1_4()
		{
			// Add competition type
			addColumnToTable("Teams", "CompetitorId5", "int null", dbconn);
			addConstraint("CompetitorsTeams5", new DatabaseDataset(), dbconn);

			addColumnToTable("Competition", "Championship", "int null", dbconn);
			string sqlUpdate = "update competition set Championship=0";

			Trace.WriteLine("CDatabase: Running SQL to set competition Championship: " + sqlUpdate);
			OleDbCommand SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();
		}
		#endregion
		#region Version 1.5.6
		private void UpgradeVersion1_5_6()
		{
			addColumnToTable("Competition", "PatrolConnectionType", "int null", dbconn);
			string sqlUpdate = "update competition set PatrolConnectionType=1";

			Trace.WriteLine("CDatabase: Running SQL to set competition patrolConnectionType: " + sqlUpdate);
			OleDbCommand SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();
		}
		#endregion
		#region Version 1.6.0
		private void UpgradeVersion1_6_0()
		{
			string sqlUpdate;
			OleDbCommand SqlCmd;

			// Add shooterFee
			for (int i = 1; i <= 4; i++)
			{
				addColumnToTable("Competition", "ShooterFee" + i.ToString(), "int null", dbconn);

				sqlUpdate = "update competition set ShooterFee" + i.ToString() + "=ShooterFee";
				Trace.WriteLine("CDatabase: Running SQL to set competition shooterFee: " + sqlUpdate);
				SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
				SqlCmd.ExecuteNonQuery();
			}

			// Add lastupdate
			addColumnToTable("Shooters", "LastUpdate", "DateTime null", dbconn);
			addColumnToTable("Clubs", "LastUpdate", "DateTime null", dbconn);
			addColumnToTable("Weapons", "LastUpdate", "DateTime null", dbconn);

			sqlUpdate = "update shooters set LastUpdate='2004-01-01 10:00:00'";
			Trace.WriteLine("CDatabase: Running SQL to set shooters lastupdate: " + sqlUpdate);
			SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();

			sqlUpdate = "update clubs set LastUpdate='2004-01-01 10:00:00'";
			Trace.WriteLine("CDatabase: Running SQL to set clubs lastupdate: " + sqlUpdate);
			SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();

			sqlUpdate = "update weapons set LastUpdate='2004-01-01 10:00:00'";
			Trace.WriteLine("CDatabase: Running SQL to set weapons lastupdate: " + sqlUpdate);
			SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();

			// Add one to competition championship type
			sqlUpdate = "update competition set Championship=Championship+1";
			Trace.WriteLine("CDatabase: Running SQL to update ChampionShipType: " + sqlUpdate);
			SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();
		}
		#endregion
		#region 1.6.2
		private void UpgradeVersion1_6_2()
		{
			addColumnToTable("Stations", "Distinguish", "bit null", dbconn);
			addColumnToTable("Competition", "OneClass", "bit null", dbconn);

			string sqlUpdate;
			OleDbCommand SqlCmd;

			// Set Distinguish=0
			sqlUpdate = "update Stations set Distinguish=0";
			Trace.WriteLine("CDatabase: Running SQL to update Stations: " + sqlUpdate);
			SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();

			// Set OneClass=0
			sqlUpdate = "update Competition set OneClass=0";
			Trace.WriteLine("CDatabase: Running SQL to update Competitions: " + sqlUpdate);
			SqlCmd = new OleDbCommand(sqlUpdate, dbconn);
			SqlCmd.ExecuteNonQuery();

		}
		#endregion
		#endregion

		#region Helpers
		private static bool checkTableExist(string tableName, System.Data.OleDb.OleDbConnection dbconn)
		{
			try
			{
				System.Data.OleDb.OleDbCommand cmd = new OleDbCommand("select * from " + tableName, dbconn);
				System.Data.OleDb.OleDbDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.Default);
				reader.Close();
				return true;
			}
			catch (System.Data.OleDb.OleDbException exc)
			{
				// Older version than 1.1
				if (exc.Message.IndexOf("cannot find the input table") == -1)
				{
					Trace.WriteLine("Ops, unknown System.Data.OleDb.OleDbException while checking for db upgrade:\r\n" +
						exc.ToString());
				}
				return false;
			}
		}


		private bool dropTable(string tableName, System.Data.OleDb.OleDbConnection dbconn)
		{
			try
			{
				System.Data.OleDb.OleDbCommand cmd = new OleDbCommand("drop table " + tableName, dbconn);
				cmd.ExecuteNonQuery();
				return true;
			}
			catch (System.Data.OleDb.OleDbException exc)
			{
				// Older version than 1.1
				if (exc.Message.IndexOf("cannot find the input table") == -1)
				{
					Trace.WriteLine("Ops, unknown System.Data.OleDb.OleDbException while checking for db upgrade:\r\n" +
						exc.ToString());
				}
				return false;
			}
		}

		private static void addColumnToTable(string tablename, string columnname,
			string columntype, OleDbConnection dbconn)
		{
			try
			{
				string sqlCreate = "ALTER TABLE " + tablename + " " +
					"ADD " + columnname + " " + columntype;

				// Execute against database
				Trace.WriteLine("CDatabase: Running SQL to add column \"" +
					columnname + "\" to table \"" + tablename + "\": " + sqlCreate);
				OleDbCommand SQL = new OleDbCommand(sqlCreate, dbconn);
				SQL.ExecuteNonQuery();
				SQL.Dispose();
			}
			catch (OleDbException exc)
			{
				if (exc.Message.IndexOf("already exists in table") == -1)
					throw exc;
			}
		}

		private static void addConstraint(string constraintName, DatabaseDataset database, OleDbConnection conn)
		{
			Trace.WriteLine("CDatabase: Entering createDatabaseConstraints()");

			StringBuilder sqlCreate = new StringBuilder("");

			foreach (System.Data.DataRelation rel in database.Relations)
			{
				if (rel.RelationName == constraintName)
				{
					sqlCreate = new StringBuilder("");
					sqlCreate.Append("ALTER TABLE " + rel.ChildTable.TableName + " ADD " +
						"CONSTRAINT " + rel.RelationName + " FOREIGN KEY " +
						"(" + rel.ChildColumns[0].ColumnName + ")" +
						" REFERENCES " + rel.ParentTable +
						" (" + rel.ParentColumns[0].ColumnName +
						")");

					// Execute against database
					Trace.WriteLine("CDatabase: Running SQL to create relation: " + sqlCreate.ToString());
					OleDbCommand SQL = new OleDbCommand(sqlCreate.ToString(), conn);

					SQL.ExecuteNonQuery();
					SQL.Dispose();
				}
			}
		}
		#endregion

	}
}
