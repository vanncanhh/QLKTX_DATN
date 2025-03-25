﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class UserModelView
    {
        public int id { get; set; }
        public string? full_name { get; set; }
        public string? phone_number { get; set; }
        public string? email { get; set; }
        public string? avatar { get; set; }
        public string? address { get; set; }
        public string? password { get; set; }
        public int? role { get; set; }  
        public int? status { get; set; }
        public DateTime? register_date { get; set; }
        public string? code { get; set; }
    }
    public class UserMapModelView
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        //public string? avatar { get; set; }
        //public string? address { get; set; }
        //public string? password { get; set; }
        public int? RoleUser { get; set; }
        //public int? status { get; set; }
        //public DateTime? register_date { get; set; }
        //public string? code { get; set; }
    }
}
