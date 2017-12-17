using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

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

        [WebMethod(Description = @"<H2>Get all departments by university id</H2>
           <br> <b>Return:</b>
           <br>List of students")]
        public List<Department> getAllDepartmentsByUniversityId(int id)
        {
            return DepartmentDAO.getAllByUniversityId(id);
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

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Delete a student by Id</H2>
        <br> <b>Parameters:</b>
        <br> int Id - Id of Student
        <br> <b>Return:</b>
        <br>int - number of delete students")]
        public int deleteStudentById(int Id)
        {
            if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id))
            {
                return StudentDAO.deleteById(Id);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Delete all students</H2>
        <br><b>Return:</b>
        <br>int - number of delete students")]
        public int deleteAllStudents()
        {
            if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
                && AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id)
            {
                return StudentDAO.deleteAll();
            }
            return -1;
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
        [WebMethod(Description = @"<H2>Add new university</H2>
        <br><b>Return:</b>
        <br>1 if university added,
        <br> o if university did not add,
        <br> -1 if error")]
        public int addUniversity(University uni)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account != null &&
                AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id))
            {

                return UniversityDAO.add(uni);
            }
            return -1;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Add new university departmaent</H2>
        <br><b>Return:</b>
        <br>1 if  university departmaent added,
        <br> o if  university departmaent did not add,
        <br> -1 if error")]
        public int addDepartment(Department d)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account != null &&
                AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id))
            {

                return DepartmentDAO.add(d);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Add new student_group</H2>
        <br><b>Return:</b>
        <br>1 if  student_group departmaent added,
        <br> o if  student_group departmaent did not add,
        <br> -1 if error")]
        public int addStudentGroup(StudentGroup s)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account != null &&
                AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id))
            {

                return StudentGroupDAO.add(s);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public List<StudentGroup> getAllStudenyGroupByManagerId(int id)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account != null &&
                AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id))
            {

                return StudentGroupDAO.getAllByManagerId(id);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addExamHistory(ExamHistory ex)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account != null &&
                AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id))
            {

                return ExamHistoryDAO.add(ex);
            }
            return -1;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public List<ExamHistory> getAllExamHistoryByStudentId(int id)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account != null && AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5)
            {

                return ExamHistoryDAO.getAllByStudentId(account.Id);
            }
            return null;
        }

        [WebMethod]
        public ExamStatus getExamStatusById(int id)
        {
            return ExamStatusDAO.getById(id);
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = @"<H2>Add new exam</H2>
        <br><b>Return:</b>
        <br>1 if exam added,
        <br> o if  exam did not add,
        <br> -1 if error")]
        public int addExam(Exam e)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account != null &&
                AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id))
            {

                return ExamDAO.add(e);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addStudentToGroup(int studentId, int groupId)
        {
            if (AccountDAO.getMD5(Authentication.Password) == AccountDAO.getByUsername(Authentication.Username).PasswordMD5
                && (AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id
                || AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id))
            {
                Student student = StudentDAO.getById(studentId);
            student.GroupId = groupId;
            return StudentDAO.update(studentId, student);
            }
            return -1;
        }

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

        [WebMethod]
        public List<University> getAllUniversities()
        {
            return UniversityDAO.getAll();
        }

        [WebMethod]
        public List<Exam> getAllExams()
        {
            return Exa.getAll();
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

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public float passExam(Exam exam)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if ((account != null) && (AccountDAO.getMD5(Authentication.Password) == account.PasswordMD5))
            {
                Exam originalExam = ExamDAO.getById(exam.Id);
                XmlDocument xDoc1 = new XmlDocument();
                xDoc1.LoadXml(exam.Content);
                XmlElement xRoot = xDoc1.DocumentElement;
                XmlNodeList items1 = xRoot.SelectNodes("//item");

                XmlDocument xDoc2 = new XmlDocument();
                xDoc1.LoadXml(originalExam.Content);
                XmlElement xRoot2 = xDoc2.DocumentElement;
                XmlNodeList items2 = xRoot2.SelectNodes("//item");
                int incorrect = 0;
                for (int i=0; i<items1.Count; i++)
                {
                    incorrect += (items1.Item(i).Attributes["correct"].Value != items2.Item(i).Attributes["correct"].Value)
                        ? 1 : 0;
                }
                return (incorrect / 2) / exam.CountOfQuestion;

            }
            return 0.0f;
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
        public Role getRoleById(int id)
        {
            return RoleDAO.getRoleByUserId(id);
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
