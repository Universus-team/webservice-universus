using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    [Serializable]
    public class University
    {
        public University(string name, string address, bool hasStateAccreditation, string webSite)
        {
            Name = name;
            Address = address;
            HasStateAccreditation = hasStateAccreditation;
            WebSite = webSite;
        }

        public University()
        {
        }

        public University(int id, string name, string address, bool hasStateAccreditation, string webSite)
        {
            Id = id;
            Name = name;
            Address = address;
            HasStateAccreditation = hasStateAccreditation;
            WebSite = webSite;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool HasStateAccreditation { get; set; }
        public string WebSite { get; set; }


    }

    [Serializable]
    public class Department
    {
        public Department(string name, string phoneNumber, string email, int universityId)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            UniversityId = universityId;
        }

        public Department()
        {
        }

        public Department(int id, string name, string phoneNumber, string email, int universityId)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            UniversityId = universityId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int UniversityId { get; set; }
    }

    [Serializable]
    public class Speciality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpecialityCode { get; set; }
        public string Description { get; set; }

    }

    [Serializable]
    public class StudentGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ManagerID { get; set; }
        public int DepartmentID { get; set; }
    }

    [Serializable]
    public class Teacher : Account
    {
        public Teacher(Account account, int departmentId)
        {
            Id = account.Id;
            Username = account.Username;
            PasswordMD5 = account.PasswordMD5;
            Name = account.Name;
            Surname = account.Surname;
            Patronymic = account.Patronymic;
            Phone = account.Phone;
            Email = account.Email;
            RoleId = account.RoleId;
            DepartmentId = departmentId;
        }
        int DepartmentId { get; set; }
    }

    [Serializable]
    public class Student : Account
    {

        public Student(Account account, int groupId)
        {
            Id = account.Id;
            Username = account.Username;
            PasswordMD5 = account.PasswordMD5;
            Name = account.Name;
            Surname = account.Surname;
            Patronymic = account.Patronymic;
            Phone = account.Phone;
            Email = account.Email;
            RoleId = account.RoleId;
            GroupId = groupId;
        }

        public Student()
        {
        }

        public int GroupId { get; set; }


    }

    [Serializable]
    public class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string MessageContent { get; set; }
        public DateTime DateOfMessage { get; set; }
        public bool ItRead { get; set; }
    }

    [Serializable]
    public class Account
    {
        public Account(string username, string passwordMD5, string name, string surname, string patronymic, int roleId, string email, string phone)
        {
            Username = username;
            PasswordMD5 = passwordMD5;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            RoleId = roleId;
            Email = email;
            Phone = phone;
        }

        public Account() { }

        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordMD5 { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }

    [Serializable]
    public class Role
    {
        public readonly static string ADMIN = "admin";
        public readonly static string MODERATOR = "moderator";
        public readonly static string TEACHER = "teacher";
        public readonly static string STUDENT = "student";
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Exam
    {
        public Exam(int id, string title, string description, string author, int countOfQuestion, string content)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
            CountOfQuestion = countOfQuestion;
            Content = content;
        }

        public Exam()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int CountOfQuestion { get; set; }
        public string Content { get; set; }


    }

    [Serializable]
    public class ExamHistory
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public int StatusId { get; set; }
        public float Result { get; set; }
        public float PassingScore { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DateOfTest { get; set; }

    }

    [Serializable]
    public class ExamStatus
    {

        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Subject
    {

        public Subject()
        {

        }
        public Subject(string name, int teacherId, DateTime createdDate)
        {
            Name = name;
            TeacherId = teacherId;
            CreatedDate = createdDate;
        }

        public Subject(int id, string name, int teacherId, DateTime createdDate)
        {
            Id = id;
            Name = name;
            TeacherId = teacherId;
            CreatedDate = createdDate;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
