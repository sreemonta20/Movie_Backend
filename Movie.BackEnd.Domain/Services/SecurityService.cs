using Abp.Json;
using AutoMapper;
using Movie.BackEnd.Common.Utilities;
using Movie.BackEnd.Common.Utilities.Interfaces;
using Movie.BackEnd.Domain.Contract;
using Movie.BackEnd.Domain.DTO;
using Movie.BackEnd.Persistence.DBManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Movie.BackEnd.Persistence.DBContext;
using Movie.BackEnd.Persistence.DBModel;
using SecurityDBContext = Movie.BackEnd.Persistence.DBContext.MovieDBContext;

namespace Movie.BackEnd.Domain.Services
{
    public class SecurityService : ISecurityRepository, IMessageResposeState
    {
        #region Variable Declatation & Initialization
        private SecurityDBContext _secContext = null;
        private Hashtable oHashTable = null;
        private IDBManager<dtoLoginUser> _secSqlMgr = null;
        private IConfiguration _iconfig;
        private AppSettings appsettings = null;
        private readonly IMapper _mapper;
        // Implementation of .Net Core In Memory Cache -------------- start
        private readonly IMemoryCache _inMemoryCache;
        // Implementation of .Net Core In Memory Cache -------------- end
        #endregion

        #region Constructor Initialization
        public SecurityService(SecurityDBContext secContext, IDBManager<dtoLoginUser> secSqlMgr, IConfiguration iconfig,
        IMapper mapper, IMemoryCache inMemoryCache)
        {
            this._secContext = secContext;
            this._secSqlMgr = secSqlMgr;
            this._iconfig = iconfig;
            this.appsettings = new AppSettings(_iconfig);
            this._mapper = mapper;
            // Implementation of .Net Core In Memory Cache -------------- start
            this._inMemoryCache = inMemoryCache;
            // Implementation of .Net Core In Memory Cache -------------- end
        }


        #endregion

        #region All Service Methods

        #region User Operation
        public async Task<object> AuthenticateUser(dtoUserAuthReq oAuthReq)
        {
            object result = null; dtoLoginUser oDtoLoginUser = null; string message = string.Empty; bool state = false;
            try
            {

                using (_secContext = new SecurityDBContext())
                {

                    var authUser = await _secContext.LoginUser.SingleOrDefaultAsync(x => x.UserName == oAuthReq.UserName);
                    bool isValidPwd = BCrypt.Net.BCrypt.Verify(oAuthReq.Password, authUser.UserPassword);
                    if (isValidPwd)
                    {

                        oDtoLoginUser = new dtoLoginUser();
                        oDtoLoginUser.UserCode = authUser.UserCode;
                        oDtoLoginUser.UserId = authUser.UserId;
                        oDtoLoginUser.UserName = authUser.UserName;
                        oDtoLoginUser.Token = string.Empty;

                        AppUser.AppUserCode = oDtoLoginUser.UserCode;
                        AppUser.AppUserId = oDtoLoginUser.UserId;
                        AppUser.AppUserName = oDtoLoginUser.UserName;


                        TokenGenerator(oDtoLoginUser);

                        message = MessageConstants.SuccessfulLoggedIn;
                        state = MessageConstants.SuccessState;

                    }
                    else
                    {
                        message = MessageConstants.InvalidCredential;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }

            return result = new
            {
                oDtoLoginUser,
                message,
                state
            };

        }


        public async Task<object> GetSpecificUser(int userCode)
        {
            dtoLoginUser oDtoLoginUser = null; object result = null; string message = string.Empty; bool state = false;

            try
            {
                using (_secContext = new SecurityDBContext())
                {
                    LoginUser oLoginUser = await _secContext.LoginUser.Where(x => x.UserCode == userCode).FirstOrDefaultAsync();
                    if (oLoginUser.IsNotNull())
                    {

                        oDtoLoginUser = _mapper.Map<dtoLoginUser>(oLoginUser);
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.DataNotFound;
                        state = MessageConstants.SuccessState;
                    }
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }

            return result = new
            {
                oDtoLoginUser,
                message,
                state
            };
        }




        private void TokenGenerator(dtoLoginUser oDtoLoginUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.appsettings.SecretKey);
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = this.appsettings.Issuer,
                    Audience = this.appsettings.Audience,
                    Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, this.appsettings.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", oDtoLoginUser.UserCode.ToString()),
                    new Claim("UserName", oDtoLoginUser.UserName)
                }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                oDtoLoginUser.Token = tokenHandler.WriteToken(token);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }

        }



        public void SetMsgResState(out string message, out bool state, Enum eventoperation)
        {
            message = String.Empty; state = false;

            try
            {
                switch (eventoperation)
                {
                    case ExtensionMethods.EventOperation.New:
                        message = MessageConstants.Saved;
                        state = MessageConstants.SuccessState;
                        break;

                    case ExtensionMethods.EventOperation.Update:
                        message = MessageConstants.Updated;
                        state = MessageConstants.SuccessState;
                        break;
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }

        }

        #endregion

        #endregion
    }
}
