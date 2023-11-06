using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApiVestas.Models;

namespace WebApiVestas.BD_persistent
{
	public class DataLayer_Section
	{
		private SqlConnection conn=null;
		public DataLayer_Section(string connectionString)
		{
			conn=new SqlConnection(connectionString);
		}
		public List<Section> GetAll(out string erro)
		{
			erro = null;
			List<Section> list = new List<Section>();
			if (conn != null)
			{
				try
				{
					conn.Open();
					if (conn.State == ConnectionState.Open)
					{
						using(SqlCommand cmd = new SqlCommand(null,conn))
						{
							cmd.CommandText = "WITH ShellData AS " +
							"(SELECT sh.ref_section AS SectionID,sh.Position,sh.BottomDiam,sh.TopDiam,sh.Height,sh.Mass FROM Shells sh)" +
							" ,ShellMaxPosition AS (SELECT SectionID,MAX(Position) AS MaxPosition FROM ShellData GROUP BY SectionID)" +
							" SELECT s.PartNumber," +
							" MIN(CASE WHEN sd.Position = 1 THEN sd.BottomDiam END) AS BottomDiam," +
							" MAX(CASE WHEN sd.Position = sp.MaxPosition THEN sd.TopDiam END) AS TopDiam," +
							" SUM(sd.Height) AS TotalHeight," +
							" SUM(sd.Mass) AS TotalMass" +
							" FROM Section s" +
							" LEFT JOIN ShellData sd ON s.ID = sd.SectionID" +
							" LEFT JOIN ShellMaxPosition sp ON s.ID = sp.SectionID" +
							" GROUP BY s.PartNumber, sp.MaxPosition;";
							cmd.CommandType = CommandType.Text;
							SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.Default);
							while (dataReader.Read())
							{
								list.Add(new Section
								{
									partNumber = dataReader.GetString(0),
									bottomDiam=Convert.ToDouble(dataReader.GetDecimal(1)),
									topDiam=Convert.ToDouble(dataReader.GetDecimal(2)),
									height= Convert.ToDouble(dataReader.GetDecimal(3)),
									mass=Convert.ToDouble(dataReader.GetDecimal(4))
								});
							}
						}
					}
					else
					{
						erro = "Operation failed";
					}
				}
				catch(Exception ex) 
				{
					erro=ex.Message + "["+ex.StackTrace + "]";
				}
				finally
				{
					conn.Close();
				}
			}
			return list;
		}

		public string NewSection(NewSection section,out string erro)
		{
			erro = null;
			
			if (conn != null)
			{
				try
				{
					conn.Open();
					if (conn.State == ConnectionState.Open)
					{
						

							using (SqlTransaction transaction = conn.BeginTransaction())
							{
								try
								{
								double mass;
								mass=(Math.PI/2)*section.shell.steel_density*section.shell.thickness*section.shell.height*(section.shell.bottom_diam+section.shell.top_diam);
								section.shell.mass = Math.Round(mass,2);
								int sectionID = 0;
								// Primeiro, insere a Section
									using (SqlCommand cmdSection = new SqlCommand(null, conn, transaction))
									{
										cmdSection.CommandText = "INSERT INTO Section (PartNumber) VALUES (@PartNumber);SELECT SCOPE_IDENTITY();";
										cmdSection.Parameters.AddWithValue("@PartNumber", section.partNumber);
										sectionID = Convert.ToInt32(cmdSection.ExecuteScalar());
								}

								section.shell.ref_section = sectionID;
								// Em seguida, insere a Shell associada à Section recém-inserida
								using (SqlCommand cmdShell = new SqlCommand(null, conn, transaction))
									{
										cmdShell.CommandText = "INSERT INTO Shells (Position, Height, BottomDiam, TopDiam, Thickness, SteelDensity, Mass,ref_section) " +
										"VALUES (1,@height,@bottomdiam,@topdiam,@thick,@steel,@mass,@ref_section)";

										cmdShell.Parameters.AddWithValue("@height",section.shell.height);
										cmdShell.Parameters.AddWithValue("@bottomdiam", section.shell.bottom_diam);
										cmdShell.Parameters.AddWithValue("@topdiam", section.shell.top_diam);
										cmdShell.Parameters.AddWithValue("@thick", section.shell.thickness);
										cmdShell.Parameters.AddWithValue("@steel", section.shell.steel_density);
										cmdShell.Parameters.AddWithValue("@mass", section.shell.mass);
										cmdShell.Parameters.AddWithValue("@ref_section", section.shell.ref_section);

										cmdShell.ExecuteNonQuery();
									}

									// Commit da transação se ambas as inserções forem bem-sucedidas
									transaction.Commit();
								}
								catch (SqlException e)
								{
									// Em caso de erro, desfazer a transação
									transaction.Rollback();
									erro = e.Message;
								}
							}

					}
					else
					{
						erro = "Operation failed";
					}
				}
				catch (Exception ex)
				{
					erro = ex.Message + "[" + ex.StackTrace + "]";
				}
				finally
				{
					conn.Close();
				}
			}
			return erro;

		}

