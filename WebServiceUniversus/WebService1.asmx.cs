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

    // Developer : Mikhail Kurochkin
    // e-mail : mkv-1724@mail.ru
    // address : city Vladimir state Russia
    // License : MIT
    // sorry all comments in Russians

	// Класс для хранения аутентификационных данных
    public class AuthHeader : SoapHeader
    {
        public string Email;
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

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Список всех тестов авторы которых, состоят в отделе по идентификатору, указанному в параметрах. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если состоит в отделе по указанному идентификатору) <br>
        Модератор: + <br>
        Админ: + <br>")]
        public List<Exam> getAllExamsByDepartmentId(int id)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email))
                {
                    Account account =AccountDAO.getByEmail(Authentication.Email);
                    if (account.DepartmentId != id) return null;
                }
                return ExamDAO.getAllByDepartmentIdLite(id);
            }
            return null;
            
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Принятие теста. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + <br>
        Преподаватель: - <br>
        Модератор: - <br>
        Админ: - <br>
        Возврат: процент правильных ответов в формате от 0 до 1 (100%) <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : данные истории тестирования были изменены <br>
        -4 : крайнее время выполнения теста истекло <br>
        -5 : тест уже был пройден <br>")]
        public float takeExam(ExamHistory eh)
        {
            if (identification(Authentication.Email, Authentication.Password) && isStudent(Authentication.Email) )
            {
                ExamHistory eh2 = ExamHistoryDAO.getById(eh.Id);
                if (eh2.StatusId != 1) return -5;
                if (eh2.StudentId != eh.StudentId) return -3;
                if (eh2.TeacherId != eh.TeacherId) return -3;
                if (eh2.ExamId != eh.ExamId) return -3;
                if (eh2.PassingScore != eh.PassingScore) return -3;

                if (eh2.Deadline.CompareTo(DateTime.UtcNow) <= 0) return -4;


                eh.DateOfTest = DateTime.UtcNow;
                eh.Result = eh2.Exam.compare(eh.Exam);
                eh.StatusId = (eh.Result >= eh.PassingScore) ? 3 : 2;

                if (ExamHistoryDAO.update(eh) == -1) return -1;

                return eh.Result;
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получение объекта теста по его идентификатору. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если входит в тот же отдел университета, что и автор теста)<br>
        Модератор: + <br>
        Админ: + <br>")]
        public Exam getExamById(int id)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                Exam exam = ExamDAO.getById(id);
                if (isTeacher(Authentication.Email))
                {
                    Account account = AccountDAO.getByEmail(Authentication.Email);
                    Account examAcc = AccountDAO.getById(exam.AuthorId);
                    if (account.DepartmentId != examAcc.DepartmentId) return null;
                }
                return exam;
            }
            return null;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавление новой истории тестирования (назначение теста студенту). <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int addExamHistory(ExamHistory ex)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                Account acc = AccountDAO.getByEmail(Authentication.Email);
                ex.TeacherId = acc.Id;
                ex.StatusId = 1;
                return ExamHistoryDAO.add(ex);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получение объекта истории тестирования. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + (Если ExamHistory.StudentId == идентификатору студента)<br>
        Преподаватель: + (Если ExamHistory.TeacherId == идентификатору преподавателя)<br>
        Модератор: - <br>
        Админ: - <br>")]
        public ExamHistory getExamHistoryById(int id)
        {
            if (identification(Authentication.Email, Authentication.Password))
            {

                Account acc = AccountDAO.getByEmail(Authentication.Email);
                ExamHistory ex = ExamHistoryDAO.getById(id);
                if (isStudent(Authentication.Email))
                {
                    ex.Exam.clearAnswer();
                    if (ex.StudentId != acc.Id) return null;
                } else if (isTeacher(Authentication.Email))
                {
                    if (ex.TeacherId != acc.Id) return null;
                } else
                {
                    return null;
                }

                return ex;



            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получение списка истории тестирования по идентификатору студента. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + (если входной идентификатор равен идентификатору аккаунта)<br>
        Преподаватель: - <br>
        Модератор: - <br>
        Админ: - <br>")]
        public List<ExamHistory> getAllExamHistoryByStudentId(int id)
        {
            if (identification(Authentication.Email, Authentication.Password))
            {
                if (isStudent(Authentication.Email))
                {
                    Account acc = AccountDAO.getByEmail(Authentication.Email);
                    if (id != acc.Id) return null;
                    
                }
                return ExamHistoryDAO.getAllByStudentId(id);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получение списка истории тестирования по идентификатору теста и назначавшего преподавателя. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если назначал тестирование) <br>
        Модератор: - <br>
        Админ: - <br>")]
        public List<ExamHistory> getAllExamHistoryByTeacherId(int accountId, int examId)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email))
                {
                    Account acc = AccountDAO.getByEmail(Authentication.Email);
                    if (accountId != acc.Id) return null;

                }
                return ExamHistoryDAO.getAllByTeacherId(accountId, examId);
            }
            return null;
        }

        [WebMethod(Description =
        @"
        Получение статуса истории тестирования по идентификатору статуса. <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public ExamStatus getExamStatusById(int id)
        {
            return ExamStatusDAO.getById(id);
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавление нового теста. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 если всё прошло удачно <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int addExam(Exam e)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                e.AuthorId = account.Id;
                return ExamDAO.add(e);
            }
            return -2;
        }

        //--- ACCOUNT ---


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получение своего аккаунта. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        [SoapHeader("Authentication", Required = true)]
        [WebMethod]
        public Account getAccount()
        {
            if (identification(Authentication.Email, Authentication.Password))
            {
                return AccountDAO.getByEmail(Authentication.Email);
            }
            return null;
        }

        [WebMethod(Description =
        @"Получение аккаунта по его идентификатору (хеш пароля не переда). <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public Account getAccountById(int id)
        {
            return AccountDAO.getByIdWithoutPassword(id);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Обновление данных аккаунта. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 если всё прошло удачно <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int updateAccount(Account account)
        {
            Account acc = AccountDAO.getByEmail(Authentication.Email);
            account.Id = acc.Id;
            account.PasswordMD5 = acc.PasswordMD5;
            account.Email = acc.Email;

            return AccountDAO.update(account);

        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Удаление аккаунта по его идентификатору. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: + (может удалять только преподавателей и студентов)<br>
        Админ: + (может удалять всех пользователей)<br>
        Возврат: 1 если всё прошло удачно <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : модератор не может удалить другого модератора или админа")]
        public int deleteAccountById(int id)
        {

            if (identification(Authentication.Email, Authentication.Password)
                  && (isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                
                Account account = AccountDAO.getById(id);
                if (account == null) return 0;

                // если модератор удаляет другого модератора или админа даём ему отказ
                if ((account.RoleId == 3 || account.RoleId == 4) && isModerator(Authentication.Email))
                {
                    return -3;
                }

                // удаляем всю историю пользователя
                MessageDAO.deleteAllMessageByUserId(id);
                if (account.RoleId == 1) StudentToGroupDAO.deleteAllByUserId(id);
                if (account.RoleId == 2) TeacherToGroupDAO.deleteAllByUserId(id);

                // удаляем самого пользователя
                return AccountDAO.deleteById(id);
            }
            return -2;
        }

        [WebMethod(Description =
        @"
        Получение списка всех студентов системы. <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public List<Account> getAllStudents()
        {
            return AccountDAO.getAllByRoleId(1);
        }

        [WebMethod(Description =
        @"
        Получение списка всех преподавателей системы. <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public List<Account> getAllTeachers()
        {
            return AccountDAO.getAllByRoleId(2);
        }


        [WebMethod(Description =
        @"
        Получение списка всех модераторов системы системы. <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public List<Account> getAllModerators()
        {
            return AccountDAO.getAllByRoleId(3);
        }

        [WebMethod(Description =
        @"
        Получение списка всех администраторов системы. <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public List<Account> getAllAdmins()
        {
            return AccountDAO.getAllByRoleId(4);
        }

        [WebMethod(Description =
        @"
        Добавление нового студента. <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]        
        public int addStudent(Account student)
        {
            student.RoleId = 1;
            student.PasswordMD5 = AccountDAO.getMD5(student.PasswordMD5);
            return AccountDAO.add(student);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавление нового преподавателя. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 если всё прошло удачно <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int addTeacher(Account account)
        {
            if (identification(Authentication.Email, Authentication.Password)
                  &&
                  (isAdmin(Authentication.Email) || isModerator(Authentication.Email)) )
            {
                account.RoleId = 2;
                return AccountDAO.add(account);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавление нового модератора. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: - <br>
        Админ: + <br>
        Возврат: 1 если всё прошло удачно <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int addModerator(Account account)
        {
            if (identification(Authentication.Email, Authentication.Password)
                  && isAdmin(Authentication.Email))
            {
                account.RoleId = 3;
                return AccountDAO.add(account);
            }
            return -2;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавление нового администратора. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: - <br>
        Админ: + <br>
        Возврат: 1 если всё прошло удачно <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int addAdmin(Account account)
        {
            if (identification(Authentication.Email, Authentication.Password)
                  && isAdmin(Authentication.Email))
            {
                account.RoleId = 4;
                return AccountDAO.add(account);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получение идентификатора пользователя. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int getId()
        {
            if (identification(Authentication.Email, Authentication.Password))
            {
                return AccountDAO.getByEmail(Authentication.Email).Id;
            }
            return -2;
        }

        [WebMethod(Description =
        @"
        Получение роли по её идентификаторуи. <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public Role getRoleById(int id)
        {
            return RoleDAO.getRoleByUserId(id);
        }

        //--- CHAT ---


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получить количество новых сообщений для некого конкретного пользователя. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: количество новых сообщений пользователя <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int getCountNewMessages(int userId)
        {
            if (identification(Authentication.Email, Authentication.Password))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                return MessageDAO.getCountNewMessages(userId, account.Id);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получить последнее сообщение пользователя. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public Message getLastMessage(int userId)
        {
            if (identification(Authentication.Email, Authentication.Password))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                return MessageDAO.getLastMessage(userId, account.Id);
            }
            return null;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получить все диалоги пользователя в виде списка аккаунтов пользователей, с котороми открыт диалог. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public List<Account> getDialogs()
        {
            if (identification(Authentication.Email, Authentication.Password))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                return MessageDAO.getDialogs(account.Id);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получить все сообщения диалога текущего пользователя с пользователем под идентификатором userId. <br>
        Имеют доступ: <br>
        Все зарегистрированные пользователи")]
        public List<Message> getDialog(int userId)
        {

            if (identification(Authentication.Email, Authentication.Password))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                return MessageDAO.getDialog(account.Id, userId);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Получить список новых сообщений для текущего пользователя от пользователя под идентификатором userId. <br>
        Имеют доступ: <br>
        Все зарегистрированные пользователи <br>")]
        public List<Message> getNewMessages(int userId)
        {

            if (identification(Authentication.Email, Authentication.Password))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                return MessageDAO.getDialog(userId, account.Id);
            }
            return null;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Функция отправки сообщения от текущего пользователя к пользователю с идентификатором toUserId. <br>
        Имеют доступ: <br>
        Все зарегистрированные пользователи <br>
        Возврат: true - если сообщение было отправлено, иначе false  <br>")]
        public bool sendMessage(int toUserId, string strMess)
        {
            
            if (identification(Authentication.Email, Authentication.Password))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                MessageDAO.add(account.Id, toUserId, strMess);
                return true;
            }
            return false;
        }


        //---UNIVERSITY---

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавить новый университет в систему. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если университет был добавлен <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int addUniversity(University uni)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {

                return UniversityDAO.add(uni);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Обновить данные существующего университета. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если университет был добавлен <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int updateUniversity(University uni)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {

                return UniversityDAO.update(uni);
            }
            return -2;
        }

        [WebMethod(Description =
        @"
        Получить объект университета по его идентификатору. <br>
        Имеют доступ: <br>
        Все пользователи. <br>")]
        public University getUniversityById(int id)
        {
            return UniversityDAO.getById(id);
        }

        [WebMethod(Description =
        @"
        Получить объект университета по его идентификатору без дополнительных тяжеловесных данных. <br>
        Имеют доступ: <br>
        Все пользователи. <br>")]
        public University getUniversityByIdLite(int id)
        {
            return UniversityDAO.getByIdLite(id);
        }


        [WebMethod(Description =
        @"
        Получить список всех университетов. <br>
        Имеют доступ: <br>
        Все пользователи. <br>")]
        public List<University> getAllUniversities()
        {
            return UniversityDAO.getAll();
        }

        [WebMethod(Description =
        @"
        Получит список всех университетов без дополнительных тяжеловесных данных. <br>
        Имеют доступ: <br>
        Все пользователи. <br>")]
        public List<University> getAllUniversitiesLite()
        {
            return UniversityDAO.getAllLite();
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Удалить университет по его идентификатору. <br>
        Имеют доступ: <br>
        Все пользователи. <br>")]
        public int deleteUniversityById(int id)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {

                return UniversityDAO.deleteById(id);
            }
            return -2;
        }


        //---Department---

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавление нового отдела университета. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если отдел был добавлен <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int addDepartment(Department d)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {

                return DepartmentDAO.add(d);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Обновить данные существующего отдела университета. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если отдел был обновлён <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int updateDepartment(Department d)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {

                return DepartmentDAO.update(d);
            }
            return -2;
        }

        [WebMethod(Description =
        @"
        Получение объекта отдела университета по его идентификатору. <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        public Department getDepartmentById(int id)
        {
            return DepartmentDAO.getById(id);
        }

        [WebMethod(Description =
        @"
        Получение объекта отдела университета по его идентификатору без тяжеловесных дополнительных данных. <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        public Department getDepartmentByIdLite(int id)
        {
            return DepartmentDAO.getByIdLite(id);
        }

        [WebMethod(Description =
        @"
        Получение списка всех отделов университета по его идентификатору (университета). <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        ]
        public List<Department> getAllDepartmentsByUniversityId(int id)
        {
            return DepartmentDAO.getAllByUniversityId(id);
        }

        [WebMethod(Description =
        @"
        Получение списка всех отделов университета по его идентификатору (университета) без тяжеловесных дополнительных данных. <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        public List<Department> getAllDepartmentsByUniversityIdLite(int id)
        {
            return DepartmentDAO.getAllByUniversityIdLite(id);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Удалить существующий объект отдела университета. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: - <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если отдел был удалён <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>")]
        public int deleteDepartmentById(int id)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {

                return DepartmentDAO.deleteById(id);
            }
            return -2;
        }

        //--- STUDENT GROUP ---

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавить новую студентческую группу. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если он входит в отдел, в которой добавляется группа) <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если группа была добавлена <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : ошибка, преподаватель не имеет прав добавлять группу в заданный отдел <br>")]
        public int addStudentGroup(StudentGroup s)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email)) {
                    Account acc = AccountDAO.getByEmail(Authentication.Email)
                    if (acc.DepartmentId != s.DepartmentId) return -3;
                }

                return StudentGroupDAO.add(s);
            }
            return -2;
        }


        [WebMethod(Description =
        @"
        Получить объект группы студентов по её идентификатору. <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        public StudentGroup getStudentGroupById(int id)
        {
            return StudentGroupDAO.getById(id);
        }

        [WebMethod (Description =
        @"
        Получить список объектов группы студентов по идентификатору отдела, в котором они находятся. <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        public List<StudentGroup> getAllStudentGroupByDepartmentId(int id)
        {
            return StudentGroupDAO.getAllByDepartmentId(id);
        }

        [WebMethod(Description =
        @"
        Получить объект группы студентов по её идентификатору без дополнительных тяжеловесных данных. <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        public StudentGroup getStudentGroupByIdLite(int id)
        {
            return StudentGroupDAO.getByIdLite(id);
        }

        [WebMethod(Description =
        @"
        Получить список объектов группы студентов без дополнительных тяжеловесных данных по идентификатору отдела, в котором они находятся. <br>
        Имеют доступ: <br>
        Все пользователи системы <br>")]
        public List<StudentGroup> getAllStudentGroupByDepartmentIdLite(int id)
        {
            return StudentGroupDAO.getAllByDepartmentIdLite(id);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавить студента в группу <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если является одним из руководителей группы) <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если студент был добавлен в группу <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : преподватель не имеет полномочий добавлять студента в группу <br>
        -4 : аккаунт под studentId не является студентом <br>")]
        public int addStudentToGroup(int studentId, int groupId)
        {
            if (identification(Authentication.Email, Authentication.Password)
            && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email)) {
                    TeacherToGroup ttg = new TeacherToGroup();
                    ttg.AccountId = AccountDAO.getByEmail(Authentication.Email).Id;
                    ttg.GroupId = groupId;
                    if (!TeacherToGroupDAO.consists(ttg)) {
                        return -3;
                    }
                }
                if (!isStudent(studentId))
                {
                    return -4;
                }
                StudentToGroup stg = new StudentToGroup();
                stg.AccountId = studentId;
                stg.GroupId = groupId;
                return StudentToGroupDAO.add(stg);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Добавить преподавателя в группу в качестве одного из руководителей этой группы. <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если он входит в отдел, в которой добавляется группа) <br>
        Модератор: + <br>
        Админ: + <br>
        Возврат: 1 - если группа была добавлена <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : преподаватель не имеет полномочий добавлять студента в группу <br>
        -4 : аккаунт под teacherId не является преподавателем <br>")]
        public int addTeacherToGroup(int teacherId, int groupId)
        {
            if (identification(Authentication.Email, Authentication.Password)
            && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email))
                {
                    TeacherToGroup ttg = new TeacherToGroup();
                    ttg.AccountId = AccountDAO.getByEmail(Authentication.Email).Id;
                    ttg.GroupId = groupId;
                    if (!TeacherToGroupDAO.consists(ttg))
                    {
                        return -3;
                    }
                }
                if (!isTeacher(teacherId))
                {
                    return -4;
                }
                TeacherToGroup stg = new TeacherToGroup();
                stg.AccountId = teacherId;
                stg.GroupId = groupId;
                return TeacherToGroupDAO.add(stg);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : преподаватель не имеет полномочий добавлять студента в группу <br>
        -4 : аккаунт под studentId не является студентом <br>")]
        public int deleteStudentFromGroup(int studentId, int groupId)
        {
            if (identification(Authentication.Email, Authentication.Password)
            && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email))
                {
                    TeacherToGroup ttg = new TeacherToGroup();
                    ttg.AccountId = AccountDAO.getByEmail(Authentication.Email).Id;
                    ttg.GroupId = groupId;
                    if (!TeacherToGroupDAO.consists(ttg))
                    {
                        return -3;
                    }
                }
                if (!isStudent(studentId))
                {
                    return -4;
                }
                StudentToGroup stg = new StudentToGroup();
                stg.AccountId = studentId;
                stg.GroupId = groupId;
                return StudentToGroupDAO.delete(stg);
            }
            return -2;
        }

        [WebMethod]
        public List<Account> getAllStudentsByGroupId(int id)
        {
            return StudentToGroupDAO.getAllAccountByGroupId(id);
        }

        [WebMethod]
        public List<Account> getAllTeachersByGroupId(int id)
        {
            return TeacherToGroupDAO.getAllAccountByGroupId(id);
        }

        [WebMethod]
        public List<StudentGroup> getAllGroupsByTeacherId(int id)
        {
            return TeacherToGroupDAO.getAllGroupsByUserIdLite(id);
        }

        [WebMethod]
        public List<StudentGroup> getAllGroupsByStudentId(int id)
        {
            return StudentToGroupDAO.getAllGroupsByUserIdLite(id);
        }

        [WebMethod(Description =
        @"
        Проверяет состоит ли студент в группе <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public bool isStudentMemberOfGroup(int idAccount, int idGroup)
        {
            StudentToGroup model = new StudentToGroup();
            model.AccountId = idAccount;
            model.GroupId = idGroup;
            return StudentToGroupDAO.consists(model);
        }

        [WebMethod(Description =
        @"
        Проверяет состоит ли преподаватель в группе <br>
        Имеют доступ: <br>
        Гость: + <br>
        Студент: + <br>
        Преподаватель: + <br>
        Модератор: + <br>
        Админ: + <br>")]
        public bool isTeacherMemberOfGroup(int idAccount, int idGroup)
        {
            TeacherToGroup model = new TeacherToGroup();
            model.AccountId = idAccount;
            model.GroupId = idGroup;
            return TeacherToGroupDAO.consists(model);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Обновление данных группы <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если состоит в группе) <br>
        Модератор: + <br>
        Админ: + <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : преподаватель не имеет полномочий изменять группу <br>
        -4 : аккаунт под studentId не является студентом <br>")]
        public int updateStudentGroup(StudentGroup sg)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email))
                {
                    Account acc = AccountDAO.getByEmail(Authentication.Email);
                    TeacherToGroup model = new TeacherToGroup();
                    model.AccountId = acc.Id;
                    model.GroupId = sg.Id;
                    if (!TeacherToGroupDAO.consists(model)) return -3;
                }
                return StudentGroupDAO.update(sg);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Удаление группы по Id<br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если состоит в группе) <br>
        Модератор: + <br>
        Админ: + <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : преподаватель не имеет полномочий удалять группу <br>
        -4 : аккаунт под studentId не является студентом <br>")]
        public int deleteStudentGroup(int id)
        {
            if (identification(Authentication.Email, Authentication.Password)
                && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                if (isTeacher(Authentication.Email))
                {
                    Account acc = AccountDAO.getByEmail(Authentication.Email);
                    TeacherToGroup model = new TeacherToGroup();
                    model.AccountId = acc.Id;
                    model.GroupId = id;
                    if (!TeacherToGroupDAO.consists(model)) return -3;
                }
                return StudentGroupDAO.deleteById(id);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"
        Выход из состава группы <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: + (если состоит в группе) <br>
        Преподаватель: + (если состоит в группе) <br>
        Модератор: - <br>
        Админ: - <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : вы не состоите в группе <br>
        -4 : аккаунт под studentId не является студентом <br>")]
        public int leaveStudentGroupById(int id)
        {
            if (identification(Authentication.Email, Authentication.Password))
            {
                if (isTeacher(Authentication.Email))
                {
                    Account acc = AccountDAO.getByEmail(Authentication.Email);
                    TeacherToGroup model = new TeacherToGroup();
                    model.AccountId = acc.Id;
                    model.GroupId = id;
                    if (!TeacherToGroupDAO.consists(model)) return -3;
                    return TeacherToGroupDAO.delete(model);
                } else if (isStudent(Authentication.Email))
                {
                    Account acc = AccountDAO.getByEmail(Authentication.Email);
                    StudentToGroup model = new StudentToGroup();
                    model.AccountId = acc.Id;
                    model.GroupId = id;
                    if (!StudentToGroupDAO.consists(model)) return -3;
                    return StudentToGroupDAO.delete(model);
                } else
                {
                    return -2;
                }
            }
            return -2;
        }

        //--- TUTORIAL ---

        [WebMethod(Description =
        @"
        Выдаёт все методические материалы группы по id <br>
        Аутентификация и авторизация не трубуется")]
        public List<Tutorial> getAllTutorialByStudentGroupId(int id)
        {
            return TutorialDAO.getAllByStudentGroupId(id);
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"Добавление методических материалов <br>
        Требуется аутентификация и авторизация <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если состоит в группе) <br>
        Модератор: + <br>
        Админ: + <br>
        Коды ошибок <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : преподватель не имеет полномочий добавлять методические материалы в группу <br>")]
        public int addTutorial(Tutorial model)
        {
            if (identification(Authentication.Email, Authentication.Password)
            && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                if (account.RoleId == RoleDAO.getByName("teacher").Id)
                {
                    TeacherToGroup ttg = new TeacherToGroup();
                    ttg.AccountId = account.Id;
                    ttg.GroupId = model.StudentGroupId;
                    if (!TeacherToGroupDAO.consists(ttg))
                    {
                        return -3;
                    }
                }
                model.AuthorId = account.Id;
                return TutorialDAO.add(model);
            }
            return -2;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description =
        @"Удаление методических материалов <br>
        Имеют доступ: <br>
        Гость: - <br>
        Студент: - <br>
        Преподаватель: + (если состоит в группе) <br>
        Модератор: + <br>
        Админ: + <br>
        Коды ошибок <br>
        Требуется аутентификация и авторизация <br>
        -1 : ошибка БД <br>
        -2 : ошибка аутентификации или авторизации <br>
        -3 : преподватель не имеет полномочий удалять методические материалы в группе <br>
        -4 : методический материал по указанному id не найден")]
        public int deleteTutorial(int id)
        {
            if (identification(Authentication.Email, Authentication.Password)
            && (isTeacher(Authentication.Email) || isModerator(Authentication.Email) || isAdmin(Authentication.Email)))
            {
                Account account = AccountDAO.getByEmail(Authentication.Email);
                Tutorial tutorial = TutorialDAO.getById(id);
                if (tutorial == null) return -4;
                if (account.RoleId == RoleDAO.getByName("teacher").Id)
                {
                    TeacherToGroup ttg = new TeacherToGroup();
                    ttg.AccountId = account.Id;
                    ttg.GroupId = tutorial.StudentGroupId;
                    if (!TeacherToGroupDAO.consists(ttg))
                    {
                        return -3;
                    }
                }
                return TutorialDAO.deleteById(id);
            }
            return -2;
        }



        [WebMethod(Description =
        @"Метод для проверки соединения с веб-сервисом <br>
        возвращает полученную в параметре строку + test_123")]
        public string test(string test) {
        	return test + " test_123";
        }

        private bool identification(string email, string password)
        {
            Account account = AccountDAO.getByEmail(email);
            if (account == null) return false; //username not found
            string passwordMd5 = AccountDAO.getMD5(password);
            return passwordMd5 == account.PasswordMD5;
        }

        private bool isStudent(string email)
        {
            return AccountDAO.getByEmail(email).RoleId == RoleDAO.getByName("student").Id;
        }

        private bool isTeacher(string email)
        {
            return AccountDAO.getByEmail(email).RoleId == RoleDAO.getByName("teacher").Id;
        }

        private bool isModerator(string email)
        {
            return AccountDAO.getByEmail(email).RoleId == RoleDAO.getByName("moderator").Id;
        }

        private bool isAdmin(string email)
        {
            return AccountDAO.getByEmail(email).RoleId == RoleDAO.getByName("admin").Id;
        }

        private bool isStudent(int id)
        {
            return AccountDAO.getById(id).RoleId == RoleDAO.getByName("student").Id;
        }

        private bool isTeacher(int id)
        {
            return AccountDAO.getById(id).RoleId == RoleDAO.getByName("teacher").Id;
        }


    }

}
