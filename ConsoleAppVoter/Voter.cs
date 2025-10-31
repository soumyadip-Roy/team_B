using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppVoter
{
    internal abstract class Voter
    {
        public abstract void GetVoterId();
        public abstract string GetVoterName();
        public abstract int GetVoterStatus();
        public abstract string GetVoterConstituency();
        public abstract string GetVoterPollingBooth();
        public abstract int GetVoterAge(DateOnly birthDate);


    }
}
