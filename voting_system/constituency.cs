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

        static string connection_string = "server = LAPTOP-MI8QQQVT;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };

        public string Constituencyname;
        public string District;
        public string State;
        public List<Candidate> Candidates;


        public Constituency(string name, string district, string state)
        {
            Constituencyname = name;
            District = district;
            State = state;
            Candidates = new List<Candidate>();
        }

        public override void Fetchfromtable()
        {

            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "SELECT * FROM CONSTITUENCY_TABLE WHERE CONSTITUENCY=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", Constituencyname);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();



            }

        }

        public override void Deletefromtable()
        {
            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "DELETE FROM CONSTITUENCY_TABLE WHERE NAME=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", Constituencyname);
                conn.Open();
                cmd.ExecuteNonQuery();
            }



        }

        public override void Updatetotable()
        {


        }
        public override void Savetotable()
        {

            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "INSERT INTO CONSTITUENCY_TABLE (CONSTITUENCY, DISTRICT, STATE) VALUES (@name, @dist, @state)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", Constituencyname);

                cmd.Parameters.AddWithValue("@dist", District);
                cmd.Parameters.AddWithValue("@state", State);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void Display()
        {
            Console.WriteLine($"Constituency: {Constituencyname}");
            Console.WriteLine($"District: {District}");
            Console.WriteLine($"State: {State}");
            Console.WriteLine("Candidates:");

            foreach (var cand in Candidates)
            {
                Console.WriteLine($"   - {cand.CandidateName} ({cand.PartyName}) | Votes: {cand.VoteCount}");
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

