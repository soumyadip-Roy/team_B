using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voting_system
{
    public class Party : Database
    {

        public string PartyName;
        public string PartySymbol;
        public string LeaderName;
        public int VoteCount;
        public string Constituency;
        public List<Candidate> Members;

        static string connection_string = "server = LAPTOP-MI8QQQVT;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };


        public Party(string name, string symbol, string leader)
        {
            PartyName = name;
            PartySymbol = symbol;
            LeaderName = leader;
            VoteCount = 0;
            Members = new List<Candidate>();
        }


        public override void Fetchfromtable()
        {
            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "SELECT * FROM PARTY_TABLE WHERE NAME=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", PartyName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();



            }


        }
        public override void Updatetotable()
        {

            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "UPDATE PARTY_TABLE SET VOTE_COUNT=@votes WHERE NAME=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@votes", VoteCount);
                cmd.Parameters.AddWithValue("@name", PartyName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public override void Deletefromtable()
        {
            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "DELETE FROM PARTY_TABLE WHERE NAME=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", PartyName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }


        }
        public override void Savetotable()
        {
            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "INSERT INTO PARTY_TABLE (NAME, CONSTITUENCY, LOGO, VOTE_COUNT) VALUES (@name, @const, @logo, @votes)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", PartyName);

                cmd.Parameters.AddWithValue("@const", Constituency);
                cmd.Parameters.AddWithValue("@logo", PartySymbol);
                cmd.Parameters.AddWithValue("@votes", VoteCount);
                conn.Open();
                cmd.ExecuteNonQuery();
            }


        }



        public void Display()
        {
            Console.WriteLine($"Party: {PartyName}");
            Console.WriteLine($"Leader: {LeaderName}");
            Console.WriteLine($"Symbol: {PartySymbol}");
            Console.WriteLine($"Total Votes: {VoteCount}");
            Console.WriteLine("Members:");

            foreach (var mem in Members)
            {
                Console.WriteLine($"   - {mem.CandidateName} ({mem.Constituency}) | Votes: {mem.VoteCount}");
            }
        }
        public void Tpartyvotes()
        {
            VoteCount = Members.Sum(c => c.VoteCount);
            Console.WriteLine($"Total votes for {PartyName}: {VoteCount}");
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
