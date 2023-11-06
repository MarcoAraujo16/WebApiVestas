using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiVestas.Models
{
	public class Shell
	{
		public int position { get; set; }
		public double height { get; set; }
		public double bottom_diam { get; set; }
		public double top_diam { get; set; }
		public double thickness { get; set; }
		public double steel_density { get; set; }
		public double mass { get; set; }
		public int ref_section { get; set; }
	}
}