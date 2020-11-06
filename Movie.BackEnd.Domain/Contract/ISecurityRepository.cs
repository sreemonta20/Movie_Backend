using Movie.BackEnd.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie.BackEnd.Domain.Contract
{
    public interface ISecurityRepository
    {
        Task<object> AuthenticateUser(dtoUserAuthReq oAuthReq);
    }
}
