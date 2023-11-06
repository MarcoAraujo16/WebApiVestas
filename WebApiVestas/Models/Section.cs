using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiVestas.Models
{
	public class Section
	{
		public string partNumber { get; set; }
		public double bottomDiam {  get; set; }
		public double topDiam { get; set;}

		public double height { get; set; }

		public double mass { get; set; }
	}
}