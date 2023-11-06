using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiVestas.Models
{
	public interface ISection
	{
		IEnumerable<Section> GetAll();
	}
}