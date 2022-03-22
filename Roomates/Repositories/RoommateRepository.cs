﻿using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Chore.Id AS ChoreId, Chore.Name AS ChoreName
                                        FROM Chore LEFT JOIN RoommateChore ON Chore.Id = RoommateChore.ChoreId
                                        WHERE RoommateId is null";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room()
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                                }
                            };
                        }
                        return roommate;
                    }

                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName FROM Roommate";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                            string firstNameValue = reader.GetString(firstNameColumnPosition);

                            int lastNameColumnPosition = reader.GetOrdinal("LastName");
                            string lastNameValue = reader.GetString(lastNameColumnPosition);

                            Roommate roommate = new Roommate
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue
                            };

                            roommates.Add(roommate);
                        }
                        return roommates;
                    }

                }
            }
        }
    }
}