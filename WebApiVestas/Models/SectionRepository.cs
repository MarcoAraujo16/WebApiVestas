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
	}
}