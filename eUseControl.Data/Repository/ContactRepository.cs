using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Data.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Contact> GetAllContacts()
        {
            List<Contact> contacts = new List<Contact>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetAllContacts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(MapContactFromReader(reader));
                        }
                    }
                }
            }

            return contacts;
        }

        public Contact GetContactById(int id)
        {
            Contact contact = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetContactById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            contact = MapContactFromReader(reader);
                        }
                    }
                }
            }

            return contact;
        }

        public int CreateContact(Contact contact)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_CreateContact", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = contact.FullName;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = contact.Email;
                    command.Parameters.Add("@Subject", SqlDbType.NVarChar, 200).Value = contact.Subject;
                    command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = contact.Message;
                    command.Parameters.Add("@IPAddress", SqlDbType.NVarChar, 50).Value = 
                        (object)contact.IPAddress ?? DBNull.Value;

                    connection.Open();
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public void MarkAsRead(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_MarkContactAsRead", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStatus(int id, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateContactStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void MarkResponseSent(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_MarkResponseSent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Contact> GetContactsByStatus(string status)
        {
            List<Contact> contacts = new List<Contact>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetContactsByStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(MapContactFromReader(reader));
                        }
                    }
                }
            }

            return contacts;
        }

        public List<Contact> GetUnreadContacts()
        {
            List<Contact> contacts = new List<Contact>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetUnreadContacts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(MapContactFromReader(reader));
                        }
                    }
                }
            }

            return contacts;
        }

        public bool DeleteContact(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_DeleteContact", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private Contact MapContactFromReader(SqlDataReader reader)
        {
            return new Contact
            {
                Id = Convert.ToInt32(reader["Id"]),
                FullName = reader["FullName"].ToString(),
                Email = reader["Email"].ToString(),
                Subject = reader["Subject"].ToString(),
                Message = reader["Message"].ToString(),
                IsRead = Convert.ToBoolean(reader["IsRead"]),
                DateSubmitted = Convert.ToDateTime(reader["DateSubmitted"]),
                DateRead = reader["DateRead"] != DBNull.Value ? Convert.ToDateTime(reader["DateRead"]) : (DateTime?)null,
                Status = reader["Status"].ToString(),
                ResponseSent = Convert.ToBoolean(reader["ResponseSent"]),
                IPAddress = reader["IPAddress"] != DBNull.Value ? reader["IPAddress"].ToString() : null
            };
        }
    }
}