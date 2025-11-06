using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppVoter
{
    interface IVotingRights
    {
        void CastVote();
        void ResetVotingStatusinDB(string voter_id);

        void ViewVoterDetails();
    }
}
