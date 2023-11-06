using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiVestas.Models;

namespace WebApiVestas.Controllers
{
	[EnableCors(origins: "http://localhost,http://locahost:8100,http://localhost:44350", headers: "*", methods: "*", SupportsCredentials = true)]

	public class SectionController:ApiController
	{
		private ISection repository;
		public SectionController() { }
		public SectionController(ISection repository) { 
			this.repository = repository;
		}
		[Route("api/section/getall")]
		public IEnumerable<Section> GetAll()
		{
			return repository.GetAll();
		}
	}
}