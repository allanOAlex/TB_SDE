﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Shared.Dtos
{
    public record Dto
    {
        public int Id { get; set; }
        public bool Succesful { get; set; }
        public string? Message { get; set; }

    }
}
