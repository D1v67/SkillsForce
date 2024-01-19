using BusinessLayer.SkillsForce.Services;
using Common.SkillsForce.Entity;
using Common.SkillsForce.ViewModel;
using DataAccessLayer.SkillsForce.Interface;
using Moq;

namespace BusinessLayerNUnitTest
{
    [TestFixture]
    public class RegisterTest
    {
        private Mock<IAccountDAL> _stubAccountDAL;
        private Mock<IUserDAL> _stubUserDAL;

        private AccountService _accountService;
        private UserService _userService;

        private List<AccountModel> _accountsRepository;
        private List<UserModel> _usersRepository;

        [SetUp]
        public void Setup()
        {
            _usersRepository = new List<UserModel>
            {
                new UserModel
                {
                    UserID = 1,
                    FirstName = "Divesh",
                    LastName = "Nugessur",
                    NIC = "N051100150456A",
                    MobileNumber = "58386112",
                    Email = "divesh@gmail.com",
                    ManagerID = 1,
                    DepartmentID = 1,
                },
                new UserModel
                {
                    UserID = 2,
                    FirstName = "John",
                    LastName = "Doe",
                    NIC = "N051100150457A",
                    MobileNumber = "12345678",
                    Email = "john.doe@example.com",
                    ManagerID = 1,
                    DepartmentID = 2,
                }
            };

            _accountsRepository = new List<AccountModel>
            {
                new AccountModel
                {
                    UserID = 1,
                    HashedPassword = HexStringToByteArray("A394723EBCF1BE4F32E1A9BE7F396CB19C26924913EAB2FB47CD1219B1A59E5D"),
                    SaltValue = HexStringToByteArray("645C8A074930444D00A159A7022A70A2")
                },
                new AccountModel
                {
                    UserID = 2,
                    HashedPassword = HexStringToByteArray("B394723EBCF1BE4F32E1A9BE7F396CB19C26924913EAB2FB47CD1219B1A59E5D"),
                    SaltValue = HexStringToByteArray("745C8A074930444D00A159A7022A70A3")
                },

                new AccountModel
                {
                    UserID = 3,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "test@example.com",
                    listOfRoles = new List<UserRoleModel>
                    {
                        new UserRoleModel { RoleID = 1, RoleName = "Admin" },
                        new UserRoleModel { RoleID = 2, RoleName = "User" }
                    }
                }
            };


            _stubUserDAL = new Mock<IUserDAL>();
            _stubAccountDAL = new Mock<IAccountDAL>();

            _stubAccountDAL.Setup(accountDAL => accountDAL.GetUserCredentialsAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => _accountsRepository.FirstOrDefault((a) => a.Email == email));

            _stubAccountDAL.Setup(a => a.RegisterAsync(It.IsAny<RegisterViewModel>()))
                           .ReturnsAsync((RegisterViewModel model) => {
                               var user = new UserModel()
                               {
                                   FirstName = model.FirstName,
                                   LastName = model.LastName,
                                   NIC = model.NIC,
                                   DepartmentID = model.DepartmentID,
                                   Email = model.Email,
                                   ManagerID = model.ManagerID,
                               };  
                               user.UserID = _usersRepository.Max(a => a.UserID)+1;
                               var account = new AccountModel()
                               {
                                   UserID = user.UserID,
                                   HashedPassword = model.HashedPassword,
                                   SaltValue = model.SaltValue
                               };

                                _usersRepository.Add(user);
                                _accountsRepository.Add(account);
                              return true;
                           });


            _stubUserDAL.Setup(userDAL => userDAL.IsEmailAlreadyExistsAsync(It.IsAny<string>()))
                        .ReturnsAsync((string email) => _usersRepository.Any(u => u.Email == email));


            _stubUserDAL.Setup(userDAL => userDAL.IsNICExistsAsync(It.IsAny<string>()))
                        .ReturnsAsync((string nic) => _usersRepository.Any(u => u.NIC == nic));


            _stubUserDAL.Setup(userDAL => userDAL.IsMobileNumberExistsAsync(It.IsAny<string>()))
                        .ReturnsAsync((string mobileNumber) => _usersRepository.Any(u => u.MobileNumber == mobileNumber));


            _stubAccountDAL.Setup(accountDAL => accountDAL.GetUserDetailsWithRolesAsync(It.IsAny<AccountModel>()))
                  .ReturnsAsync((AccountModel account) => _accountsRepository.FirstOrDefault(u => u.Email == account.Email));


            _accountService = new AccountService(_stubAccountDAL.Object, _stubUserDAL.Object);
        }

