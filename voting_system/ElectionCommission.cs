using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voting_system
{
    public static class ElectionCommission
    {


        

            public static int Tvotes = 0;

            public static List<Candidate> Candidates = new List<Candidate>();

        public static List<Party> Parties = new List<Party>();




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
