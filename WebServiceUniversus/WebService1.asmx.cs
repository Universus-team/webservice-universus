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

        //[SoapHeader("Authentication", Required = true)]
        //[WebMethod]
        //public int addStudentToGroup(int studentId, int groupId)
        //{
        //    /*if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
        //        && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
        //        || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id
        //        || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id))
        //    {*/
        //    Student student = StudentDAO.getById(studentId);
        //    student.GroupId = groupId;
        //    return StudentDAO.update(studentId, student);
        //    //}
        //    return -1;
        //}

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public bool sendMessage(int toUserId, string strMess)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if ((account != null) && (AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5))
            {
                MessageDAO.add(account.Id, toUserId, strMess);
                return true;
            }
            return false;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public bool checkNewMessage(int fromUser)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if ((account != null) && (AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5))
            { 
                return MessageDAO.checkNewMessage(account.Id, fromUser);
            }
            return false;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public List<string> getDialogs()
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if ((account != null) && (AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5))
            {
                return MessageDAO.getDialogs(account.Id);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public List<Message> getDialog(int userId)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if ((account != null) && (AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5))
            {
                return MessageDAO.getDialog(account.Id, userId);
            }
            return null;
        }



        [SoapHeader("Authentication", Required = true)
]
        [WebMethod(Description = @"<H2>returt account id by username and password</H2>
        <br><b>Return:</b>
        <br>-1 if incorrect password 
        <br>-2 if incorrect username")]
        public int getId()
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account == null) return -2; //username not found
            string passwordMd5 = AccountDAO.getMD5(Authentication.Password);
            if ( passwordMd5 == account.PasswordMD5)
            {
                return account.Id;
            }
            return -1;//incorrect password
        }

        [WebMethod]
        public String getRoleNameByUserId(int id)
        {
            Role role = RoleDAO.getRoleByUserId(id);
            if (role != null)
            {
                return role.Name;
            }
            return null;
        }

        [WebMethod]
        public int getUserIdByUsername(string username)
        {
            if (username == null) return -1;
            Account account = AccountDAO.getByUsername(username);
            if (account == null) return -2;
            return account.Id;
        }

        [WebMethod]
        public string getUsernameById(int id)
        {
            Account account = AccountDAO.getById(id);
            if (account == null) return null;
            return account.Username;
        }


    }

}
