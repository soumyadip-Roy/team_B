using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;



namespace TestVoterApplication
{
    public class Candidate
    {
        static string connection_string = "server = LAPTOP-MDVNPCGM;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };

        public string CandidateName;
        public string PartyName ;
        public string PartySymbol;
        public string Constituency ;
        public int VoteCount;
        public string CandidateId;
        public Candidate() { }

        public Candidate(string candidateName, string partyName, string partySymbol, string constituency, string candidateId)
        {
            CandidateName = candidateName;
            PartyName = partyName;
            PartySymbol = partySymbol;
            Constituency = constituency;
            CandidateId = candidateId;
        }

        public void Fetchfromtable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "SELECT NAME, CONSTITUENCY, PARTY, VOTE_COUNT, CANDIDATE_ID FROM CANDIDATE_TABLE WHERE NAME=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", CandidateName ?? "");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CandidateName = reader.IsDBNull(0) ? CandidateName : reader.GetString(0);
                            Constituency = reader.IsDBNull(1) ? Constituency : reader.GetString(1);
                            PartyName = reader.IsDBNull(2) ? PartyName : reader.GetString(2);
                            VoteCount = reader.IsDBNull(3) ? VoteCount : reader.GetInt32(3);
                            CandidateId = reader.IsDBNull(4) ? CandidateId : reader.GetString(4);
                        }
                    }
                }
            }
        }

        public void Savetotable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "INSERT INTO CANDIDATE_TABLE (NAME, CONSTITUENCY, PARTY, VOTE_COUNT, CANDIDATE_ID) VALUES (@name, @const, @party, @votes, @cid)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", CandidateName ?? "");
                    cmd.Parameters.AddWithValue("@const", Constituency ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@party", PartyName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@votes", VoteCount);
                    cmd.Parameters.AddWithValue("@cid", CandidateId ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Updatetotable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "UPDATE CANDIDATE_TABLE SET CONSTITUENCY=@const, PARTY=@party, VOTE_COUNT=@votes WHERE CANDIDATE_ID=@cid OR NAME=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@const", Constituency ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@party", PartyName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@votes", VoteCount);
                    cmd.Parameters.AddWithValue("@cid", CandidateId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@name", CandidateName ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Deletefromtable()
        {
            using (var conn = new SqlConnection(connection_string))
            {
                conn.Open();
                string query = "DELETE FROM CANDIDATE_TABLE WHERE CANDIDATE_ID=@cid OR NAME=@name";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", CandidateId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@name", CandidateName ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Display()
        {
            Console.WriteLine($"Candidate: {CandidateName} | Party: {PartyName} | Constituency: {Constituency} | Votes: {VoteCount} | ID: {CandidateId}");
        }


    }
}



