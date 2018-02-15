using NUnit.Framework;
using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{

    [TestFixture]
    class Test
    {
        static int testId = 0;
        static String testStr = "test";
        static DateTime testDate = new DateTime(1970, 01, 01);
        static bool testBool = true;
        static int testInt = 1;
        static Account account = new Account(testStr, testStr, testStr, testStr, testStr, testInt, testStr, testStr);
        static Student student = new Student(account, 1);

        [TestCase]
        public void getStudentById()
        {
            Student test = StudentDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, testId);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.Surname, testStr);
            Assert.AreEqual(test.Email, testStr);
            Assert.AreEqual(test.Phone, testStr);
            Assert.AreEqual(test.GroupId, testInt);
        }

        [TestCase]
        public void getStudentGroupById()
        {
            StudentGroup test = StudentGroupDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, testId);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.CreatedDate, testDate);
            Assert.AreEqual(test.Email, testStr);
            Assert.AreEqual(test.ManagerID, testInt);
        }


        [TestCase]
        public void getRoleById()
        {
            Role test = RoleDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, testId);
            Assert.AreEqual(test.Name, testStr);
        }

        [TestCase]
        public void getAccountById()
        {
            Account test = AccountDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, testId);
        }

        [TestCase]
        public void getDepartmentById()
        {
            Department test = DepartmentDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, testId);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.Email, testStr);
            Assert.AreEqual(test.PhoneNumber, testStr);
        }

        [TestCase]
        public void getUniversityById()
        {
            University test = UniversityDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, testId);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.HasStateAccreditation, testBool);
            Assert.AreEqual(test.Address, testStr);
        }

        [TestCase]
        public void getMessageById()
        {
            Message test = MessageDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, testId);
            Assert.AreEqual(test.FromUserId, testInt);
            Assert.AreEqual(test.ToUserId, testInt);
            Assert.AreEqual(test.MessageContent, testStr);
            Assert.AreEqual(test.ItRead, testBool);
        }

        [TestCase]
        public void deleteStudentById()
        { 
            int id = StudentDAO.add(student);
            StudentDAO.deleteById(id);
            Assert.Null(StudentDAO.getById(id));
        }


        [TestCase]
        public void deleteAccountById()
        {
            int id = AccountDAO.add(account);
            AccountDAO.deleteById(id);
            Assert.Null(AccountDAO.getById(id));
        }

        [TestCase]
        public void deleteRoleById()
        {
            int id = RoleDAO.add(testStr);
            RoleDAO.deleteById(id);
            Assert.Null(RoleDAO.getById(id));
        }

    }
}
