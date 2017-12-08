using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    [Serializable]
    public class Unisersity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool HasStateAccreditation { get; set; }
    }

    [Serializable]
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
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
        public int LeaderID { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ManagerID { get; set; }
    }

    [Serializable]
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }

    [Serializable]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
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
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordMD5 { get; set; }
        public int RoleId { get; set; }
    }

    [Serializable]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


}
