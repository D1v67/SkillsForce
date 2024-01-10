using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.EmailSender;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.Interface;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Security.Cryptography;

namespace BusinessLayerNUnitTest
{
    [TestFixture]
    public class Tests
    {

        private Mock<IAccountDAL> _stubAccountDAL;
        private AccountService _accountService;

        private List<AccountModel> _accounts;

        [SetUp]
        public void Setup()
        {
            _accounts = new List<AccountModel>()
            {
                new AccountModel()
                {
                Email = "divesh@gmail.com",
                Password = "0xA394723EBCF1BE4F32E1A9BE7F396CB19C26924913EAB2FB47CD1219B1A59E5D",
                }
   
            };
            //SELECT a.HashedPassword, a.SaltValue FROM[User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email"

            _stubAccountDAL = new Mock<IAccountDAL>();
            
            _stubAccountDAL.Setup(accountDAL => accountDAL.IsUserAuthenticatedAsync(It.IsAny<AccountModel>())).
                ReturnsAsync((AccountModel account) => { return _accounts.Any(a => a.Email.Equals(account.Email)); }) ;



            _stubAccountDAL.Setup(accountDAL => accountDAL.IsUserAuthenticatedAsync(It.IsAny<AccountModel>())).
    ReturnsAsync((AccountModel account) => { return _accounts.Any(a => a.Email.Equals(account.Email) && a.Password.Equals(account.Password)); });


        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}