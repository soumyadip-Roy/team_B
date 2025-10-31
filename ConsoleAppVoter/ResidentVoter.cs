using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppVoter
{
    internal class ResidentVoter : Voter
    {
        public override void GetVoterId()
        {
            Console.WriteLine("Function 1");
        }
        public override string GetVoterName()
        {
            Console.WriteLine("Function 2");
            return "Hello";
        }
        public override int GetVoterStatus()
        {
            Console.WriteLine("Function 3");
            return 0;
        }

        public override string GetVoterConstituency()
        {
            Console.WriteLine("Function 4");
            return "hello";
        }
        public override string GetVoterPollingBooth()
        {
            Console.WriteLine("Function 5");
            return "hello";
        }

        public override int GetVoterAge(DateOnly birthDate)
        {
            Console.WriteLine("Function 6");
            return 0;
        }

        public ResidentVoter() { }
    }
}
