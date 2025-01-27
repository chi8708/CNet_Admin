﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CNet.Web.Api.Controllers
{
    [Produces("application/json")]
    public class BaseController:Controller
    {
       public CNetUser ThisUser=> User.GetCNetUser();
    }
}