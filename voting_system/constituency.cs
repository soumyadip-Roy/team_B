using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace voting_system
{


    public class Constituency : Database
    {
        private static readonly string connectionString = DbConfig.ConnectionString;

        public string Constituencyname { get; set; } = "";
        public string District { get; set; } = "";
        public string State { get; set; } = "";
        public List<Candidate> Candidates { get; set; } = new List<Candidate>();

        public Constituency() { }

        public Constituency(string name, string district, string state)
        {
            Constituencyname = name;
            District = district;
            State = state;
        }

        public override void Fetchfromtable()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CONSTITUENCY, DISTRICT, STATE FROM CONSTITUENCY_TABLE WHERE CONSTITUENCY=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", Constituencyname ?? "");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Constituencyname = reader.IsDBNull(0) ? Constituencyname : reader.GetString(0);
                            District = reader.IsDBNull(1) ? District : reader.GetString(1);
                            State = reader.IsDBNull(2) ? State : reader.GetString(2);
                        }
                    }
                }
            }
        }

        public override void Deletefromtable()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM CONSTITUENCY_TABLE WHERE CONSTITUENCY=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", Constituencyname ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public override void Updatetotable()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE CONSTITUENCY_TABLE SET DISTRICT=@dist, STATE=@state WHERE CONSTITUENCY=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dist", District ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@state", State ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@name", Constituencyname ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public override void Savetotable()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO CONSTITUENCY_TABLE (CONSTITUENCY, DISTRICT, STATE) VALUES (@name, @dist, @state)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", Constituencyname ?? "");
                    cmd.Parameters.AddWithValue("@dist", District ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@state", State ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Display()
        {
            Console.WriteLine($"Constituency: {Constituencyname} | District: {District} | State: {State}");
            foreach (var c in Candidates)
            {
                Console.WriteLine($"   - {c.CandidateName} ({c.PartyName}) Votes: {c.VoteCount}");
            }
        }

        public void topcandidate()
        {
            if (Candidates.Count == 0)
            {
                Console.WriteLine($"No candidates found in {Constituencyname}");
                return;
            }

            var top = Candidates.OrderByDescending(c => c.VoteCount).First();
            Console.WriteLine($"Top candidate in {Constituencyname}: {top.CandidateName} ({top.PartyName}) with {top.VoteCount} votes.");
        }

        public void Tvotes()
        {
            int total = Candidates.Sum(c => c.VoteCount);
            Console.WriteLine($"Total votes in {Constituencyname}: {total}");
        }
    }

}

