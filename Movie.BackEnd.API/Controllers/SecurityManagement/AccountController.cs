using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Movie.BackEnd.Common.Utilities;
using Movie.BackEnd.Domain.Contract;
using Movie.BackEnd.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Movie.BackEnd.API.Controllers.SecurityManagement
{
    [Authorize]
    [Route("api/[controller]"), Produces("application/json"), EnableCors("AllowOrigin")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Variable Declatation & Initialization

        ISecurityRepository _securityService;

        #endregion


        #region Constructor Initialization

        public AccountController(ISecurityRepository securityService)
        {
            this._securityService = securityService;
        }

        #endregion


        #region Api Controller Methods or Verbs

        // POST: api/Account/Login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<object> Login([FromBody]dtoUserAuthReq model)
        {
            object result = null; object resData = null; string message = String.Empty;
            try
            {
                resData = await _securityService.AuthenticateUser(model);

            }
            catch (Exception Ex)
            {
                message = Ex.Message;
            }
            return result = new
            {
                resData,
                message
            };
        }


        #endregion

    }
}