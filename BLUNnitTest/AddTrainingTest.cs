//using BusinessLayer.SkillsForce.Services;
//using Common.SkillsForce.Entity;
//using DataAccessLayer.SkillsForce.Interface;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BusinessLayerNUnitTest
//{
//    public class AddTrainingTest
//    {
//        private Mock<ITrainingDAL> _stubTrainingDAL;
//        private TrainingService _trainingService;

//        private List<AccountModel> _accounts;



//        [SetUp]

//        public void Setup()
//        {
//            //_accounts = new List<AccountModel>()
//            //{
//            //    new AccountModel()
//            //    {
//            //        Email = "divesh@gmail.com",
//            //        Password = "admin",
//            //        HashedPassword = HexStringToByteArray("A394723EBCF1BE4F32E1A9BE7F396CB19C26924913EAB2FB47CD1219B1A59E5D"),
//            //        SaltValue = HexStringToByteArray("645C8A074930444D00A159A7022A70A2")
//            //    }
//            //};


//            _stubTrainingDAL = new Mock<ITrainingDAL>();


//            //_stubAccountDAL.Setup(accountDAL => accountDAL.GetUserCredentialsAsync(It.IsAny<string>()))
//            //    .ReturnsAsync((string email) => _accounts.FirstOrDefault((a) => a.Email == email));

//            //_trainingService = new TrainingService(_stubTrainingDAL.Object, null);
//        }


//        //[Test]
//        //public async Task TestUserLogin_ValidCredentials_ResultIsLoginSuccessful()
//        //{
//        //    //Arrange
//        //    AccountModel account = new AccountModel()
//        //    {
//        //        Email = "divesh@gmail.com",
//        //        Password = "admin"
//        //    };

//        //    //Act
//        //    bool isExist = await _accountService.IsUserAuthenticatedAsync(account);
//        //    //Assert
//        //    Assert.IsTrue(isExist);
//        //}

//        //[Test]
//        //public async Task TestUserLogin_InvalidPassword_ResultIsLoginUnsuccessful()
//        //{
//        //    // Arrange
//        //    AccountModel account = new AccountModel()
//        //    {
//        //        Email = "divesh@gmail.com",
//        //        Password = "invalid_password"
//        //    };

//        //    // Act
//        //    bool isExist = await _accountService.IsUserAuthenticatedAsync(account);

//        //    // Assert
//        //    Assert.IsFalse(isExist);
//        //}

//        //[Test]
//        //public async Task TestUserLogin_InvalidEmail_ResultIsLoginUnsuccessful()
//        //{
//        //    // Arrange
//        //    AccountModel account = new AccountModel()
//        //    {
//        //        Email = "diveshhdhdhd@gmail.com",
//        //        Password = "admin"
//        //    };

//        //    // Act
//        //    bool isExist = await _accountService.IsUserAuthenticatedAsync(account);

//        //    // Assert
//        //    Assert.IsFalse(isExist);
//        //}

//        //[Test]
//        //public async Task TestUserLogin_NullCredentials_ResultIsLoginUnsuccessful()
//        //{
//        //    // Arrange
//        //    AccountModel account = new AccountModel()
//        //    {
//        //        Email = null,
//        //        Password = null
//        //    };

//        //    // Act
//        //    bool isExist = await _accountService.IsUserAuthenticatedAsync(account);

//        //    // Assert
//        //    Assert.IsFalse(isExist);
//        //}

//        //private static byte[] HexStringToByteArray(string hex)
//        //{
//        //    int length = hex.Length / 2;
//        //    byte[] result = new byte[length];

//        //    for (int i = 0; i < length; i++)
//        //    {
//        //        result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
//        //    }

//        //    return result;
//        //}
//    }
//}
