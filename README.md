<h1>Instructions to run WebApi</h1>

The web api, as well as the database, is hosted online, so it does not need to be run on your machine and can be accessed through the following endpoints:

<h2>GetAllSection</h2> (return all the sections): http://marcoaraujo16-001-site1.ftempurl.com/api/section/getall

<h2>GetSectionDetails</h2> (return section details and all of his shells by I): http://marcoaraujo16-001-site1.ftempurl.com/api/section/GetSectionDetails/{id}

<h2>NewSection</h2> (create a new section with a shell): http://marcoaraujo16-001-site1.ftempurl.com/api/section/new

JSON sample to use on Insomnia (for example):<br>
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

<h2>New Shell</h2> (create a new shell {associated to a section by it's ID} ): http://marcoaraujo16-001-site1.ftempurl.com/api/shell/new
JSON sample to use on Insomnia (for example):<br>
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

<h2>Delete Section</h2> (delete section and all of its shells by ID) : http://marcoaraujo16-001-site1.ftempurl.com/api/shell/delete/{id}


<h1>DATABASE</h1>
<p>The database used is SQL Server 16 and can be accessed using the following credentials:</p><br>
<p>Data Source=SQL8006.site4now.net<br>Initial Catalog=db_aa0fd5_vestasex2<br>User Id=db_aa0fd5_vestasex2_admin<br>Password='bdVestas2023'</p>
