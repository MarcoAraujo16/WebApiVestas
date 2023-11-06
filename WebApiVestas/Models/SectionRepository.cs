using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using WebApiVestas.Models;

namespace WebApiVestas.BD_persistent
{
	
	public class SectionRepository:ISection
	{
		private DataLayer_Section dataLayer=null;
		public SectionRepository()
		{
			dataLayer = new DataLayer_Section(ConfigurationManager.ConnectionStrings["connBD"].ConnectionString);
		}
		public IEnumerable<Section> GetAll()
		{
			if (dataLayer != null)
			{
				string erro;
				List<Section> list = dataLayer.GetAll(out erro);
				if (erro == null && list != null && list.Count > 0)
					return list.ToArray<Section>();
				else
					return null;
			}
			else
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
		}
		public string NewSection(NewSection section)
		{
			if (dataLayer != null)
			{
				string erro;
				dataLayer.NewSection(section,out erro);
				if (erro == null)
					return "New section added successfully!";
				else
					return erro;
			}
			else
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
		}
		public string NewShell(Shell shell)
		{
			if (dataLayer != null)
			{
				string erro;
				dataLayer.NewShell(shell, out erro);
				if (erro == null)
					return "New shell added successfully!";
				else
					return erro;
			}
			else
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
		}
		public string DeleteSection(int sectionID)
		{
			if (dataLayer != null)
			{
				string erro;
				dataLayer.DeleteSection(sectionID, out erro);
				if (erro == null)
					return "Section removed successfully!";
				else
					return erro;
			}
			else
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
		}

		public SectionDetails GetSectionDetails(int sectionID)
		{
			if (dataLayer != null)
			{
				string erro;
				SectionDetails details=dataLayer.GetSectionDetails(sectionID,out erro);
				if (erro == null)
					return details;
				else
					return null;
			}
			else
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
		}

	}
}