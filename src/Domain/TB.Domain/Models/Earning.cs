﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Domain.Models
{
    public class Earning
    {
        public Earning()
        {
                
        }

        [Key]
        public int Id { get; set; }
        public string? Symbol { get; set; }
        public DateTime Date { get; set; }
        public string? Quater { get; set; }
        public decimal EpsEst { get; set; }
        public decimal Eps { get; set; }
        public string? ReleaseTime { get; set; }



    }
}
