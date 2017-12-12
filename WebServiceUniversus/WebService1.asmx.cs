using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;


namespace WebServiceUniversus
{
    public class AuthHeader : SoapHeader
    {
        public string Username;
        public string Password;
    }
    /// <summary>
    /// Сводное описание для WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        public AuthHeader Authentication;

        [WebMethod(Description = @"<H2>Get all students from database</H2>
           <br> <b>Return:</b>
           <br>List of students")]
        public List<Student> getAllStudents()
        {
            return StudentDAO.getAll();
        }

        [WebMethod(Description = @"<H2>Get a student by Id</H2>
        <br> <b>Parameters:</b>
        <br> int Id - Id of Student
        <br> <b>Return:</b>
        <br>Student")]
        public Student getStudentById(int Id)
        {
            return StudentDAO.getById(Id);
        }

        //[WebMethod(Description = @"<H2>Get a speciality by Id</H2>
        //<br> <b>Parameters:</b>
        //<br> int Id - Id of speciality
        //<br> <b>Return:</b>
        //<br>speciality")]
        //public Speciality getSpecialityById(int Id)
        //{
        //    return SpecialityDAO.getById(Id);
        //}

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Delete a student by Id</H2>
        <br> <b>Parameters:</b>
        <br> int Id - Id of Student
        <br> <b>Return:</b>
        <br>int - number of delete students")]
        public int deleteStudentById(int Id)
        {
            //if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
            //    && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
            //    || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id))
            //{
                return StudentDAO.deleteById(Id);
            //}
            //return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Delete all students</H2>
        <br><b>Return:</b>
        <br>int - number of delete students")]
        public int deleteAllStudents()
        {
            //if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
            //    && AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id)
            //{
                return StudentDAO.deleteAll();
            //}
            //return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Add new moderator account</H2>
        <br><b>Return:</b>
        <br>int - 1 if moderator added, o if moderator did not add, -1 if error")]
        public int addModeratorAccount(string username, string password)
        {
            if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
                && AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id)
            {
                return AccountDAO.add(username, password, RoleDAO.getByName("moderator").Id);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Add new student</H2>
        <br><b>Return:</b>
        <br>int - 1 if student added, o if student did not add, -1 if error")]
        public int addStudent(string name, string surname, string email, string phone_number, int group_id)
        {
             return StudentDAO.add(new Student(0, name, surname, email, phone_number, group_id));
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addStudentToGroup(int studentId, int groupId)
        {
            /*if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id))
            {*/
                Student student = StudentDAO.getById(studentId);
                student.GroupId = groupId;
                return StudentDAO.update(studentId, student);
            //}
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public bool sendMessage(int fromUserId, int toUserId, string strMess)
        {
            if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getById(fromUserId).PasswordMD5)
            {
                MessageDAO.add(fromUserId, toUserId, strMess);
                return true;
            }
            return false;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public Message receiveMessage(int userId)
        {
            if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getById(userId).PasswordMD5)
            {
                return MessageDAO.getMessageTo(userId);
            }
            return null;
        }




    }

}
