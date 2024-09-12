﻿using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace EF_DapperPractice.Models {
    [Table("Companies")]
    public class Company {
        public Company()
        {
            Employees = new List<Employee>();
        }
        [Dapper.Contrib.Extensions.Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        [Write(false)]
        public List<Employee> Employees { get; set; }
    }
}