        [Test]
        [TestCase("Peter", "Parker", "N051100150456B", "59749958", "admin@gmail.com", 1, 1, "employee")]
        [TestCase("Bruce", "Bannner", "N051100150456D", "58386119", "bruce@gmail.com", 1, 2, "employee")]
        public async Task TestUserRegistration_ValidInput_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName,string NIC, string mobile, string Email, int ManagerID, int DepartmentID, string password)
        {
            //Arrange
            var user = new RegisterViewModel()
            {

                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = Email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            //Act
            var result = await _accountService.RegisterUserAsync(user);

            //Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsNull(result.Errors);
        }


        [Test]
        [TestCase("Peter", "Parker", "", "59749958", "admin@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_MissingNICInput_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName, string NIC, string mobile, string Email, int ManagerID, int DepartmentID, string password)
        {
            //Arrange
            var user = new RegisterViewModel()
            {

                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = Email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            //Act
            var result = await _accountService.RegisterUserAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("NIC is required."));
        }


        [Test]
        [TestCase("Peter", "Parker", "N051100150456B", "", "admin@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_MissingMobileNumberInput_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName, string NIC, string mobile, string Email, int ManagerID, int DepartmentID, string password)
        {
            //Arrange
            var user = new RegisterViewModel()
            {

                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = Email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            //Act
            var result = await _accountService.RegisterUserAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Mobile Number is required."));
        }

        [Test]
        [TestCase("Peter", "Parker", "", "59749958", "", 1, 1, "employee")]
        public async Task TestUserRegistration_MissingEmailInput_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName, string NIC, string mobile, string Email, int ManagerID, int DepartmentID, string password)
        {
            //Arrange
            var user = new RegisterViewModel()
            {

                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = Email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            //Act
            var result = await _accountService.RegisterUserAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Email is required."));
        }

        [Test]
        [TestCase("John", "Doe", "N0511001504500", "58389912", "divesh@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_DuplicateEmail_ResultIsRegistrationNotSuccessfulWithError(string firstName, string lastName, string NIC, string mobile, string Email, int ManagerID, int DepartmentID, string password)
        {
            //Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = Email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            //Act
            var result = await _accountService.RegisterUserAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Email is already in use."));
        }

        [Test]
        [TestCase("Peter", "Parker", "N051100150457A", "59749908", "admiccn@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_DuplicateNIC_ResultIsRegistrationNotSuccessfulWithError(string firstName, string lastName, string NIC, string mobile, string Email, int ManagerID, int DepartmentID, string password)
        {
            //Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = Email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            //Act
            var result = await _accountService.RegisterUserAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("NIC is already in use."));
        }

        [Test]
        [TestCase("Peter", "Parker", "N051100150456Z", "58386112", "admicccn@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_DuplicateMobileNumber_ResultIsRegistrationNotSuccessfulWithError(string firstName, string lastName, string NIC, string mobile, string Email, int ManagerID, int DepartmentID, string password)
        {
            //Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = Email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            //Act
            var result = await _accountService.RegisterUserAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotEmpty(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Mobile Number is already in use."));
        }


        [Test]
        [TestCase("Peter", "Parker", "N051900150456Z", "58387712", "ValidEmail@gmail.com", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N059100150456Z", "58387712", "john.jones@gmail.com", 1, 1, "employee")]

        public async Task TestUserRegistration_ValidEmailRegexPatterns_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsNull(result.Errors);       
        }


        [Test]
        [TestCase("Peter", "Parker", "N051900150456Z", "58387712", "Valid##Email@gmail.com", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N059100150456Z", "58387712", "john.jones@ceridian.com", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N059100150456Z", "58387712", "john.jones@ceridian.umail.uom.ac", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N059100150456Z", "58387712", "john..jones@gmail.com", 1, 1, "employee")]

        public async Task TestUserRegistration_InValidEmailRegexPatterns_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Email is required and must be in a valid format."));
        }



        [Test]
        [TestCase("Peter", "Parker", "N051@015045%Z", "58387712", "ValidEmail@gmail.com", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N05.9100-0456Z", "58387712", "john.jones@gmail.com", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N059100 4x0456Z", "58387712", "john.jones@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_InValidNICRegexPatterns_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("NIC is required and must be a valid format."));
        }


        [Test]
        [TestCase("Peter", "Parker", "N051900150456Z", "+2307712", "ValidEmail@gmail.com", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N059100150456Z", "98387712", "john.jones@gmail.com", 1, 1, "employee")]
        [TestCase("Peter", "Parker", "N059100150456Z", "983 877 12", "john.jones@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_InValidMobileNumberRegexPatterns_ResultIsRegistrationSuccessfulAndNoErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Mobile Number is required and must be in a valid format."));
        }

        [Test]
        [TestCase("J", "Doe", "N051100150459B", "52345678", "Doe@gmail.com", 1, 1, "employee")]
        [TestCase("Isthisthereallifeisthisjustfantasycaughtinalandslidenoescpaefromrealityopenyoureyeslookuptotheskiesandseeimjustapoorboyineednosympathy", "Doe", "N051100150459B", "52345678", "ValidEmail@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_InvalidFirstNameLength_ResultIsRegistrationUnsuccessfulAndErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("First Name must be between 2 and 50 characters."));
        }


        [Test]
        [TestCase("JojoRabbit", "D", "N051100150459B", "52345678", "Doe@gmail.com", 1, 1, "employee")]
        [TestCase("John", "Isthisthereallifeisthisjustfantasycaughtinalandslidenoescpaefromrealityopenyoureyeslookuptotheskiesandseeimjustapoorboyineednosympathy", "N051100150459B", "52345678", "ValidEmail@gmail.com", 1, 1, "employee")]
        public async Task TestUserRegistration_InvalidLastNameLength_ResultIsRegistrationUnsuccessfulAndErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Last Name must be between 2 and 50 characters."));
        }

        [Test]
        [TestCase("J", "Doe", "N051100150459B", "52345678", "thisIsAEmailThatIsLOngerThan30chaarterslongithinkIhaverecahed30rihthhhhhhh@gmail.com", 1, 1, "employee")]
        [TestCase("JonDow", "Doe", "N051100150459B", "52345678", "2", 1, 1, "employee")]
        public async Task TestUserRegistration_InvalidEmailLength_ResultIsRegistrationUnsuccessfulAndErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Email must be between 2 and 30 characters."));
        }


        [Test]
        [TestCase("Peter", "Parker", "N051100150456B12212121", "59749958", "admin@gmail.com", 1, 1, "employee")]
        [TestCase("Bruce", "Bannner", "N0511D", "58386119", "bruce@gmail.com", 1, 2, "employee")]
        public async Task TestUserRegistration_InvalidNICLength_ResultIsRegistrationUnsuccessfulAndErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("NIC must be exactly 14 characters."));
        }

        [Test]
        [TestCase("Peter", "Parker", "N051100150456A", "222", "admin@gmail.com", 1, 1, "employee")]
        [TestCase("Bruce", "Bannner", "N051100150456A", "583861333319", "bruce@gmail.com", 1, 2, "employee")]
        public async Task TestUserRegistration_InvalidMobileNumberLength_ResultIsRegistrationUnsuccessfulAndErrors(string firstName, string lastName, string NIC, string mobile, string email, int ManagerID, int DepartmentID, string password)
        {
            // Arrange
            var user = new RegisterViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                NIC = NIC,
                MobileNumber = mobile,
                Email = email,
                ManagerID = ManagerID,
                DepartmentID = DepartmentID,
                Password = password
            };

            // Act
            var result = await _accountService.RegisterUserAsync(user);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Contains("Mobile Number must be exactly 8 characters."));
        }

        //[Test]
        //public async Task TestGetUserDetailsWithRolesAsync_UserExists_ReturnsUserWithRoles()
        //{
        //    // Arrange
        //    var accountModel = new AccountModel { Email = "test@example.com" };
        //    var expectedUser = new AccountModel
        //    {
        //        UserID = 3,
        //        FirstName = "John",
        //        LastName = "Doe",
        //        Email = "test@example.com",
        //        listOfRoles = new List<UserRoleModel>
        //        {
        //            new UserRoleModel { RoleID = 1, RoleName = "Admin" },
        //            new UserRoleModel { RoleID = 2, RoleName = "User" }
        //        }
        //    };

        //    //_stubAccountDAL.Setup(accountDAL => accountDAL.GetUserDetailsWithRolesAsync(It.IsAny<AccountModel>()))
        //    //               .ReturnsAsync(expectedUser);

        //    // Act
        //    var result = await _accountService.GetUserDetailsWithRolesAsync(accountModel);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.AreEqual(expectedUser.UserID, result.UserID);
        //    Assert.AreEqual(expectedUser.FirstName, result.FirstName);
        //    Assert.AreEqual(expectedUser.LastName, result.LastName);
        //    Assert.AreEqual(expectedUser.Email, result.Email);
        //   // Assert.AreEqual(expectedUser.listOfRoles[0], result.listOfRoles[1]);
        //}

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
