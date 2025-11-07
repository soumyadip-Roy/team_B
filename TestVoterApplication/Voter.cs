using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVoterApplication
{
    internal abstract class Voter : IVotingRights
    {
        public abstract void SetVoterDetails(string name, DateOnly date_of_birth, string voter_id, string constituency, string polling_booth, int voting_status);
        public abstract string GetVoterId(string name, string constituency);
        public abstract string GetVoterName();
        public abstract int GetVoterStatus(int age, bool isInBlacklist, string voter_id);
        public abstract string GetVoterConstituency();
        public abstract string GetVoterPollingBooth(string voter_id);
        public abstract int GetVoterAge(string voter_id);
        public abstract void CastVote();
        public abstract void ResetVotingStatusinDB(string voter_id);
        public abstract void ViewVoterDetails();


    }
}
