using BusinessLayer.SkillsForce.Helpers;
using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using DataAccessLayer.SkillsForce.DAL;
using DataAccessLayer.SkillsForce.Interface;
using Moq;

namespace BLUNnitTest
{
    [TestFixture]
    public class Tests
    {

        private Mock<IAccountDAL> _stubAccountDAL;
        private  AccountService _accountService;

        private List<AccountModel> _accounts;

        //private PasswordHasher hasher;

        [SetUp]


        public void Setup()
        {
            _accounts = new List<AccountModel>()
            {
                new AccountModel()
                {
                    Email = "divesh@gmail.com",
                    Password = "admin",
                    //HashedPassword = "0xA394723EBCF1BE4F32E1A9BE7F396CB19C26924913EAB2FB47CD1219B1A59E5D",
                    //SaltValue = "0x645C8A074930444D00A159A7022A70A2"

                    HashedPassword = HexStringToByteArray("A394723EBCF1BE4F32E1A9BE7F396CB19C26924913EAB2FB47CD1219B1A59E5D"),
                    SaltValue = HexStringToByteArray("645C8A074930444D00A159A7022A70A2")
                }
            };
            //SELECT a.HashedPassword, a.SaltValue FROM[User] u INNER JOIN Account a ON u.UserID = a.UserID WHERE u.Email = @Email"

            _stubAccountDAL = new Mock<IAccountDAL>();

           // hasher = new PasswordHasher();

            _stubAccountDAL.Setup(accountDAL => accountDAL.IsUserAuthenticatedAsync(It.IsAny<AccountModel>())).
            ReturnsAsync((AccountModel account) => {

                //byte[] hashedPassword = (PasswordHasher.HashPassword(account.Password)).Item1;
                //byte[] salt = (PasswordHasher.HashPassword(account.Password)).Item2;

                bool exist = PasswordHasher.VerifyPassword(account.Password, account.HashedPassword, account.SaltValue);
               // string pass = hashedPassword.ToString();
                return _accounts.Any(a => a.Email.Equals(account.Email) && exist);
            });

            _stubAccountDAL.Setup(accountDAL => accountDAL.GetUserCredentialsAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => _accounts.FirstOrDefault((a) => a.Email == email));

            _accountService = new AccountService(_stubAccountDAL.Object,null);
        }

        [Test]
        public async Task Test1()
        {

            //Arrange
            AccountModel account = new AccountModel(){
                Email = "divesh@gmail.com",
                Password = "admin"
            };

            //Act
            bool isExist  = await _accountService.IsUserAuthenticatedAsync(account);   
            //Assert
            Assert.IsTrue(isExist);
        }

                    // Add a helper method to convert hex strings to byte arrays
            private byte[] ConvertHexStringToByteArray(string hexString)
            {
                hexString = hexString.Substring(2); // Remove "0x" prefix
                int length = hexString.Length / 2;
                byte[] byteArray = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    byteArray[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
                return byteArray;
            }

            private static byte[] HexStringToByteArray(string hex)
            {
                int length = hex.Length / 2;
                byte[] result = new byte[length];

                for (int i = 0; i < length; i++)
                {
                    result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }

                return result;
            }
    }
}