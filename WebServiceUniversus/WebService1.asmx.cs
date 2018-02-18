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

	// Класс для хранения аутентификационных данных
	// 
    public class AuthHeader : SoapHeader
    {
        public string Username;
        public string Password;
    }


    /// <summary>
    /// Сводное описание для WebService1
    /// </summary>
    [WebService(Namespace = "http://universus-webservice.ru/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]

    public class WebService1 : System.Web.Services.WebService
    {
    	// ---КАК ПРОХОДИТ ИДЕНТИФИКАЦИЯ и АУТЕНТИФИКАЦИЯ---
    	// bool identification(username, password) -- проверяет, есть пользователь username в системе
    	// если нет вернёт false, иначе проверит password, если password для username верен вернёт true

    	// ---АВТОРИЗАЦИЯ---
    	// проходит спомощью методов bool isStudent(username), bool isTeacher(username), bool isModerator(username),
    	// bool isAdmin(username) которые вернут true если username принадлежит к той или иной группе пользователей
    	// ВНИМАНИЕ перед авторизацией обязательно необходимо провести аутентификацию

    	// ---хранение USERNAME & PASSWORD---
    	// username и password хранятся в SOAP HEADER
    	// чтобы использовать эту возможность перед методом веб-сервиса необходимо добавить строчку 
    	// [SoapHeader("Authentication", Required = true)]
    	// потом в самом методе достать данные из объекта Authentication

    	
        public AuthHeader Authentication;

        [WebMethod]
        public List<Student> getAllStudents()
        {
            return StudentDAO.getAll();
        }

        [WebMethod]
        public List<Department> getAllDepartmentsByUniversityId(int id)
        {
            return DepartmentDAO.getAllByUniversityId(id);
        }

        [WebMethod]
        public Student getStudentById(int Id)
        {
            return StudentDAO.getById(Id);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int deleteStudentById(int Id)
        {
            if (identification(Authentication.Username, Authentication.Password)
                && (isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
            {
                return StudentDAO.deleteById(Id);
            }
            return -1;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addModeratorAccount(Account account)
        {
            if (identification(Authentication.Username, Authentication.Password)
                  && isAdmin(Authentication.Username))
            {
                account.RoleId = 2;
                return AccountDAO.add(account);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int deleteModeratorAccountById(int id)
        {
            if (identification(Authentication.Username, Authentication.Password)) return -3;
            if (isAdmin(Authentication.Username))
            {
                Account account = AccountDAO.getById(id);
                if (isModerator(account.Username))
                    return AccountDAO.deleteById(id);
                else
                    return -4;
            } else
            {
                return -2;
            }
           
        }

        [WebMethod]
        public int addStudent(Student student)
        {
            return StudentDAO.add(student);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addUniversity(University uni)
        {
            if (identification(Authentication.Username, Authentication.Password)
                && (isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
            {

                return UniversityDAO.add(uni);
            }
            return -1;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addDepartment(Department d)
        {
            if (identification(Authentication.Username, Authentication.Password)
                && (isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
            {

                return DepartmentDAO.add(d);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addStudentGroup(StudentGroup s)
        {
            if (identification(Authentication.Username, Authentication.Password)
                && (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
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
            if (identification(Authentication.Username, Authentication.Password)
                            && (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
            {

                return StudentGroupDAO.getAllByManagerId(id);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addExamHistory(ExamHistory ex)
        {
            if (identification(Authentication.Username, Authentication.Password)
                && (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
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
            if (identification(Authentication.Username, Authentication.Password))
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
            if (identification(Authentication.Username, Authentication.Password)
                && (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
            {

                return ExamDAO.add(e);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addStudentToGroup(int studentId, int groupId)
        {
            if (identification(Authentication.Username, Authentication.Password)
                && (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username)))
            {
                Student student = StudentDAO.getById(studentId);
                student.GroupId = groupId;
                return StudentDAO.update(student);
            }
            return -1;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public bool sendMessage(int toUserId, string strMess)
        {
            
            if (identification(Authentication.Username, Authentication.Password))
            {
                Account account = AccountDAO.getByUsername(Authentication.Username);
                MessageDAO.add(account.Id, toUserId, strMess);
                return true;
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
            return ExamDAO.getAll();
        }

        [WebMethod]
        public Exam getExamById(int id)
        {
            Exam exam = ExamDAO.getById(id);
            exam.clearAnswer();
            return exam;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public List<string> getDialogs()
        {
            if (identification(Authentication.Username, Authentication.Password))
            {
                Account account = AccountDAO.getByUsername(Authentication.Username);
                return MessageDAO.getDialogs(account.Id);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public List<Message> getDialog(int userId)
        {
            
            if (identification(Authentication.Username, Authentication.Password))
            {
                Account account = AccountDAO.getByUsername(Authentication.Username);
                return MessageDAO.getDialog(account.Id, userId);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public float passExam(Exam exam)
        {

            if (identification(Authentication.Username, Authentication.Password))
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

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int getId()
        {
            if ( identification(Authentication.Username, Authentication.Password))
            {
                return AccountDAO.getByUsername(Authentication.Username).Id;
            }
            return -2;
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

        [WebMethod]
        public Subject getSubjectById(int id)
        {
            return SubjectDAO.getById(id);
        }

        [WebMethod]
        public List<Subject> getAllSubject()
        {
            return SubjectDAO.getAll();
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int updateSubject(Subject subject)
        {
            if (identification(Authentication.Username, Authentication.Password)) return -2;
            if (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username))
            {
                return SubjectDAO.update(subject);
            }
            else
            {
                return -3;
            }
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int addSubject(Subject subject)
        {
            if (identification(Authentication.Username, Authentication.Password)) return -2;
            if (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username))
            {
                return SubjectDAO.add(subject);
            } else
            {
                return -3;
            }
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public int deleteSubjectById(int id)
        {
            if (identification(Authentication.Username, Authentication.Password)) return -2;
            if (isTeacher(Authentication.Username) || isModerator(Authentication.Username) || isAdmin(Authentication.Username))
            {
                return SubjectDAO.deleteById(id);
            }
            else
            {
                return -3;
            }
        }

        private bool identification(string username, string password)
        {
            Account account = AccountDAO.getByUsername(Authentication.Username);
            if (account == null) return false; //username not found
            string passwordMd5 = AccountDAO.getMD5(Authentication.Password);
            return passwordMd5 == account.PasswordMD5;
        }

        private bool isStudent(string username)
        {
            return AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("student").Id;
        }

        private bool isTeacher(string username)
        {
            return AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("teacher").Id;
        }

        private bool isModerator(string username)
        {
            return AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("moderator").Id;
        }

        private bool isAdmin(string username)
        {
            return AccountDAO.getByUsername(Authentication.Username).RoleId == RoleDAO.getByName("admin").Id;
        }



    }

}
