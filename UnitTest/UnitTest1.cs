using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseLib;


namespace UnitTest
{
    [TestClass]
    public class DAOTest
    {

        Student testStudent;

        [TestFixtureSetUp]
        public void Init()
        {
        }


        [TestMethod]
        public void getStudentById()
        {
            Student student = StudentDAO.getById(1);
            Assert.AreEqual(student.Id, );
        }
    }
}
