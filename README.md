Instructions to run WebApi

The web api, as well as the database, is hosted online, so it does not need to be run on your machine and can be accessed through the following endpoints:

<h1>GetAllSection</h1> (return all the sections): http://marcoaraujo16-001-site1.ftempurl.com/api/section/getall

<h1>GetSectionDetails</h1> (return section details and all of his shells by I): http://marcoaraujo16-001-site1.ftempurl.com/api/section/GetSectionDetails/{id}

<h1>NewSection</h1> (create a new section with a shell): http://marcoaraujo16-001-site1.ftempurl.com/api/section/new

JSON sample to use on Insomnia (for example):
<em>
{
  "partNumber": "796544",
  "shell": {
		"position": null,
		"height": 10.0,
    "bottom_diam": 5.0,
    "top_Diam": 7.0,
    "thickness": 2.0,
    "steel_density": 7.8,
    "mass": null,
		"ref_section":null
  }
}
</em>

<h1>New Shell</h1> (create a new shell {associated to a section by it's ID} ): http://marcoaraujo16-001-site1.ftempurl.com/api/shell/new
JSON sample to use on Insomnia (for example):
<em>
  {
		"position": null,
		"height": 10.0,
    "bottom_diam": null,
    "top_Diam": 10,
    "thickness": 2.0,
    "steel_density": 7.8,
    "mass": null,
		"ref_section":1
}
</em>

<h1>Delete Section</h1> (delete section and all of its shells by ID) : http://marcoaraujo16-001-site1.ftempurl.com/api/shell/delete/{id}
