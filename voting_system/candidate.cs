using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;



namespace voting_system
{
    public class Candidate : Party
    {

        static string connection_string = "server = LAPTOP-MI8QQQVT;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };

        public string CandidateName;
        public string Constituency;
        public int VoteCount;


        public Candidate(
            string candidateName,
            string partyName,
            string partySymbol,
            string leader,
            string constituency
        ) : base(partyName, partySymbol, leader)
        {
            CandidateName = candidateName;
            Constituency = constituency;
            VoteCount = 0;
        }


        public override void Fetchfromtable()
        {
            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "SELECT * FROM CANDIDATE_TABLE WHERE NAME=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", CandidateName);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();



            }


        }
        public override void Updatetotable()
        {

            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "UPDATE CANDIDATE_TABLE SET VOTE_COUNT=@votes WHERE NAME=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@votes", VoteCount);
                cmd.Parameters.AddWithValue("@name", CandidateName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }
        public override void Deletefromtable()
        {
            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "DELETE FROM CANDIDATE_TABLE WHERE CandidateName=@name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", CandidateName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }


        }
        public override void Savetotable()
        {

            using (SqlConnection conn = new SqlConnection(connection_string))
            {
                string query = "INSERT INTO CANDIDATE_TABLE (NAME, CONSTITUENCY, PARTY, VOTE_COUNT) VALUES (@name, @const, @party, @votes)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", CandidateName);

                cmd.Parameters.AddWithValue("@const", Constituency);
                cmd.Parameters.AddWithValue("@party", PartyName);
                cmd.Parameters.AddWithValue("@votes", VoteCount);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }


        public void Displaycand()
        {
            Console.WriteLine($"Candidate: {CandidateName}");
            Console.WriteLine($"Party: {PartyName}");
            Console.WriteLine($"Constituency: {Constituency}");
            Console.WriteLine($"Votes: {VoteCount}");
        }
        public void IncreaseVoteCount()
        {
            VoteCount += 1;


            Updatetotable();
            Console.WriteLine($"{CandidateName} now has {VoteCount} votes!");


        }
        public void Assignconstituency(string constituencyName)
        {
            Constituency = constituencyName;
            Updatetotable();
            Console.WriteLine($"{CandidateName} assigned to constituency: {constituencyName}");
        }



    }
}


