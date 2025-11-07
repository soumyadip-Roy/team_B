using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voting_system
{
    public static class ElectionCommission
    {



        static string connection_string = "server = LAPTOP-MI8QQQVT;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";
        static SqlConnection conn_voter = new SqlConnection(connection_string) { };

        public static int Tvotes = 0;

        public static List<Candidate> Candidates = new List<Candidate>();

        public static List<Party> Parties = new List<Party>();

        static void ConnectToDatabase()
        {
            conn_voter.Open();
        }

        static void DisconnectFromDatabase()
        {
            conn_voter.Close();
        }

        public static void LoadCandidates(List<Candidate> candidate_list)
        {
            try
            {
                ConnectToDatabase();
                var sqlQuery = "Select * from CANDIDATE_TABLE";
                using(SqlCommand command = new SqlCommand(sqlQuery, conn_voter))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var candidate_name = reader.GetString(0);
                                var candidate_constituency = reader.GetString(1);
                                var candidate_party = reader.GetString(2);
                                var candidate_votes = reader.GetInt16(3);
                                var candidate_partySymbol = "";
                                // Add the leader column to the table. 
                                var candidate_leader = "";
                                sqlQuery = "Select * from PARTY_TABLE where NAME = @party";
                                using (SqlCommand command_read = new SqlCommand(sqlQuery, conn_voter))
                                {
                                    command_read.Parameters.AddWithValue("@party", candidate_party);
                                    using(SqlDataReader reader_party = command_read.ExecuteReader())
                                    {
                                        if (reader_party.HasRows)
                                        {
                                            if (reader_party.Read())
                                            {
                                                candidate_partySymbol = reader_party.GetString(2);
                                                //candidate_leader = reader_party.GetString(4);
                                            }
                                        }
                                    }
                                }

                                Candidate temp_cand_creation = new Candidate(candidate_name, candidate_party, candidate_partySymbol, candidate_leader, candidate_constituency);
                                candidate_list.Add(temp_cand_creation);

                            }
                        }
                    }
                }
            }
            catch(Exception E)
            {
                Console.WriteLine(E.ToString());
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        public static void LoadParties()
        {
            ConnectToDatabase();
            try
            {
                var sqlquery = "Select * from PARTY_TABLE";
                using(SqlCommand command = new SqlCommand(sqlquery, conn_voter))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var party_name = reader.GetString(0);
                                var party_symbol = reader.GetString(1);
                                var party_leader = "";
                                //var party_leader = reader.GetString(3);
                                Party temp_party_creation = new Party(party_name, party_symbol, party_leader);
                                Parties.Add(temp_party_creation);
                            }
                        }
                    }
                }
            }
            catch(Exception E)
            {
                Console.WriteLine(E.ToString());
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        public static void Updatevotes()
            {
                

                foreach (var cand in Candidates)
                {
                    cand.Updatetotable();  
                    Tvotes += cand.VoteCount;
                }
                
            }


        public static void DisplayResults()
        {
            Console.WriteLine("---------------- Results -----");

            foreach (var cand in Candidates)
            {
                cand.Displaycand();
                Console.WriteLine("---------------------------------");
            }

            Console.WriteLine($"\nTotal Votes Cast: {Tvotes}");

            Console.WriteLine("---------------------------------");

        }
        public static void Displaypwinners()
        {
            Console.WriteLine("\n----- Party-wise Winners -----");
            foreach (var p in Parties)
            {
                p.winner();
            }
        }

        public static void winningparty()
        {

            Console.WriteLine("---------winner---------");

            if (Parties.Count == 0)
            {
                Console.WriteLine("zero partie");
                return;
            }

            
            foreach (var party in Parties)
            {
                party.Tpartyvotes(); 
            }

            
            var sorted = Parties.OrderByDescending(p => p.VoteCount).ToList();

            var winnerParty = sorted.First();
            Party runnerUpParty = null;
            if (sorted.Count > 1)
            {
                runnerUpParty = sorted[1];
            }

            Console.WriteLine($"\nWinning Party: {winnerParty.PartyName}");
            Console.WriteLine($"Total Votes: {winnerParty.VoteCount}");

            if (runnerUpParty != null)
            {
                int majority = winnerParty.VoteCount - runnerUpParty.VoteCount;
                Console.WriteLine($"Majoriity over {runnerUpParty.PartyName}: {majority} votes");
            }
            else
            {
                Console.WriteLine("No runner-up");
            }


            Console.WriteLine("------------------------------------\n");



        }


        /*

        public static void winner()
        {
            if (Members.Count == 0)
            {
                Console.WriteLine($"No candidates in {PartyName}");
                return;
            }

            var win = Members.OrderByDescending(m => m.VoteCount).First();
            Console.WriteLine($"Winner from {PartyName}: {win.CandidateName} ({win.Constituency}) with {win.VoteCount} votes.");
        }
        */

    }
    }
