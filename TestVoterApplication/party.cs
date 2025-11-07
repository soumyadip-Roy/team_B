using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVoterApplication
{
    public class Party : Database
    {
        public string PartyName;
        public string PartySymbol;
        public string LeaderName;
        public int VoteCount;
        public List<Candidate> Members  = new List<Candidate>();

        static string connection_string = "server = LAPTOP-MDVNPCGM;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };

        public Party() { }
        public Party(string name, string symbol, string leader)
        {
            PartyName = name;
            PartySymbol = symbol;
            LeaderName = leader;
        }

        public override void Fetchfromtable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "SELECT NAME, LOGO, LEADER FROM PARTY_TABLE WHERE NAME=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", PartyName ?? "");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PartyName = reader.IsDBNull(0) ? PartyName : reader.GetString(0);
                            PartySymbol = reader.IsDBNull(1) ? PartySymbol : reader.GetString(1);
                            LeaderName = reader.IsDBNull(2) ? LeaderName : reader.GetString(2);
                        }
                    }
                }
            }
        }

        public override void Updatetotable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "UPDATE PARTY_TABLE SET LOGO=@logo, LEADER=@leader, VOTE_COUNT=@votes WHERE NAME=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@logo", PartySymbol ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@leader", LeaderName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@votes", VoteCount);
                    cmd.Parameters.AddWithValue("@name", PartyName ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public override void Deletefromtable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "DELETE FROM PARTY_TABLE WHERE NAME=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", PartyName ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public override void Savetotable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "INSERT INTO PARTY_TABLE (NAME, LOGO, LEADER, VOTE_COUNT) VALUES (@name, @logo, @leader, @votes)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", PartyName ?? "");
                    cmd.Parameters.AddWithValue("@logo", PartySymbol ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@leader", LeaderName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@votes", VoteCount);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RecalculateVotes()
        {
            VoteCount = Members.Sum(m => m.VoteCount);
        }

        public void Display()
        {
            Console.WriteLine($"Party: {PartyName} | Leader: {LeaderName} | Symbol: {PartySymbol} | TotalVotes: {VoteCount}");
            foreach (var m in Members)
            {
                Console.WriteLine($"   - {m.CandidateName} ({m.Constituency}) Votes: {m.VoteCount}");
            }
        }

        public void winner()
        {
            if (Members.Count == 0)
            {
                Console.WriteLine($"No candidates in {PartyName}");
                return;
            }

            var win = Members.OrderByDescending(m => m.VoteCount).First();
            Console.WriteLine($"Winner from {PartyName}: {win.CandidateName} ({win.Constituency}) with {win.VoteCount} votes.");
        }

    }
}
