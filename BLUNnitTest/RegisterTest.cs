//using BusinessLayer.SkillsForce.Services;
//using Common.SkillsForce.Entity;
//using Common.SkillsForce.ViewModel;
//using DataAccessLayer.SkillsForce.Interface;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BusinessLayerNUnitTest
//{
//    [TestFixture]
//    public class RegisterTest
//    {
//        private Mock<IAccountDAL> _stubAccountDAL;
//        private Mock<IUserDAL> _stubUserDAL;

//        private AccountService _accountService;
//        private UserService _userService;

//        private List<AccountModel> _accounts;
//        private List<UserModel> _users;

//        [SetUp]
//        public void Setup()
//        {
//            _users = new List<UserModel>()
//            {
//                new UserModel()
//                {
//                    UserID = 1,
//                    FirstName = "User",
//                    LastName = "Test",
//                    NIC = "U0930029930492",
//                    MobileNumber = "57939928",
//                    Email = "user@ceridian.com",
//                    ManagerID = 1,
//                    DepartmentID = 1,
        
//                }
//            };

//            _stubUserDAL = new Mock<IUserDAL>();
//            _stubAccountDAL = new Mock<IAccountDAL>();


//            _stubAccountDAL.Setup(accountDAL => accountDAL.GetUserCredentialsAsync(It.IsAny<string>()))
//                .ReturnsAsync((string email) => _accounts.FirstOrDefault((a) => a.Email == email));

//            RegisterViewModel capturedRegisterViewModel = null;

//            _stubAccountDAL.Setup(a => a.RegisterAsync(It.IsAny<RegisterViewModel>()))
//                           .Callback<RegisterViewModel>(model => capturedRegisterViewModel = model);



//            // Set up the GetUserByEmailAsync method to return a user if it exists in the list
//            _stubUserDAL.Setup(userDAL => userDAL.IsEmailAlreadyExistsAsync(It.IsAny<string>()))
//                        .ReturnsAsync((string email) => _users.Any(u => u.Email == email));

//            // Set up the GetUserByNICAsync method to return a user if it exists in the list
//            _stubUserDAL.Setup(userDAL => userDAL.IsNICExistsAsync(It.IsAny<string>()))
//                        .ReturnsAsync((string nic) => _users.Any(u => u.NIC == nic));

//            // Set up the GetUserByMobileNumberAsync method to return a user if it exists in the list
//            _stubUserDAL.Setup(userDAL => userDAL.IsMobileNumberExistsAsync(It.IsAny<string>()))
//                        .ReturnsAsync((string mobileNumber) => _users.Any(u => u.MobileNumber == mobileNumber));








//            _accountService = new AccountService(_stubAccountDAL.Object, null);
//        }

//        [Test]
//        public async Task TestUserRegistration_ValidInput_ResultIsRegistrationSuccessful()
//        {
//            //Arrange
//            var user = new RegisterViewModel()
//            {
               
//                FirstName = "User",
//                LastName = "Test",
//                NIC = "U0930029930492",
//                MobileNumber = "57939928",
//                Email = "user@ceridian.com",
//                ManagerID = 1,
//                DepartmentID = 1,

//            };

//            //Act
//           // bool isExist = await _accountService.RegisterUserAsync(user);
//            //Assert
//            //Assert.IsTrue(isExist);
//        }

 


//    }
//}
