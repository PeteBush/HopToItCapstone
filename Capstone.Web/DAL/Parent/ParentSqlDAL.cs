﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using System.Data.SqlClient;
using Capstone.Web.Models.ViewModels;

namespace Capstone.Web.DAL.Parent
{
    public class ParentSqlDAL : IParentDAL
    {
        private readonly string connectionString;

        private const string SQL_GetParent = "SELECT * FROM parent WHERE email = @email;";
        private const string SQL_CreateParent = "INSERT INTO parent VALUES (@first_name, @last_name, @email, @password, @salt);";
        private const string SQL_GetChildren = "SELECT * FROM child INNER JOIN parent on parent.parent_id = child.parent_id WHERE parent.parent_id = @parent_id;";

        public ParentSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void CreateParent(ParentModel newParent)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CreateParent, conn);
                    cmd.Parameters.AddWithValue("@first_name", newParent.First_Name);
                    cmd.Parameters.AddWithValue("@last_name", newParent.Last_Name);
                    cmd.Parameters.AddWithValue("@email", newParent.Email);
                    cmd.Parameters.AddWithValue("@password", newParent.Password);
                    cmd.Parameters.AddWithValue("@salt", newParent.Salt);

                    int result = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                throw;
            }
        }

        public ParentModel GetParent(string emailAddress)
        {
            ParentModel parent = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetParent, conn);
                    cmd.Parameters.AddWithValue("@email", emailAddress);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        parent = new ParentModel
                        {
                            Email = Convert.ToString(reader["email"]),
                            First_Name = Convert.ToString(reader["first_name"]),
                            Last_Name = Convert.ToString(reader["last_name"]),
                            Parent_ID = Convert.ToInt32(reader["parent_id"]),
                            Password = Convert.ToString(reader["p_word"]),
                            Salt = Convert.ToString(reader["salt"]),
                        };
                    }
                }
            }
            catch (SqlException e)
            {
                throw;
            }
            return parent;
        }

        public List<ChildModel> GetChildren(int parent_Id)
        {
            List<ChildModel> children = new List<ChildModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetChildren, conn);
                    cmd.Parameters.AddWithValue("@parent_id", parent_Id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        ChildModel child = new ChildModel
                        {
                            Child_Id = Convert.ToInt32(reader["child_id"]),
                            First_Name = Convert.ToString(reader["first_name"]),
                            UserName = Convert.ToString(reader["username"]),
                            Seconds = Convert.ToInt32(reader["seconds"]),
                            Carrots = Convert.ToInt32(reader["carrots"]),
                           
                        };
                         children.Add(child);
                    }
                }
            }
            catch (SqlException e)
            {
                throw;
            }
            return children;
        }
    }
}