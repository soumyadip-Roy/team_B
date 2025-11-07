using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVoterApplication
{




    public static class ElectionCommission
    {
        private static readonly string connectionString = "server = LAPTOP-MDVNPCGM;database=VOTER_PROJECT;TrustServerCertificate=Yes;Trusted_Connection=True";

        public static List<Candidate> Candidates { get; } = new List<Candidate>();
        public static List<Party> Parties { get; } = new List<Party>();
        public static int TotalVotes { get; private set; } = 0;

        public static void LoadCandidatesFromDb()
        {
            Candidates.Clear();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT NAME, CONSTITUENCY, PARTY, VOTE_COUNT, CANDIDATE_ID FROM CANDIDATE_TABLE";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var c = new Candidate
                            {
                                CandidateName = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                Constituency = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                PartyName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                VoteCount = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                CandidateId = reader.IsDBNull(4) ? "" : reader.GetString(4)
                            };
                            Candidates.Add(c);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in LoadCandidatesFromDb: " + e);
            }
        }

        public static void LoadPartiesFromDb()
        {
            Parties.Clear();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT NAME, LOGO, LEADER FROM PARTY_TABLE";
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var p = new Party
                            {
                                PartyName = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                PartySymbol = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                LeaderName = reader.IsDBNull(2) ? "" : reader.GetString(2)
                            };
                            Parties.Add(p);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in LoadPartiesFromDb: " + e);
            }
        }

        public static void AssignCandidatesToParties()
        {
            foreach (var p in Parties)
                p.Members.Clear();

            foreach (var c in Candidates)
            {
                var party = Parties.FirstOrDefault(p => string.Equals(p.PartyName, c.PartyName, StringComparison.OrdinalIgnoreCase));
                if (party == null)
                {
                    party = new Party(c.PartyName, "", "");
                    Parties.Add(party);
                }
                party.Members.Add(c);
            }
        }

        public static void UpdateVotesToDb()
        {
            TotalVotes = 0;
            foreach (var c in Candidates)
            {
                try
                {
                    c.Updatetotable();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error updating candidate {c.CandidateName}: {e}");
                }
                TotalVotes += c.VoteCount;
            }
        }

        public static void RecalculatePartyVotes()
        {
            foreach (var p in Parties)
                p.RecalculateVotes();
        }

        public static void DisplayResults()
        {
            Console.WriteLine("===== ELECTION RESULTS =====");
            foreach (var p in Parties.OrderByDescending(p => p.VoteCount))
            {
                p.Display();
                Console.WriteLine("----------------------------------");
            }
            Console.WriteLine($"TOTAL VOTES (sum of candidate votes): {TotalVotes}");
        }

        public static void DisplayOverallWinner()
        {
            RecalculatePartyVotes();
            var ordered = Parties.OrderByDescending(p => p.VoteCount).ToList();
            if (ordered.Count == 0)
            {
                Console.WriteLine("No parties loaded.");
                return;
            }

            var winner = ordered.First();
            var runnerUp = ordered.Count > 1 ? ordered[1] : null;
            Console.WriteLine($"Winning party: {winner.PartyName} with {winner.VoteCount} votes.");
            if (runnerUp != null)
            {
                Console.WriteLine($"Runner up: {runnerUp.PartyName} with {runnerUp.VoteCount} votes. Majority: {winner.VoteCount - runnerUp.VoteCount}");
            }
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
    
