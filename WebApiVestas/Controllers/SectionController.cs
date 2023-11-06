using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiVestas.Models;

namespace WebApiVestas.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]

	public class SectionController:ApiController
	{
		private ISection repository;
		public SectionController() { }
		public SectionController(ISection repository) { 
			this.repository = repository;
		}

		[Route("api/section/getall")]
		[AcceptVerbs("GET", "POST")]

		public IEnumerable<Section> GetAll()
		{
			return repository.GetAll();
		}

		[Route("api/section/new")]
		[AcceptVerbs("GET", "POST")]

		public string NewSection(NewSection section)
		{
			return repository.NewSection(section);
		}

		[Route("api/shell/new")]
		[AcceptVerbs("GET", "POST")]

		public string NewShell(Shell shell)
		{
			return repository.NewShell(shell);
		}

		[Route("api/shell/delete/{sectionID}")]
		[AcceptVerbs("GET", "POST")]

		public string DeleteShell(int sectionID)
		{
			return repository.DeleteSection(sectionID);
		}

		[Route("api/section/GetSectionDetails/{sectionID}")]
		[AcceptVerbs("GET", "POST")]

		public SectionDetails GetSectionDetails(int sectionID)
		{
			return repository.GetSectionDetails(sectionID);
		}
	}
}