using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace DemoPL.Models
{
	public class DepartmentViewModel
	{
		public int Id { get; set; } //PK

		[Required(ErrorMessage = "Name Is Required ")]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required(ErrorMessage = "Code Is Required ")]

		public string Code { get; set; }

		public DateTime DateofCreation { get; set; }

		[InverseProperty(nameof(Employee.Department))]
		public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

	}
}
