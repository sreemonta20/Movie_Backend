using Movie.BackEnd.API.Controllers.SecurityManagement;
using Movie.BackEnd.Domain.Contract;
using Movie.BackEnd.Domain.DTO;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Movie.BackEnd.Test
{

    public class SecurityTest
    {
        
        private  ISecurityRepository _securityService;

        public SecurityTest(ISecurityRepository securityService)
        {
            this._securityService = securityService;
        }

        [Fact]
        public void TestBegin()
        {
            var model = new dtoUserAuthReq
            {
                UserName = "samirul",
                Password = "123456"
            };



            Assert.NotEqual("sreemonta", model.UserName);
        }
        

        [Fact]
        public async Task ShouldHaveValidateUserAsync()
        {
            var model = new dtoUserAuthReq
            {
                UserName = "samirul",
                Password = "123456"
            };


            object data = await _securityService.AuthenticateUser(model);
            Assert.NotNull(data);
        }


    }
}
