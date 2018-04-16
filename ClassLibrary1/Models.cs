using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DatabaseLib
{
    [Serializable]
    public class University
    {


        public University()
        {
        }


        public University(string full_name, string short_name, string description, string logo_url,
            string address, string webSite)
        {
            FullName = full_name;
            ShortName = short_name;
            Description = description;
            LogoURL = logo_url;
            Address = address;
            WebSite = webSite;
        }


        public University(int id, string full_name, string short_name, string description, string logo_url,
            string address, string webSite)
        {
            Id = id;
            FullName = full_name;
            ShortName = short_name;
            Description = description;
            LogoURL = logo_url;
            Address = address;
            WebSite = webSite;
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string LogoURL { get; set; }
        public string Address { get; set; }
        public string WebSite { get; set; }

    }

    [Serializable]
    public class Department
    {
        public Department(string name, string description, string dean_name, int universityId)
        {
            Name = name;
            Description = description;
            Description = dean_name;
            UniversityId = universityId;
        }

        public Department()
        {
        }

        public Department(int id, string name, string description, string dean_name, int universityId)
        {
            Id = id;
            Name = name;
            DeanName = dean_name;
            Description = description;
            UniversityId = universityId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeanName { get; set; }
        public int UniversityId { get; set; }
    }

    [Serializable]
    public class StudentGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DepartmentID { get; set; }
        public string Description { get; set; }
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
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDay { get; set; }
        public string PhotoURL { get; set; }
        public string PasswordMD5 { get; set; }


        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public int RoleId { get; set; }
        public int DepartmentId { get; set; }

    }

    [Serializable]
    public class StudentToGroup
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int GroupId { get; set; }
    }

    [Serializable]
    public class TeacherToGroup
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int GroupId { get; set; }
    }

    [Serializable]
    public class Role
    {
        public readonly static string ADMIN = "admin";
        public readonly static string MODERATOR = "moderator";
        public readonly static string TEACHER = "teacher";
        public readonly static string STUDENT = "student";

        public Role(string name)
        {
            Name = name;
        }

        public Role()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Exam
    {
        public Exam(int id, string title, string description, int authorId, int countOfQuestion, string content)
        {
            Id = id;
            Title = title;
            Description = description;
            AuthorId = authorId;
            CountOfQuestion = countOfQuestion;
            Content = content;
        }

        public Exam()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int CountOfQuestion { get; set; }
        public string Content { get; set; }

        public void clearAnswer()
        {
            List<Question> questions = new JavaScriptSerializer().Deserialize<List<Question>>(Content);
            foreach (Question question in questions)
            {
                foreach (var answer in question.answers)
                {
                    answer.correct = "0";
                }
            }
            Content = new JavaScriptSerializer().Serialize(questions);
        }

        public float compare(Exam exam)
        {
            if (exam.Id != Id) return -6;
            List<Question> questions = new JavaScriptSerializer().Deserialize<List<Question>>(exam.Content);
            List<Question> questionsSource = new JavaScriptSerializer().Deserialize<List<Question>>(Content);
            questions.Sort((ques1, ques2) => ques1.question.CompareTo(ques2.question));
            questionsSource.Sort((ques1, ques2) => ques1.question.CompareTo(ques2.question));
            int correct = 0;
            for (int i = 0; i < CountOfQuestion; i++)
            {
                questions[i].answers.Sort((ans1, ans2) => ans1.content.CompareTo(ans2.content));
                questionsSource[i].answers.Sort((ans1, ans2) => ans1.content.CompareTo(ans2.content));
                int j;
                for (j = 0; j < questionsSource[i].answers.Count; j++)
                {
                    if (questions[i].answers[j].correct.CompareTo(questionsSource[i].answers[j].correct) != 0)
                    {
                        break;
                    }
                }
                
                if (j == questionsSource[i].answers.Count) correct ++;
            }
            return (((float) correct) / ((float) CountOfQuestion));

        }

        [Serializable]
        class Question
        {
            public string question { get; set; }
            public List<Answer> answers { get; set; }
        }

        [Serializable]
        class Answer
        {
            public string content { get; set; }
            public string correct { get; set; }
        }
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
        public Exam Exam { get; set; }

    }

    [Serializable]
    public class ExamStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Tutorial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int StudentGroupId { get; set; }
    }

}
