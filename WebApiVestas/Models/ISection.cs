using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiVestas.Models
{
	public interface ISection
	{
		IEnumerable<Section> GetAll();

		string NewSection(NewSection newSection); 

		string NewShell(Shell shell);

		string DeleteSection(int sectionID);

		SectionDetails GetSectionDetails(int sectionID);
	}
}