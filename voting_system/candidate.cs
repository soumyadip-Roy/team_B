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
                string query = "SELECT * FROM Candidate WHERE CandidateName=@name";
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
                string query = "UPDATE Candidate SET VoteCount=@votes WHERE CandidateName=@name";
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
                string query = "DELETE FROM Candidate WHERE CandidateName=@name";
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
                string query = "INSERT INTO Candidate (CandidateName, PartyName, Constituency, VoteCount) VALUES (@name, @party, @const, @votes)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", CandidateName);
                cmd.Parameters.AddWithValue("@party", PartyName);
                cmd.Parameters.AddWithValue("@const", Constituency);
                cmd.Parameters.AddWithValue("@votes", VoteCount);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

       
        public void Displaycand() { }
        public void IncreaseVoteCount() { }    
        public void Assignconstituency(string constituencyName) { }
        
        
        
    }
}

