using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiVestas.Models
{
	public class SectionDetails
	{
		public Section section { get; set; }
		public List<Shell> shells { get; set; }
	}
}