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
        Student student;

        //[SetUp]
        //public void init()
        //{
        //    student.Id = 1;
        //    student.Name = "Mikhail";
        //    student.Surname = "Kurochkin";
        //    student.Email = "mkv-1724@mail.ru";
        //    student.PhoneNumber = "79157510930";
        //    student.GroupId = 1;
        //}

        [TestCase]
        public void getStudentById()
        {
            Student test = StudentDAO.getById(1);
            Assert.AreEqual(test.Id, 1);
            Assert.AreEqual(test.Name, "Mikhail");
            Assert.AreEqual(test.Surname, "Kurochkin");
            Assert.AreEqual(test.Email, "mkv-1724@mail.ru");
            Assert.AreEqual(test.PhoneNumber, "79157510930");
            Assert.AreEqual(test.GroupId, 2);

        }
    }
}