		public string NewShell(Shell shell,out string erro)
		{
			erro = null;

			if (conn != null)
			{
				try
				{
					conn.Open();
					if (conn.State == ConnectionState.Open)
					{

						using (SqlTransaction transaction = conn.BeginTransaction())
						{

							try
							{
								// Consulte a base de dados para obter a última Position da Shell relacionada à Section
								using (SqlCommand cmdGetLastPosition = new SqlCommand(null, conn, transaction))
								{
									cmdGetLastPosition.CommandText = "SELECT TOP 1 Position,TopDiam FROM Shells WHERE ref_section = @SectionID ORDER BY Position DESC";
									cmdGetLastPosition.Parameters.AddWithValue("@SectionID", shell.ref_section);

									// Execute a consulta para obter a última Position
									using (SqlDataReader reader = cmdGetLastPosition.ExecuteReader())
									{
										if (reader.Read())
										{
											shell.position = reader.GetInt32(0) + 1; // Incrementa a última Position
											shell.bottom_diam = Convert.ToDouble(reader.GetDecimal(1));
										}
									}
								}
								double mass = 0.00;
								mass = (Math.PI / 2) * shell.steel_density * shell.thickness * shell.height * (shell.bottom_diam + shell.top_diam);
								shell.mass = Math.Round(mass, 2);

								using (SqlCommand cmdShell = new SqlCommand(null, conn, transaction))
								{
									cmdShell.CommandText = "INSERT INTO Shells (Position, Height, BottomDiam, TopDiam, Thickness, SteelDensity, Mass,ref_section) " +
									"VALUES (@position,@height,@bottomdiam,@topdiam,@thick,@steel,@mass,@ref_section)";

									cmdShell.Parameters.AddWithValue("@position", shell.position);
									cmdShell.Parameters.AddWithValue("@height", shell.height);
									cmdShell.Parameters.AddWithValue("@bottomdiam", shell.bottom_diam);
									cmdShell.Parameters.AddWithValue("@topdiam", shell.top_diam);
									cmdShell.Parameters.AddWithValue("@thick", shell.thickness);
									cmdShell.Parameters.AddWithValue("@steel", shell.steel_density);
									cmdShell.Parameters.AddWithValue("@mass", shell.mass);
									cmdShell.Parameters.AddWithValue("@ref_section", shell.ref_section);

									cmdShell.ExecuteNonQuery();
								}

								// Commit da transação se ambas as inserções forem bem-sucedidas
								transaction.Commit();
							}
							catch (SqlException e)
							{
								// Em caso de erro, desfazer a transação
								transaction.Rollback();
								erro = e.Message;
							}


						}
					}
					else
					{
						erro = "Operation failed";
					}
				}
				catch (Exception ex)
				{
					erro = ex.Message + "[" + ex.StackTrace + "]";
				}
				finally
				{
					conn.Close();
				}
			}
			return erro;
		}

		public string DeleteSection(int sectionID, out string erro)
		{
			erro = null;
			if (conn != null)
			{
				try
				{
					conn.Open();

					if (conn.State == ConnectionState.Open)
					{
						using (SqlTransaction transaction = conn.BeginTransaction())
						{
							try
							{
								// Exclua todas as Shells associadas à Section
								using (SqlCommand cmdDeleteShells = new SqlCommand(null, conn, transaction))
								{
									cmdDeleteShells.CommandText = "DELETE FROM Shells WHERE ref_section = @SectionID";
									cmdDeleteShells.Parameters.AddWithValue("@SectionID", sectionID);
									cmdDeleteShells.ExecuteNonQuery();
								}

								// Em seguida, exclua a Section
								using (SqlCommand cmdDeleteSection = new SqlCommand(null, conn, transaction))
								{
									cmdDeleteSection.CommandText = "DELETE FROM Section WHERE ID = @SectionID";
									cmdDeleteSection.Parameters.AddWithValue("@SectionID", sectionID);
									cmdDeleteSection.ExecuteNonQuery();
								}

								// Commit da transação se as exclusões forem bem-sucedidas
								transaction.Commit();
							}
							catch (SqlException e)
							{
								// Em caso de erro, desfazer a transação
								transaction.Rollback();
								erro=e.Message;
							}
						}
					}
					else
					{
						erro = "Operation failed";

					}
				}
				catch (Exception ex)
				{
					erro = ex.Message + "[" + ex.StackTrace + "]";
				}
				finally
				{
					conn.Close();
				}
			}
			return erro;

		}

