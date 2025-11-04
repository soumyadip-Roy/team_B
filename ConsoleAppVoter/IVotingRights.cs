using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppVoter
{
    interface IVotingRights
    {
        void RegisterVoteInDB(string name, int age, string constituency, string candidate, string voter_id, bool isInBlacklist);
        void ResetVotingStatusinDB(string voter_id);
    }
}
