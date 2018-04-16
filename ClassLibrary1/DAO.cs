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
            "uid=u0467_mikhail;" +
            "pwd=cgPg705?;" +
            "database=u0467439_universus;" +
            "CharSet=utf8;";


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


    public class AccountDAO : DAO
    {
        public static Account getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id,  password_md5, role_id, name, surname, patronymic, email, phone, photo_url, birth_day, department_id, address 
                                FROM account
                                WHERE id = @Id";
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
                    account.PasswordMD5 = reader.GetString(1);
                    account.RoleId = reader.GetInt32(2);
                    account.Name = reader.GetString(3);
                    account.Surname = reader.GetString(4);
                    account.Patronymic = reader.GetString(5);
                    account.Email = reader.GetString(6);
                    account.Phone = reader.GetString(7);
                    account.PhotoURL = reader.GetString(8);
                    account.BirthDay = reader.GetDateTime(9);
                    account.DepartmentId = reader.GetInt32(10);
                    account.Address = reader.GetString(11);

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

        public static Account getByIdWithoutPassword(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id,  address, role_id, name, surname, patronymic, email, phone, photo_url, birth_day, department_id 
                                FROM account
                                WHERE id = @Id";
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
                    account.Address = reader.GetString(1);
                    account.RoleId = reader.GetInt32(2);
                    account.Name = reader.GetString(3);
                    account.Surname = reader.GetString(4);
                    account.Patronymic = reader.GetString(5);
                    account.Email = reader.GetString(6);
                    account.Phone = reader.GetString(7);
                    account.PhotoURL = reader.GetString(8);
                    account.BirthDay = reader.GetDateTime(9);
                    account.DepartmentId = reader.GetInt32(10);
                    account.PasswordMD5 = "";


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
                                id,  password_md5, role_id, name, surname, patronymic, email, phone, photo_url, birth_day, department_id, address 
                                FROM account";
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
                    account.PasswordMD5 = ""; // hide password
                    account.RoleId = reader.GetInt32(2);
                    account.Name = reader.GetString(3);
                    account.Surname = reader.GetString(4);
                    account.Patronymic = reader.GetString(5);
                    account.Email = reader.GetString(6);
                    account.Phone = reader.GetString(7);
                    account.PhotoURL = reader.GetString(8);
                    account.BirthDay = reader.GetDateTime(9);
                    account.DepartmentId = reader.GetInt32(10);
                    account.Address = reader.GetString(11);


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

        public static List<Account> getAllByRoleId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id,  password_md5, role_id, name, surname, patronymic, email, phone, photo_url, birth_day, department_id, address 
                                FROM account
                                where role_id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Account> accounts = new List<Account>();
                while (reader.Read())
                {
                    Account account = new Account();
                    account.Id = reader.GetInt32(0);
                    account.PasswordMD5 = ""; //hide password
                    account.RoleId = reader.GetInt32(2);
                    account.Name = reader.GetString(3);
                    account.Surname = reader.GetString(4);
                    account.Patronymic = reader.GetString(5);
                    account.Email = reader.GetString(6);
                    account.Phone = reader.GetString(7);
                    account.PhotoURL = reader.GetString(8);
                    account.BirthDay = reader.GetDateTime(9);
                    account.DepartmentId = reader.GetInt32(10);
                    account.Address = reader.GetString(11);


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

        public static Account getByEmail(string email)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, password_md5, role_id, name, surname, patronymic, email, phone, photo_url, birth_day, department_id, address 
                                FROM account
                                WHERE email = @email";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = cmd.ExecuteReader();
                Account account = new Account();
                if (reader.Read())
                {
                    account.Id = reader.GetInt32(0);
                    account.PasswordMD5 = reader.GetString(1);
                    account.RoleId = reader.GetInt32(2);
                    account.Name = reader.GetString(3);
                    account.Surname = reader.GetString(4);
                    account.Patronymic = reader.GetString(5);
                    account.Email = reader.GetString(6);
                    account.Phone = reader.GetString(7);
                    account.PhotoURL = reader.GetString(8);
                    account.BirthDay = reader.GetDateTime(9);
                    account.DepartmentId = reader.GetInt32(10);
                    account.Address = reader.GetString(11);

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
                                (password_md5, role_id, name, surname, patronymic, email, phone, photo_url, birth_day, department_id, address)
                                VALUES
                                (@password_md5, @role_id, @name, @surname, @patronymic, @email, @phone, @photo_url, @birth_day, @department_id, @address)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@password_md5", account.PasswordMD5);
                cmd.Parameters.AddWithValue("@role_id", account.RoleId);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@surname", account.Surname);
                cmd.Parameters.AddWithValue("@patronymic", account.Patronymic);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@phone", account.Phone);
                cmd.Parameters.AddWithValue("@photo_url", account.PhotoURL);
                cmd.Parameters.AddWithValue("@birth_day", account.BirthDay);
                cmd.Parameters.AddWithValue("@department_id", account.DepartmentId);
                cmd.Parameters.AddWithValue("@address", account.Address);
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
                                password_md5 = @pwd,
                                role_id = @role,
                                name=@name,
                                surname=@sname,
                                patronymic = @patr,
                                email = @email,
                                phone=@phone, 
                                photo_url = @photo_url,
                                birth_day = @birth_day,
                                department_id = @department_id
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", account.Id);
                cmd.Parameters.AddWithValue("@pwd", account.PasswordMD5);
                cmd.Parameters.AddWithValue("@role", account.RoleId);
                cmd.Parameters.AddWithValue("@name", account.Name);
                cmd.Parameters.AddWithValue("@sname", account.Surname);
                cmd.Parameters.AddWithValue("@patr", account.Patronymic);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@phone", account.Phone);
                cmd.Parameters.AddWithValue("@photo_url", account.PhotoURL);
                cmd.Parameters.AddWithValue("@birth_day", account.BirthDay);
                cmd.Parameters.AddWithValue("@department_id", account.DepartmentId);
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

        public static List<Account> getDialogs(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT account.id, account.name, account.surname, account.patronymic, account.photo_url
                                FROM message, account 
                                WHERE 
                                    (message.to_user_id = @id OR message.from_user_id = @id)
                                AND (account.id = message.to_user_id OR account.id = message.from_user_id)
                                AND account.Id <> @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Account> users = new List<Account>();
                while (reader.Read())
                {
                    int index = users.FindIndex(item => item.Id == reader.GetInt32(0));
                    if (index >= 0) continue;
                    Account acc = new Account();
                    acc.Id = reader.GetInt32(0);
                    acc.Name = reader.GetString(1);
                    acc.Surname = reader.GetString(2);
                    acc.Patronymic = reader.GetString(3);
                    acc.PhotoURL = reader.GetString(4);
                    users.Add(acc);
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

        public static int getCountNewMessages(int from, int to)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT COUNT(id)
                                FROM message
                                WHERE from_user_id = @from_user_id AND to_user_id = @to_user_id AND it_read = 0;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@from_user_id", from);
                cmd.Parameters.AddWithValue("@to_user_id", to);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                return 0;

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

        public static Message getLastMessage(int from, int to)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, from_user_id, to_user_id, message_content, date_of_message, it_read 
                                FROM message
                                WHERE (from_user_id = @from_user_id AND to_user_id = @to_user_id) 
                                OR (from_user_id = @to_user_id AND to_user_id = @from_user_id)
                                ORDER BY id DESC
                                LIMIT 1;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@from_user_id", from);
                cmd.Parameters.AddWithValue("@to_user_id", to);
                MySqlDataReader reader = cmd.ExecuteReader();
                Message message = new Message();
                if (reader.Read())
                {
                    message.Id = reader.GetInt32(0);
                    message.FromUserId = reader.GetInt32(1);
                    message.ToUserId = reader.GetInt32(2);
                    string msg = reader.GetString(3);
                    msg = (msg.Length <= 25) ? msg : (msg.Substring(0, 25)+"...");
                    message.MessageContent = msg;
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

        public static List<Message> getDialog(int to_id, int from_id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, from_user_id, to_user_id, message_content, date_of_message, it_read
                                FROM message 
                                WHERE
                                (to_user_id = @id1 AND from_user_id = @id2) OR
                                (to_user_id = @id2 AND from_user_id = @id1)";
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
                    MessageDAO.update(message);
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

        public static List<Message> getNewMessages(int from, int to)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, from_user_id, to_user_id, message_content, date_of_message, it_read
                                FROM message 
                                WHERE to_user_id = @to_user_id AND from_user_id = @from_user_id AND it_read = 0";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@from_user_id", from);
                cmd.Parameters.AddWithValue("@to_user_id", to);
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
                    MessageDAO.update(message);
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

        public static int update(Message m)
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
                cmd.Parameters.AddWithValue("@id", m.Id);
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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM message 
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

        public static int deleteAllMessageByUserId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM message 
                                WHERE from_user_id = @id OR to_user_id = @id";
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


    public class TeacherToGroupDAO : DAO
    {
        public static int add(TeacherToGroup model)
        {
            if (model == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO teacher_to_group 
                                (id_account, id_group)
                                VALUES
                                (@id_account, @id_group)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", model.AccountId);
                cmd.Parameters.AddWithValue("@id_group", model.GroupId);
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

        public static TeacherToGroup getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM teacher_to_group
                                WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                TeacherToGroup model = new TeacherToGroup();
                if (reader.Read())
                {
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);
                }
                return model;

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

        public static bool consists(TeacherToGroup model)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id 
                                FROM teacher_to_group
                                WHERE id_account = @id_account AND id_group = @id_group";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", model.AccountId);
                cmd.Parameters.AddWithValue("@id_group", model.GroupId);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
                return false;

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

        public static List<TeacherToGroup> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM 
                                teacher_to_group";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<TeacherToGroup> models = new List<TeacherToGroup>();
                while (reader.Read())
                {
                    TeacherToGroup model = new TeacherToGroup();
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);

                    models.Add(model);
                }
                return models;
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

        public static List<Account> getAllAccountByGroupId(int groupId)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                account.id,  account.role_id, account.name, account.surname, account.patronymic,
                                account.email, account.phone, account.photo_url, account.birth_day, account.department_id, account.address 
                                FROM account, teacher_to_group
                                WHERE teacher_to_group.id_group = @group_id AND teacher_to_group.id_account = account.id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@group_id", groupId);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Account> accounts = new List<Account>();
                while (reader.Read())
                {
                    Account account = new Account();
                    account.Id = reader.GetInt32(0);
                    account.RoleId = reader.GetInt32(1);
                    account.Name = reader.GetString(2);
                    account.Surname = reader.GetString(3);
                    account.Patronymic = reader.GetString(4);
                    account.Email = reader.GetString(5);
                    account.Phone = reader.GetString(6);
                    account.PhotoURL = reader.GetString(7);
                    account.BirthDay = reader.GetDateTime(8);
                    account.DepartmentId = reader.GetInt32(9);
                    account.Address = reader.GetString(10);

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

        public static List<StudentGroup> getAllGroupsByUserIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                student_group.id, student_group.name, student_group.department_id
                                FROM student_group, teacher_to_group
                                WHERE teacher_to_group.id_account = @id_account AND teacher_to_group.id_group = student_group.id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentGroup> studentGroups = new List<StudentGroup>();
                while (reader.Read())
                {
                    StudentGroup group = new StudentGroup();
                    group.Id = reader.GetInt32(0);
                    group.Name = reader.GetString(1);
                    group.DepartmentID = reader.GetInt32(2);
                    studentGroups.Add(group);
                }
                return studentGroups;

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

        public static List<TeacherToGroup> getAllByGroupId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM 
                                teacher_to_group
                                WHERE id_group = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<TeacherToGroup> models = new List<TeacherToGroup>();
                while (reader.Read())
                {
                    TeacherToGroup model = new TeacherToGroup();
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);

                    models.Add(model);
                }
                return models;
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

        public static List<TeacherToGroup> getAllByAccountId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM 
                                teacher_to_group
                                WHERE id_account = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<TeacherToGroup> models = new List<TeacherToGroup>();
                while (reader.Read())
                {
                    TeacherToGroup model = new TeacherToGroup();
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);

                    models.Add(model);
                }
                return models;
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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM teacher_to_group 
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

        public static int deleteAllByUserId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM teacher_to_group 
                                WHERE id_account = @id";
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

        public static int deleteAllByGroupId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM teacher_to_group 
                                WHERE id_group = @id";
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

        public static int delete(TeacherToGroup model)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM teacher_to_group 
                                WHERE id_account = @id_account AND id_group = @id_group";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", model.AccountId);
                cmd.Parameters.AddWithValue("@id_group", model.GroupId);
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

    public class StudentToGroupDAO : DAO
    {
        public static int add(StudentToGroup model)
        {
            if (model == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO student_to_group 
                                (id_account, id_group)
                                VALUES
                                (@id_account, @id_group)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", model.AccountId);
                cmd.Parameters.AddWithValue("@id_group", model.GroupId);
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

        public static StudentToGroup getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM student_to_group
                                WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                StudentToGroup model = new StudentToGroup();
                if (reader.Read())
                {
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);
                }
                return model;

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

        public static bool consists(StudentToGroup model)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id 
                                FROM student_to_group
                                WHERE id_account = @id_account AND id_group = @id_group";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", model.AccountId);
                cmd.Parameters.AddWithValue("@id_group", model.GroupId);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
                return false;

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

        public static List<StudentToGroup> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM 
                                student_to_group";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentToGroup> models = new List<StudentToGroup>();
                while (reader.Read())
                {
                    StudentToGroup model = new StudentToGroup();
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);

                    models.Add(model);
                }
                return models;
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

        public static List<Account> getAllAccountByGroupId(int groupId)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                account.id,  account.role_id, account.name, account.surname, account.patronymic,
                                account.email, account.phone, account.photo_url, account.birth_day, account.department_id, account.address 
                                FROM account, student_to_group
                                WHERE student_to_group.id_group = @group_id AND student_to_group.id_account = account.id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@group_id", groupId);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Account> accounts = new List<Account>();
                while (reader.Read())
                {
                    Account account = new Account();
                    account.Id = reader.GetInt32(0);
                    account.RoleId = reader.GetInt32(1);
                    account.Name = reader.GetString(2);
                    account.Surname = reader.GetString(3);
                    account.Patronymic = reader.GetString(4);
                    account.Email = reader.GetString(5);
                    account.Phone = reader.GetString(6);
                    account.PhotoURL = reader.GetString(7);
                    account.BirthDay = reader.GetDateTime(8);
                    account.DepartmentId = reader.GetInt32(9);
                    account.Address = reader.GetString(10);

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

        public static List<StudentToGroup> getAllByGroupId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM 
                                student_to_group
                                WHERE id_group = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentToGroup> models = new List<StudentToGroup>();
                while (reader.Read())
                {
                    StudentToGroup model = new StudentToGroup();
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);

                    models.Add(model);
                }
                return models;
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

        public static List<StudentToGroup> getAllByAccountId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, id_account, id_group 
                                FROM 
                                student_to_group
                                WHERE id_account = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentToGroup> models = new List<StudentToGroup>();
                while (reader.Read())
                {
                    StudentToGroup model = new StudentToGroup();
                    model.Id = reader.GetInt32(0);
                    model.AccountId = reader.GetInt32(1);
                    model.GroupId = reader.GetInt32(2);

                    models.Add(model);
                }
                return models;
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

        public static List<StudentGroup> getAllGroupsByUserIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                student_group.id, student_group.name, student_group.department_id
                                FROM student_group, student_to_group
                                WHERE student_to_group.id_account = @id_account AND student_to_group.id_group = student_group.id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentGroup> studentGroups = new List<StudentGroup>();
                while (reader.Read())
                {
                    StudentGroup group = new StudentGroup();
                    group.Id = reader.GetInt32(0);
                    group.Name = reader.GetString(1);
                    group.DepartmentID = reader.GetInt32(2);
                    studentGroups.Add(group);
                }
                return studentGroups;

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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM student_to_group 
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

        public static int deleteAllByUserId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM student_to_group 
                                WHERE id_account = @id";
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

        public static int deleteAllByGroupId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM student_to_group 
                                WHERE id_group = @id";
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

        public static int delete(StudentToGroup model)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM student_to_group 
                                WHERE id_account = @id_account AND id_group = @id_group";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id_account", model.AccountId);
                cmd.Parameters.AddWithValue("@id_group", model.GroupId);
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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM role 
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

    public class UniversityDAO : DAO
    {
        public static int add(University uni)
        {
            if (uni == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO university
                                (full_name, short_name, description, logo_url,  address, web_site)
                                VALUES
                                (@full_name, @short_name, @description, @logo_url,  @address, @web_site)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@full_name", uni.FullName);
                cmd.Parameters.AddWithValue("@short_name", uni.ShortName);
                cmd.Parameters.AddWithValue("@description", uni.Description);
                cmd.Parameters.AddWithValue("@logo_url", uni.LogoURL);
                cmd.Parameters.AddWithValue("@address", uni.Address);
                cmd.Parameters.AddWithValue("@web_site", uni.WebSite);
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


        public static List<University> getAllLite()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, short_name, address, logo_url
                                FROM university";
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
                    University uni = new University();
                    uni.Id = reader.GetInt32(0);
                    uni.ShortName = reader.GetString(1);
                    uni.Address = reader.GetString(2);
                    uni.LogoURL = reader.GetString(3);
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


        public static List<University> getAll()
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, full_name, short_name, description, logo_url,  address, web_site
                                FROM university";
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
                        reader.GetInt32(0), // id
                        reader.GetString(1), // full name
                        reader.GetString(2), // short name
                        reader.GetString(3), // description
                        reader.GetString(4), // logo url
                        reader.GetString(5), // address
                        reader.GetString(6) // web site
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

        public static University getByIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, short_name, address, logo_url
                                FROM university
                                WHERE id = @Id";
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
                    unisersity.ShortName = reader.GetString(1);
                    unisersity.Address = reader.GetString(2);
                    unisersity.LogoURL = reader.GetString(3);

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

        public static University getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, full_name, short_name, description, logo_url,  address, web_site
                                FROM university
                                WHERE id = @Id";
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
                    unisersity.FullName = reader.GetString(1);
                    unisersity.ShortName = reader.GetString(2);
                    unisersity.Description = reader.GetString(3);
                    unisersity.LogoURL = reader.GetString(4);
                    unisersity.Address = reader.GetString(5);
                    unisersity.WebSite = reader.GetString(6);

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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM university 
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

        public static int update(University university)
        {
            if (university == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE university 
                                SET 
                                full_name = @fullname,
                                short_name = @short_name,
                                description = @description,
                                logo_url = @logo_url,
                                address = @address,
                                web_site = @web_site
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", university.Id);
                cmd.Parameters.AddWithValue("@fullname", university.FullName);
                cmd.Parameters.AddWithValue("@short_name", university.ShortName);
                cmd.Parameters.AddWithValue("@description", university.Description);
                cmd.Parameters.AddWithValue("@logo_url", university.LogoURL);
                cmd.Parameters.AddWithValue("@address", university.Address);
                cmd.Parameters.AddWithValue("@web_site", university.WebSite);
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

    public class DepartmentDAO : DAO
    {
        public static int add(Department d)
        {
            if (d == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO department
                                (name, description, dean_name, university_id)
                                VALUES
                                (@name, @description, @dean_name,  @university_id)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", d.Name);
                cmd.Parameters.AddWithValue("@description", d.Description);
                cmd.Parameters.AddWithValue("@dean_name", d.DeanName);
                cmd.Parameters.AddWithValue("@university_id", d.UniversityId);
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
            string sqlQuery = @"SELECT 
                                id, name, description, dean_name, university_id
                                FROM department
                                WHERE id = @Id";
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
                    department.Description = reader.GetString(2);
                    department.DeanName = reader.GetString(3);
                    department.UniversityId = reader.GetInt32(4);
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


        public static Department getByIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, name, university_id
                                FROM department
                                WHERE id = @Id";
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
                    department.UniversityId = reader.GetInt32(2);
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
            string sqlQuery = @"SELECT
                                id, name, description, dean_name, university_id
                                FROM department
                                WHERE university_id = @Id";
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
                    department.Description = reader.GetString(2);
                    department.DeanName = reader.GetString(3);
                    department.UniversityId = reader.GetInt32(4);
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

        public static List<Department> getAllByUniversityIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, name, university_id
                                FROM department
                                WHERE university_id = @Id";
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
                    department.UniversityId = reader.GetInt32(2);
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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM department 
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

        public static int update(Department department)
        {
            if (department == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE department 
                                SET 
                                name = @name,
                                description = @description,
                                dean_name = @dean_name,
                                university_id = @university_id
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", department.Id);
                cmd.Parameters.AddWithValue("@name", department.Name);
                cmd.Parameters.AddWithValue("@description", department.Description);
                cmd.Parameters.AddWithValue("@dean_name", department.DeanName);
                cmd.Parameters.AddWithValue("@university_id", department.UniversityId);
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

    public class StudentGroupDAO : DAO
    {
        public static int add(StudentGroup s)
        {
            if (s == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"INSERT INTO student_group 
                                (name, description, created_date, department_id)
                                VALUES
                                (@name, @description, @created_date, @department_id)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@name", s.Name);
                cmd.Parameters.AddWithValue("@created_date", s.CreatedDate);
                cmd.Parameters.AddWithValue("@description", s.Description);
                cmd.Parameters.AddWithValue("@department_id", s.DepartmentID);
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

        public static List<StudentGroup> getAllByDepartmentId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, name, description, created_date, department_id
                                FROM student_group
                                WHERE department_id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentGroup> studentGroups = new List<StudentGroup>();
                while (reader.Read())
                {
                    StudentGroup group = new StudentGroup();
                    group.Id = reader.GetInt32(0);
                    group.Name = reader.GetString(1);
                    group.Description = reader.GetString(2);
                    group.CreatedDate = reader.GetDateTime(3);
                    group.DepartmentID = reader.GetInt32(4);
                    studentGroups.Add(group);
                }
                return studentGroups;

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

        public static List<StudentGroup> getAllByDepartmentIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, name, department_id
                                FROM student_group
                                WHERE department_id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<StudentGroup> studentGroups = new List<StudentGroup>();
                while (reader.Read())
                {
                    StudentGroup group = new StudentGroup();
                    group.Id = reader.GetInt32(0);
                    group.Name = reader.GetString(1);
                    group.DepartmentID = reader.GetInt32(2);
                    studentGroups.Add(group);
                }
                return studentGroups;

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


        public static StudentGroup getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, name, description, created_date, department_id
                                FROM student_group
                                WHERE id = @Id;";
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
                    studentGroup.Description = reader.GetString(2);
                    studentGroup.CreatedDate = reader.GetDateTime(3);
                    studentGroup.DepartmentID = reader.GetInt32(4);
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


        public static StudentGroup getByIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, name, department_id
                                FROM student_group
                                WHERE id = @Id;";
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
                    studentGroup.DepartmentID = reader.GetInt32(2);
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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM student_group 
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

        public static int update(StudentGroup sg)
        {
            if (sg == null) return 0;
            MySqlConnection conn = null;
            string sqlQuery = @"UPDATE student_group 
                                SET 
                                name = @name,
                                description = @description,
                                created_date = @created_date,
                                department_id = @department_id
                                WHERE id = @id;";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", sg.Id);
                cmd.Parameters.AddWithValue("@name", sg.Name);
                cmd.Parameters.AddWithValue("@description", sg.Description);
                cmd.Parameters.AddWithValue("@created_date", sg.CreatedDate);
                cmd.Parameters.AddWithValue("@department_id", sg.DepartmentID);
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
            string sqlQuery = @"INSERT INTO exam
                                (title, description, author_id, count_of_question, content)
                                VALUES
                                (@title, @descr, @author_id, @question, @content)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@title", e.Title);
                cmd.Parameters.AddWithValue("@descr", e.Description);
                cmd.Parameters.AddWithValue("@author_id", e.AuthorId);
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
            string sqlQuery = @"SELECT
                                id, title, description, author_id, count_of_question, content 
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
                        reader.GetInt32(3),
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

        public static List<Exam> getAllByDepartmentId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                exam.id, exam.title, exam.description, exam.author_id, exam.count_of_question, exam.content 
                                FROM exam, account, department
                                WHERE exam.author_id = account.id AND account.department_id = department.id AND department.id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Exam> exams = new List<Exam>();
                while (reader.Read())
                {
                    Exam exam = new Exam(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetInt32(3),
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

        public static List<Exam> getAllByDepartmentIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                exam.id, exam.title, exam.description, exam.author_id, exam.count_of_question
                                FROM exam, account, department
                                WHERE exam.author_id = account.id AND account.department_id = department.id AND department.id = @id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Exam> exams = new List<Exam>();
                while (reader.Read())
                {
                    Exam exam = new Exam();

                    exam.Id = reader.GetInt32(0);
                    exam.Title = reader.GetString(1);
                    exam.Description = reader.GetString(2);
                    exam.AuthorId = reader.GetInt32(3);
                    exam.CountOfQuestion = reader.GetInt32(4);

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
            string sqlQuery = @"SELECT
                                id, title, description, author_id, count_of_question, content 
                                FROM exam
                                WHERE id = @id";
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
                        reader.GetInt32(3),
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

        public static Exam getByIdLite(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT
                                id, title, description, author_id, count_of_question 
                                FROM exam
                                WHERE id = @id";
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
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        null
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

                    examHistory.Exam = ExamDAO.getByIdLite(examHistory.ExamId);

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

        public static List<ExamHistory> getAllByTeacherId(int teacherId, int examId)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT id, exam_id, teacher_id, deadline, result, status_id, student_id, passing_score, date_of_test
                                FROM exam_history
                                WHERE teacher_id = @teacher_id AND exam_id = @exam_id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@teacher_id", teacherId);
                cmd.Parameters.AddWithValue("@exam_id", examId);
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



                    examHistory.Exam = ExamDAO.getByIdLite(examHistory.ExamId);

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
            string sqlQuery = @"SELECT 
                                id, exam_id, teacher_id, deadline, result, status_id, student_id, passing_score, date_of_test
                                FROM exam_history
                                WHERE id = @Id;";
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

                    examHistory.Exam = ExamDAO.getById(examHistory.ExamId);

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

    public class TutorialDAO : DAO
    {
        public static int add(Tutorial model)
        {
            if (model == null) return 0;
            MySqlConnection conn = null;

            string sqlQuery = @"INSERT INTO tutorial
                                (name, content, author_id, group_id)
                                VALUES
                                (@name, @content, @author_id, @group_id)";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;

                cmd.Parameters.AddWithValue("@name", model.Name);
                cmd.Parameters.AddWithValue("@content", model.Content);
                cmd.Parameters.AddWithValue("@author_id", model.AuthorId);
                cmd.Parameters.AddWithValue("@group_id", model.StudentGroupId);

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

        public static Tutorial getById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, name, content, author_id, group_id
                                FROM tutorial
                                WHERE id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                Tutorial tutorial = new Tutorial();
                if (reader.Read())
                {
                    tutorial.Id = reader.GetInt32(0);
                    tutorial.Name = reader.GetString(1);
                    tutorial.Content = reader.GetString(2);
                    tutorial.AuthorId = reader.GetInt32(3);
                    tutorial.StudentGroupId = reader.GetInt32(4);
                }
                return tutorial;

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

        public static List<Tutorial> getAllByStudentGroupId(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"SELECT 
                                id, name, content, author_id, group_id
                                FROM tutorial
                                WHERE group_id = @Id";
            try
            {
                conn = getConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlQuery;
                cmd.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Tutorial> tutorials = new List<Tutorial>();
                
                while (reader.Read())
                {
                    Tutorial tutorial = new Tutorial();

                    tutorial.Id = reader.GetInt32(0);
                    tutorial.Name = reader.GetString(1);
                    tutorial.Content = reader.GetString(2);
                    tutorial.AuthorId = reader.GetInt32(3);
                    tutorial.StudentGroupId = reader.GetInt32(4);

                    tutorials.Add(tutorial);
                }
                return tutorials;

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

        public static int deleteById(int id)
        {
            MySqlConnection conn = null;
            string sqlQuery = @"DELETE FROM tutorial 
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