		public SectionDetails GetSectionDetails(int sectionID,out string erro)
		{
			erro = null;
			SectionDetails res=new SectionDetails();
			Section section = new Section();
			List<Shell> shellList = new List<Shell>();
			if (conn != null)
			{
				try
				{
					conn.Open();
					if (conn.State == ConnectionState.Open)
					{
						using (SqlCommand cmd = new SqlCommand(null, conn))
						{
							cmd.CommandText = "WITH ShellData AS " +
							"(SELECT sh.ref_section AS SectionID,sh.Position,sh.BottomDiam,sh.TopDiam,sh.Height,sh.Mass FROM Shells sh)" +
							" ,ShellMaxPosition AS (SELECT SectionID,MAX(Position) AS MaxPosition FROM ShellData GROUP BY SectionID)" +
							" SELECT s.PartNumber," +
							" MIN(CASE WHEN sd.Position = 1 THEN sd.BottomDiam END) AS BottomDiam," +
							" MAX(CASE WHEN sd.Position = sp.MaxPosition THEN sd.TopDiam END) AS TopDiam," +
							" SUM(sd.Height) AS TotalHeight," +
							" SUM(sd.Mass) AS TotalMass" +
							" FROM Section s" +
							" LEFT JOIN ShellData sd ON s.ID = sd.SectionID" +
							" LEFT JOIN ShellMaxPosition sp ON s.ID = sp.SectionID" +
							" WHERE sd.SectionID=@SectionID" +
							" GROUP BY s.PartNumber, sp.MaxPosition;";
							cmd.CommandType = CommandType.Text;
							cmd.Parameters.AddWithValue("@SectionID", sectionID);

							SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.Default);
							while (dataReader.Read())
							{

								section.partNumber = dataReader.GetString(0);
								section.bottomDiam = Convert.ToDouble(dataReader.GetDecimal(1));
								section.topDiam = Convert.ToDouble(dataReader.GetDecimal(2));
								section.height = Convert.ToDouble(dataReader.GetDecimal(3));
								section.mass = Convert.ToDouble(dataReader.GetDecimal(4));
							}
							dataReader.Close();
						}

						using (SqlCommand cmd = new SqlCommand(null, conn))
						{
							cmd.CommandText = "SELECT Position,Height,BottomDiam,TopDiam,Thickness,SteelDensity,Mass,ref_section FROM Shells" +
								" WHERE ref_section=@SectionID";
							cmd.Parameters.AddWithValue("@SectionID", sectionID);
							cmd.CommandType = CommandType.Text;
							SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.Default);
							while (dataReader.Read())
							{
								shellList.Add(new Shell
								{
									position=dataReader.GetInt32(0),
									height=Convert.ToDouble(dataReader.GetDecimal(1)),
									bottom_diam=Convert.ToDouble(dataReader.GetDecimal(2)),
									top_diam= Convert.ToDouble(dataReader.GetDecimal(3)),
									thickness=Convert.ToDouble(dataReader.GetDecimal(4)),
									steel_density=Convert.ToDouble(dataReader.GetDecimal(5)),
									mass=Convert.ToDouble(dataReader.GetDecimal(6)),
									ref_section=dataReader.GetInt32(7),
								});
							}
						}
						res.section = section;
						res.shells = shellList;


					}
					else
					{
						erro = "Operation failed";
					}
				}
				catch (Exception ex)
				{
					erro = ex.Message + "[" + ex.StackTrace + "]";
				}
				finally
				{
					conn.Close();
				}
			}

			return res;
		}

		//public List<Section> GetSectionByDiam(SearchByDiam searchByDiam, out string erro)
		//{
		//	erro = null;
		//	List<Section> list = new List<Section>();
		//	if (conn != null)
		//	{
		//		try
		//		{
		//			conn.Open();
		//			if (conn.State == ConnectionState.Open)
		//			{
		//				using (SqlCommand cmd = new SqlCommand(null, conn))
		//				{
		//					cmd.CommandText = "WITH ShellData AS " +
		//					"(SELECT sh.ref_section AS SectionID,sh.Position,sh.BottomDiam,sh.TopDiam,sh.Height,sh.Mass FROM Shells sh)" +
		//					" ,ShellMaxPosition AS (SELECT SectionID,MAX(Position) AS MaxPosition FROM ShellData GROUP BY SectionID)" +
		//					" SELECT s.PartNumber," +
		//					" MIN(CASE WHEN sd.Position = 1 THEN sd.BottomDiam END) AS BottomDiam," +
		//					" MAX(CASE WHEN sd.Position = sp.MaxPosition THEN sd.TopDiam END) AS TopDiam," +
		//					" SUM(sd.Height) AS TotalHeight," +
		//					" SUM(sd.Mass) AS TotalMass" +
		//					" FROM Section s" +
		//					" LEFT JOIN ShellData sd ON s.ID = sd.SectionID" +
		//					" LEFT JOIN ShellMaxPosition sp ON s.ID = sp.SectionID" +
		//					" GROUP BY s.PartNumber, sp.MaxPosition;";
		//					cmd.CommandType = CommandType.Text;
		//					SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.Default);
		//					while (dataReader.Read())
		//					{
		//						list.Add(new Section
		//						{
		//							partNumber = dataReader.GetString(0),
		//							bottomDiam = dataReader.GetDouble(1),
		//							topDiam = dataReader.GetDouble(2),
		//							height = dataReader.GetDouble(3),
		//							mass = dataReader.GetDouble(4)
		//						});
		//					}
		//				}
		//			}
		//			else
		//			{
		//				erro = "Operation failed";
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			erro = ex.Message + "[" + ex.StackTrace + "]";
		//		}
		//		finally
		//		{
		//			conn.Close();
		//		}
		//	}
		//	return list;
		//}

	}
}