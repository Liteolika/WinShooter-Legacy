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
// $Id: CInternetExcelExport.cs 107 2009-02-01 06:25:33Z smuda $
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Allberg.Shooter.WinShooterServerRemoting;

namespace Allberg.Shooter.Common
{
	/// <summary>
	/// Summary description for InternetExport.
	/// </summary>
	public class CInternetExcelExport
	{
		internal CInternetExcelExport(Interface callerInterface)
		{
			myInterface = callerInterface;
			settings = CSettings.Instance;
		}
		Interface myInterface;
		CSettings settings;

		internal byte[] ExportResults(bool finalResults)
		{
			StringBuilder toReturn = new StringBuilder();

			Structs.ResultWeaponsClass[] wclasses = myInterface.ResultsGetWClasses();
			foreach (Structs.ResultWeaponsClass wclass in wclasses)
			{
				foreach (Structs.ShootersClass uclass in myInterface.ResultsGetUClasses(wclass))
				{
					ResultsReturn[] results =
							 myInterface.resultClass.GetResults(
							 wclass,
							 uclass,
							 myInterface.CompetitionCurrent);

					if (results.Length > 0)
					{
						toReturn.Append("\t" + wclass.ToString() + "\t" + uclass.ToString() + "\r\n");
						toReturn.Append(RenderResults(results));
						toReturn.Append("\r\n\r\n");
					}
				}
			}
			Encoding enc = ASCIIEncoding.GetEncoding(1252);
			return enc.GetBytes(toReturn.ToString());
		}

		internal byte[] ExportResults(Structs.ResultWeaponsClass wclass,
			Structs.ShootersClass uclass, bool finalResults)
		{
			ResultsReturn[] results =
				myInterface.resultClass.GetResults(
				wclass,
				uclass,
				myInterface.CompetitionCurrent);

			string resultString = RenderResults(results);

			Encoding enc = ASCIIEncoding.GetEncoding(1252);
			return enc.GetBytes(resultString);
		}

		private string RenderResults(ResultsReturn[] results)
		{
			try
			{
				Structs.CompetitionTypeEnum compType = myInterface.CompetitionCurrent.Type;

				StringBuilder toReturn = new StringBuilder();
				if (compType == Structs.CompetitionTypeEnum.Precision)
				{
					toReturn.Append("Plats\tNamn\tKlubb\tResultat\tTot\tStm\r\n");
				}
				else
				{
					toReturn.Append("Plats\tNamn\tKlubb\tResultat\tTot\tPo�ng\tStm\r\n");
				}

				bool norwegianCount = myInterface.CompetitionCurrent.NorwegianCount;

				Hashtable clubs = new Hashtable();

				int place = 0;
				foreach (ResultsReturn res in results)
				{
					place++;
					toReturn.Append(place.ToString() + "\t");
					toReturn.Append(res.ShooterName + "\t");
					if (clubs[res.ClubId] == null)
					{
						clubs[res.ClubId] = myInterface.GetClub(res.ClubId).Name;
					}
					toReturn.Append((string)clubs[res.ClubId] + "\t");

					switch (compType)
					{
						case Structs.CompetitionTypeEnum.Field:
							if (norwegianCount)
							{
								foreach (string stnRes in res.HitsPerStnString.Split(';'))
								{
									toReturn.Append(stnRes + " ");
								}

								toReturn.Append("\t" + (res.HitsTotal + res.FigureHitsTotal).ToString() + "\t");
							}
							else
							{
								toReturn.Append("\"" + res.HitsPerStnString.Replace(';', ' ') + "\"\t");
								toReturn.Append("\" " + res.HitsTotal.ToString() + "/" + res.FigureHitsTotal.ToString() + "\"\t");
							}
							break;
						case Structs.CompetitionTypeEnum.MagnumField:
							if (norwegianCount)
							{
								foreach (string stnRes in res.HitsPerStnString.Split(';'))
								{
									try
									{
										if (stnRes.Trim() != "")
										{
											int hits = int.Parse(stnRes.Split('/')[0]);
											int figures = int.Parse(stnRes.Split('/')[1]);
											toReturn.Append((hits + figures).ToString() + " ");
										}
									}
									catch (System.FormatException exc)
									{
										Trace.WriteLine(exc.ToString());
									}
								}

								toReturn.Append("\t" + (res.HitsTotal + res.FigureHitsTotal).ToString() + "\t");
							}
							else
							{
								toReturn.Append("\"" + res.HitsPerStnString.Replace(';', ' ') + "\"\t");
								toReturn.Append("\" " + res.HitsTotal.ToString() + "/" + res.FigureHitsTotal.ToString() + "\"\t");
							}
							break;
						case Structs.CompetitionTypeEnum.Precision:
							foreach (string stnRes in res.HitsPerStnString.Split(';'))
							{
								try
								{
									if (stnRes.Trim() != "")
									{
										toReturn.Append(stnRes.Trim() + " ");
									}
								}
								catch (System.FormatException exc)
								{
									Trace.WriteLine(exc.ToString());
								}
							}
							
							toReturn.Append("\t" + (res.HitsTotal + res.FigureHitsTotal).ToString() + "\t");
							break;
						default:
							throw new NotImplementedException();
					}
					if (compType != Structs.CompetitionTypeEnum.Precision)
					{
						toReturn.Append(res.PointsTotal.ToString() + "\t");
					}
					Structs.Medal medal = (Structs.Medal)res.Medal;
					switch (medal)
					{
						case Structs.Medal.StandardSilver:
							toReturn.Append("S\t");
							break;
						case Structs.Medal.StardardBrons:
							toReturn.Append("B\t");
							break;
						default:
							toReturn.Append("\t");
							break;
					}

					toReturn.Append("\r\n");
				}

				return toReturn.ToString();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc.ToString());
				throw;
			}
		}

		/*internal byte[] ExportResults(bool finalResults)
		{
			DSExportExcel results = exportResultsToDataSet();
			return renderResults(results);
		}

		internal byte[] ExportResults(Structs.ResultWeaponsClass wclass, 
			Structs.ShootersClass uclass, bool finalResults)
		{
			DSExportExcel results = exportResultsToDataSet();
			return renderResults(results);
		}

		private DSExportExcel exportResultsToDataSet()
		{
			DSExportExcel ds = new DSExportExcel();
			DSExportExcel.ResultsRow row = ds.Results.NewResultsRow();
			row.ShooterName = "Allberg, John";
			row.Place = "1";
			ds.Results.AddResultsRow(row);

			return ds;
		}

		private byte[] renderResults(DSExportExcel results)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter txtWriter = new StreamWriter(stream);
			HtmlTextWriter writer = new HtmlTextWriter(txtWriter);
			DataGrid grid = new DataGrid();
			grid.DataSource = results.Results;
			grid.AutoGenerateColumns = true;
			grid.DataBind();
			grid.RenderControl(writer);

			return stream.ToArray();
		}*/
	}
}
