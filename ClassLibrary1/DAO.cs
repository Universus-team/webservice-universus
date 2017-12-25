using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace DatabaseLib
{

    public abstract class DAO
    {

        protected static string connectionString = "server=127.0.0.1;" +
            "uid=root;" +
            "pwd=3616;" +
            "database=universus;";

        public DAO()
        {

        }


        protected static MySqlConnection getConnection()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = connectionString;
            return conn;
        }

    }

    public class StudentDAO : DAO
    {


        public static List<Student> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT group_id account_id 
                                FROM student_to_group, role, account
                                WHERE 
                                student_to_group.account_id = account.id 
                                AND account.role_id = role.id 
                                AND role.name = 'student'";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Student> students = new List<Student>();
                while (reader.Read())
                {                 
                    int group_id = reader.GetInt32(0);
                    int account_id = reader.GetInt32(1);
                    Student student = new Student(AccountDAO.getById(account_id), group_id);
                    students.Add(student);
                }
                return students;

            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static int update(Student s)
        {
            if (s == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE student_to_group 
                                SET group_id = @group
                                WHERE account_id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", s.Id);
                cmd.Parameters.AddWithValue("@group", s.GroupId);
                AccountDAO.update((Account)s);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static Student getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT group_id account_id 
                                FROM student_to_group, role, account
                                WHERE 
                                student_to_group.account_id = account.id 
                                AND account.id = @id
                                AND account.role_id = role.id 
                                AND role.name = 'student'";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int group_id = reader.GetInt32(0);
                    int account_id = reader.GetInt32(1);
                    Student student = new Student(AccountDAO.getById(account_id), group_id);
                    return student;
                }
                

            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM student_to_group WHERE account_id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                AccountDAO.deleteById(id);
                return cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static int add(Student st)
        {
            if (st == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO student_to_group 
                (group_id, account_id)
                VALUES( @group, @id);";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                int id = AccountDAO.add((Account)st);
                cmd.Parameters.AddWithValue("@group", st.GroupId);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
            catch (MySqlException ex)
            {
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -2;
        }
    }


    public class AccountDAO : DAO
    {
        public static Account getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, username, password_md5, role_id, name, surname, patronymic, email, phone 
                                FROM account
                                WHERE id = @Id and id <> 0";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                Account account = new Account();
                if (reader.Read())
                {
                    account.Id = reader.GetInt32(0);
                    account.Username = reader.GetString(1);
                    account.PasswordMD5 = reader.GetString(2);
                    account.RoleId = reader.GetInt32(3);
                    account.Name = reader.GetString(4);
                    account.Surname = reader.GetString(5);
                    account.Patronymic = reader.GetString(6);
                    account.Email = reader.GetString(7);
                    account.Email = reader.GetString(8);

                }
                return account;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static List<Account> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, username, password_md5, role_id, name, surname, patronymic, email, phone 
                                FROM account
                                WHERE id <> 0";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Account> accounts = new List<Account>();
                while (reader.Read())
                {
                    Account account = new Account();
                    account.Id = reader.GetInt32(0);
                    account.Username = reader.GetString(1);
                    account.PasswordMD5 = reader.GetString(2);
                    account.RoleId = reader.GetInt32(3);
                    account.Name = reader.GetString(4);
                    account.Surname = reader.GetString(5);
                    account.Patronymic = reader.GetString(6);
                    account.Email = reader.GetString(7);
                    account.Email = reader.GetString(8);
                    accounts.Add(account);

                }
                return accounts;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static Account getByUsername(string username)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, username, password_md5, role_id, name, surname, patronymic, email, phone 
                                FROM account
                                WHERE username = @uname and id <> 0";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@uname", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                Account account = new Account();
                if (reader.Read())
                {
                    account.Id = reader.GetInt32(0);
                    account.Username = reader.GetString(1);
                    account.PasswordMD5 = reader.GetString(2);
                    account.RoleId = reader.GetInt32(3);
                    account.Name = reader.GetString(4);
                    account.Surname = reader.GetString(5);
                    account.Patronymic = reader.GetString(6);
                    account.Email = reader.GetString(7);
                    account.Email = reader.GetString(8);

                }
                return account;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static int add(Account account)
        {
            if (account == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO account
                                (username, password_md5, role_id, name, surname, patronymic, email, phone)
                                VALUES(@uname, @pwd, @role, @name, @sname, @patr, @email, @phone)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@uname", account.Username);
                cmd.Parameters.AddWithValue("@pwd", account.PasswordMD5);
                cmd.Parameters.AddWithValue("@role", account.RoleId);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@sname", account.Surname);
                cmd.Parameters.AddWithValue("@patr", account.Patronymic);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@phone", account.Phone);
                cmd.ExecuteNonQuery();
                int id = (int)cmd.LastInsertedId;
                return id;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static int update(Account account)
        {
            if (account == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE account 
                                SET 
                                username=@uname,
                                password_md5 = @pwd.
                                role_id = @role.
                                name=@name,
                                surname=@sname,
                                patronymic = @patr,
                                email = @email,
                                phone=@phone, 
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", account.Id);
                cmd.Parameters.AddWithValue("@uname", account.Username);
                cmd.Parameters.AddWithValue("@pwd", account.PasswordMD5);
                cmd.Parameters.AddWithValue("@role", account.RoleId);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@sname", account.Surname);
                cmd.Parameters.AddWithValue("@patr", account.Patronymic);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@phone", account.Phone);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = "DELETE FROM account WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                //AccountDAO.de
                return cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static string getMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public class MessageDAO : DAO
    {
        public static int add(int fromUserId, int toUserName, string message)
        {
            if (message == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO message (from_user_id, to_user_id, message_content, date_of_message)
                VALUES (@from, @to, @msg, @date)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@from", fromUserId);
                cmd.Parameters.AddWithValue("@to", toUserName);
                cmd.Parameters.AddWithValue("@msg", message);
                cmd.Parameters.AddWithValue("@date", DateTime.UtcNow);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static Message getMessageFrom(int userId)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, from_user_id, to_user_id, message_content, date_of_message, it_read
                                FROM Message WHERE from_user_id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", userId);
                MySqlDataReader reader = cmd.ExecuteReader();
                Message message = new Message();
                if (reader.Read())
                {
                    message.Id = reader.GetInt32(0);
                    message.FromUserId = reader.GetInt32(1);
                    message.ToUserId = reader.GetInt32(2);
                    message.MessageContent = reader.GetString(3);
                    message.DateOfMessage = reader.GetDateTime(4);
                    message.ItRead = reader.GetBoolean(5);
                }
                return message;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static List<string> getDialogs(int userId)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT account.username
                                FROM Message, account 
                                WHERE message.to_user_id = @id
                                and account.id = message.from_user_id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", userId);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<string> users = new List<string>();
                while (reader.Read())
                {
                    users.Add(reader.GetString(0));
                }
                return users;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static List<Message> getDialog(int to_id, int from_id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, from_user_id, to_user_id, message_content, date_of_message, it_read
                                FROM Message 
                                WHERE (to_user_id = @id1 and from_user_id = @id2) or
                               (to_user_id = @id2 and from_user_id = @id1)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id1", to_id);
                cmd.Parameters.AddWithValue("@id2", from_id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Message> messages = new List<Message>();
                while (reader.Read())
                {
                    Message message = new Message();
                    message.Id = reader.GetInt32(0);
                    message.FromUserId = reader.GetInt32(1);
                    message.ToUserId = reader.GetInt32(2);
                    message.MessageContent = reader.GetString(3);
                    message.DateOfMessage = reader.GetDateTime(4);
                    message.ItRead = true;
                    MessageDAO.update(message.Id, message);
                    messages.Add(message);
                }
                
                return messages;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static Message getMessageTo(int userId)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, from_user_id, to_user_id, message_content, date_of_message, it_read
                                FROM Message WHERE to_user_id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", userId);
                MySqlDataReader reader = cmd.ExecuteReader();
                Message message = new Message();
                if (reader.Read())
                {
                    message.Id = reader.GetInt32(0);
                    message.FromUserId = reader.GetInt32(1);
                    message.ToUserId = reader.GetInt32(2);
                    message.MessageContent = reader.GetString(3);
                    message.DateOfMessage = reader.GetDateTime(4);
                    message.ItRead = reader.GetBoolean(5);
                }
                return message;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static Message getById(int Id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, from_user_id, to_user_id, message_content, date_of_message, it_read
                                FROM Message WHERE id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", Id);
                MySqlDataReader reader = cmd.ExecuteReader();
                Message message = new Message();
                if (reader.Read())
                {
                    message.Id = reader.GetInt32(0);
                    message.FromUserId = reader.GetInt32(1);
                    message.ToUserId = reader.GetInt32(2);
                    message.MessageContent = reader.GetString(3);
                    message.DateOfMessage = reader.GetDateTime(4);
                    message.ItRead = reader.GetBoolean(5);
                }
                return message;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static bool checkNewMessage(int toUserId, int fromUserId)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT COUNT(id)
                                FROM Message WHERE from_user_id = @from and to_user_id = @to and it_read = 0";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@from", fromUserId);
                cmd.Parameters.AddWithValue("@to", toUserId);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int count = reader.GetInt32(0);
                    return (count == 0) ? false : true;
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return false;
        }

        public static int update(int messageId, Message m)
        {
            if (m == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE message 
                                SET from_user_id=@from, to_user_id=@to, 
                                    message_content = @msg, date_of_message=@date, it_read = @read
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", messageId);
                cmd.Parameters.AddWithValue("@from", m.FromUserId);
                cmd.Parameters.AddWithValue("@to", m.ToUserId);
                cmd.Parameters.AddWithValue("@msg", m.MessageContent);
                cmd.Parameters.AddWithValue("@date", m.DateOfMessage);
                cmd.Parameters.AddWithValue("@read", m.ItRead);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }
    }

    public class RoleDAO : DAO
    {
        public static int add(string name)
        {
            if (name == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO role (name)
                VALUES(@name)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", name);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static Role getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = "SELECT id, name FROM role WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                Role role = new Role();
                if (reader.Read())
                {
                    role.Id = reader.GetInt32(0);
                    role.Name = reader.GetString(1);
                }
                return role;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static Role getRoleByUserId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT role.id, role.name FROM role, account 
                WHERE account.id = @id and account.role_id = role.id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                Role role = new Role();
                if (reader.Read())
                {
                    role.Id = reader.GetInt32(0);
                    role.Name = reader.GetString(1);
                }
                return role;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static Role getByName(string name)
        {
            MySqlConnection conn = null;
            string sqlQuery = "SELECT id, name FROM role WHERE name = @name";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", name);
                MySqlDataReader reader = cmd.ExecuteReader();
                Role role = new Role();
                if (reader.Read())
                {
                    role.Id = reader.GetInt32(0);
                    role.Name = reader.GetString(1);
                }
                return role;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }
    }

    public class SpecialityDAO : DAO
    {
        public static int add(string name, string specialityCode, string description)
        {
            if (name == null && specialityCode == null && description == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO speciality (name, speciality_code, description)
                VALUES(@name, @code, @descr)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@code", specialityCode);
                cmd.Parameters.AddWithValue("@descr", description);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static Speciality getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = "SELECT id, name, speciality_code, description FROM speciality WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                Speciality speciality = new Speciality();
                if (reader.Read())
                {
                    speciality.Id = reader.GetInt32(0);
                    speciality.Name = reader.GetString(1);
                    speciality.SpecialityCode = reader.GetString(2);
                    speciality.Description = reader.GetString(3);
                }
                return speciality;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static Speciality getBySpecialityCode(string specialityCode)
        {
            MySqlConnection conn = null;
            string sqlQuery = "SELECT id, name, speciality_code, description FROM speciality WHERE speciality_code = @code";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@code", specialityCode);
                MySqlDataReader reader = cmd.ExecuteReader();
                Speciality speciality = new Speciality();
                if (reader.Read())
                {
                    speciality.Id = reader.GetInt32(0);
                    speciality.Name = reader.GetString(1);
                    speciality.SpecialityCode = reader.GetString(2);
                    speciality.Description = reader.GetString(3);
                }
                return speciality;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }
    }


    public class UniversityDAO : DAO
    {
        public static int add(University uni)
        {
            if (uni == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO university (name, address, has_state_accreditation, web_site)
                VALUES(@name, @address, @acc, @web)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", uni.Name);
                cmd.Parameters.AddWithValue("@address", uni.Address);
                cmd.Parameters.AddWithValue("@acc", uni.HasStateAccreditation);
                cmd.Parameters.AddWithValue("@web", uni.WebSite);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static List<University> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, address, has_state_accreditation, web_site FROM university WHERE id <> 0";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<University> universities = new List<University>();
                while (reader.Read())
                {
                    University uni = new University(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetBoolean(3),
                        reader.GetString(4)
                        );

                    universities.Add(uni);
                }
                return universities;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static University getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, address, has_state_accreditation FROM university WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                University unisersity = new University();
                if (reader.Read())
                {
                    unisersity.Id = reader.GetInt32(0);
                    unisersity.Name = reader.GetString(1);
                    unisersity.Address = reader.GetString(2);
                    unisersity.HasStateAccreditation = reader.GetBoolean(3);
                }
                return unisersity;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }
    }

    public class DepartmentDAO : DAO
    {
        public static int add(Department d)
        {
            if (d == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO department (name, phone_number, email, university_id)
                VALUES(@name, @phone, @email, @uniid)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", d.Name);
                cmd.Parameters.AddWithValue("@phone", d.PhoneNumber);
                cmd.Parameters.AddWithValue("@email", d.Email);
                cmd.Parameters.AddWithValue("@uniid", d.UniversityId);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static Department getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, phone_number, email FROM department WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                Department department = new Department();
                if (reader.Read())
                {
                    department.Id = reader.GetInt32(0);
                    department.Name = reader.GetString(1);
                    department.PhoneNumber = reader.GetString(2);
                    department.Email = reader.GetString(3);
                }
                return department;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static List<Department> getAllByUniversityId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, phone_number, email FROM department WHERE university_id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Department> departments = new List<Department>();
                while (reader.Read())
                {
                    Department department = new Department();
                    department.Id = reader.GetInt32(0);
                    department.Name = reader.GetString(1);
                    department.PhoneNumber = reader.GetString(2);
                    department.Email = reader.GetString(3);
                    departments.Add(department);
                }
                return departments;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }
    }

    public class StudentGroupDAO : DAO
    {
        public static int add(StudentGroup s)
        {
            if (s == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO student_group (name, created_date, email,  manager_id, department_id)
                VALUES(@name, @date, @email, @manager, @department)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", s.Name);
                cmd.Parameters.AddWithValue("@date", s.CreatedDate);
                cmd.Parameters.AddWithValue("@email", s.Email);
                cmd.Parameters.AddWithValue("@manager", s.ManagerID);
                cmd.Parameters.AddWithValue("@department", s.DepartmentID);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static void addStudent(int studentId, int groupId)
        {

        }

        public static StudentGroup getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, created_date, email, manager_id, department_id
                                FROM student_group WHERE id = @Id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                StudentGroup studentGroup = new StudentGroup();
                if (reader.Read())
                {
                    studentGroup.Id = reader.GetInt32(0);
                    studentGroup.Name = reader.GetString(1);
                    studentGroup.CreatedDate = reader.GetDateTime(2);
                    studentGroup.Email = reader.GetString(3);
                    studentGroup.ManagerID = reader.GetInt32(4);
                    studentGroup.DepartmentID = reader.GetInt32(5);
                }
                return studentGroup;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static List<StudentGroup> getAllByManagerId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, created_date, email, manager_id, department_id
                                FROM student_group WHERE manager_id = @Id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentGroup> groups = new List<StudentGroup>();
                while (reader.Read())
                {
                    StudentGroup studentGroup = new StudentGroup();
                    studentGroup.Id = reader.GetInt32(0);
                    studentGroup.Name = reader.GetString(1);
                    studentGroup.CreatedDate = reader.GetDateTime(2);
                    studentGroup.Email = reader.GetString(3);
                    studentGroup.ManagerID = reader.GetInt32(4);
                    studentGroup.DepartmentID = reader.GetInt32(5);
                    groups.Add(studentGroup);
                }
                return groups;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static int deleteAll()
        {
            MySqlConnection conn = null;
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("delete_all_student_groups", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                return cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }
    }

    public class ExamDAO : DAO
    {
        public static int add(Exam e)
        {
            if (e == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO exam (title, description, author, count_of_question, content)
                VALUES(@title, @descr, @author, @question, @content)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@title", e.Title);
                cmd.Parameters.AddWithValue("@descr", e.Description);
                cmd.Parameters.AddWithValue("@author",e.Author);
                cmd.Parameters.AddWithValue("@question", e.CountOfQuestion);
                cmd.Parameters.AddWithValue("@content", e.Content);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static List<Exam> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, title, description, author, count_of_question, content 
                            FROM exam";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Exam> exams = new List<Exam>();
                while (reader.Read())
                {
                    Exam exam = new Exam(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetInt32(4),
                        reader.GetString(5)
                        );

                    exams.Add(exam);
                }
                return exams;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static Exam getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, title, description, author, count_of_question, content 
                            FROM exam where id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Exam(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetInt32(4),
                        reader.GetString(5)
                        );
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }



    }

    public class ExamHistoryDAO : DAO
    {
        public static int add(ExamHistory history)
        {
            if (history == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO exam_history (exam_id, teacher_id, deadline, result, status_id, student_id, passing_score, date_of_test)
                VALUES(@exam, @teacher, @deadline, @result, @status, @student, @pass, @dateOfTest)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@exam", history.ExamId);
                cmd.Parameters.AddWithValue("@teacher", history.TeacherId);
                cmd.Parameters.AddWithValue("@student", history.StudentId);
                cmd.Parameters.AddWithValue("@status", history.StatusId);
                cmd.Parameters.AddWithValue("@pass", history.PassingScore);
                cmd.Parameters.AddWithValue("@result", history.Result);
                cmd.Parameters.AddWithValue("@dateOfTest", history.DateOfTest);
                cmd.Parameters.AddWithValue("@deadline", history.Deadline);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static List<ExamHistory> getAllByStudentId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, exam_id, teacher_id, deadline, result, status_id, student_id, passing_score, date_of_test
                                FROM exam_history WHERE student_id = @Id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<ExamHistory> history = new List<ExamHistory>();
                while (reader.Read())
                {
                    ExamHistory examHistory = new ExamHistory();
                    examHistory.Id = reader.GetInt32(0);
                    examHistory.ExamId = reader.GetInt32(1);
                    examHistory.TeacherId = reader.GetInt32(2);
                    examHistory.Deadline = reader.GetDateTime(3);
                    examHistory.Result = reader.GetFloat(4);
                    examHistory.StatusId = reader.GetInt32(5);
                    examHistory.StudentId = reader.GetInt32(6);
                    examHistory.PassingScore = reader.GetFloat(7);
                    examHistory.DateOfTest = reader.GetDateTime(8);

                    history.Add(examHistory);
                }
                return history;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static ExamHistory getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, exam_id, teacher_id, deadline, result, status_id, student_id, passing_score, date_of_test
                                FROM exam_history WHERE student_id = @Id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ExamHistory examHistory = new ExamHistory();
                    examHistory.Id = reader.GetInt32(0);
                    examHistory.ExamId = reader.GetInt32(1);
                    examHistory.TeacherId = reader.GetInt32(2);
                    examHistory.Deadline = reader.GetDateTime(3);
                    examHistory.Result = reader.GetFloat(4);
                    examHistory.StatusId = reader.GetInt32(5);
                    examHistory.StudentId = reader.GetInt32(6);
                    examHistory.PassingScore = reader.GetFloat(7);
                    examHistory.DateOfTest = reader.GetDateTime(8);
                    return examHistory;
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static int update(ExamHistory history)
        {
            if (history == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE exam_history 
                                SET 
                                exam_id=@exam,
                                teacher_id = @teacher,
                                student_id = @student,
                                status_id = @status,   
                                result = @result,
                                passing_score = @pass,
                                deadline = @deadline,
                                date_of_test = @dateOfTest 
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", history.Id);
                cmd.Parameters.AddWithValue("@exam", history.ExamId);
                cmd.Parameters.AddWithValue("@teacher", history.TeacherId);
                cmd.Parameters.AddWithValue("@student", history.StudentId);
                cmd.Parameters.AddWithValue("@status", history.StatusId);
                cmd.Parameters.AddWithValue("@pass", history.PassingScore);
                cmd.Parameters.AddWithValue("@result", history.Result);
                cmd.Parameters.AddWithValue("@dateOfTest", history.DateOfTest);
                cmd.Parameters.AddWithValue("@deadline", history.Deadline);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

    }

    public class ExamStatusDAO : DAO
    {
        public static ExamStatus getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = "SELECT id, name FROM exam_status WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                ExamStatus status = new ExamStatus();
                if (reader.Read())
                {
                    status.Id = reader.GetInt32(0);
                    status.Name = reader.GetString(1);
                }
                return status;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

    }

    public class SubjectDAO : DAO
    {
        public static int add(Subject s)
        {
            if (s == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO subject (name, teacher_id, created_date)
                VALUES(@name, @teacher_id, @created_date)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", s.Name);
                cmd.Parameters.AddWithValue("@teacher_id", s.TeacherId);
                cmd.Parameters.AddWithValue("@created_date", s.CreatedDate);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static Subject getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, teacher_id, created_date
                                FROM subject WHERE id = @Id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Subject(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetDateTime(3));
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static List<Subject> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, name, teacher_id, created_date
                                FROM subject";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Subject> subjects = new List<Subject>();
                while (reader.Read())
                {
                    subjects.Add( new Subject(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetDateTime(3)));
                }
                return subjects;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return null;
        }

        public static int update(Subject subject)
        {
            if (subject == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE subject 
                                SET 
                                name=@name,
                                created_date = @date,
                                teacher_id = @teacher
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", subject.Id);
                cmd.Parameters.AddWithValue("@name", subject.Name);
                cmd.Parameters.AddWithValue("@teacher", subject.TeacherId);
                cmd.Parameters.AddWithValue("@date", subject.CreatedDate);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM subject 
                                WHERE id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return -1;
        }
    }
}
