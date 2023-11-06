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
							cmd.CommandText = "Select PartNumber from Section";
							cmd.CommandType = CommandType.Text;
							SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.Default);
							while (dataReader.Read())
							{
								list.Add(new Section
								{
									partNumber = dataReader.GetString(0)
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

	}
}