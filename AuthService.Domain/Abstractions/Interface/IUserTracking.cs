﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Abstractions.Interface
{
    public interface IUserTracking
    {
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
    }
}
