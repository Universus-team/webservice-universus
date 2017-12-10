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
        int testId = 0;
        String testStr = "test";
        DateTime testDate = new DateTime(1970, 01, 01);
        int testInt = 1;


        [TestCase]
        public void getStudentById()
        {
            Student test = StudentDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, 0);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.Surname, testStr);
            Assert.AreEqual(test.Email, testStr);
            Assert.AreEqual(test.PhoneNumber, testStr);
            Assert.AreEqual(test.GroupId, testInt);
        }

        [TestCase]
        public void getStudentGroupById()
        {
            StudentGroup test = StudentGroupDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, 0);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.CreatedDate, testDate);
            Assert.AreEqual(test.Email, testStr);
            Assert.AreEqual(test.LeaderID, testInt);
            Assert.AreEqual(test.ManagerID, testInt);
        }

        [TestCase]
        public void getSpecialityById()
        {
            Speciality test = SpecialityDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, 0);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.SpecialityCode, testStr);
            Assert.AreEqual(test.Description, testStr);
        }

        [TestCase]
        public void getRoleById()
        {
            Role test = RoleDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, 0);
            Assert.AreEqual(test.Name, testStr);
        }

        [TestCase]
        public void getAccountById()
        {
            Account test = AccountDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, 0);
            Assert.AreEqual(test.Username, testStr);
            Assert.AreEqual(test.PasswordMD5, testStr);
            Assert.AreEqual(test.RoleId, testInt);
        }

        [TestCase]
        public void getDepartmentById()
        {
            Department test = DepartmentDAO.getById(testId);
            Assert.NotNull(test);
            Assert.AreEqual(test.Id, 0);
            Assert.AreEqual(test.Name, testStr);
            Assert.AreEqual(test.Email, testStr);
            Assert.AreEqual(test.PhoneNumber, testStr);
        }

    }
}
