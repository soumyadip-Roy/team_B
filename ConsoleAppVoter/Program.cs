namespace ConsoleAppVoter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NRIVoter one = new NRIVoter();
            var name = "SR_TEST";
            //DateOnly dob = new DateOnly(2002,02,01);
            var voter_id = "TEST1234TEST";
            
            var constituency = "SR_TEST1234000";
            //var voting_status = 1;
            //string name, DateOnly date_of_birth, string voter_id, string constituency, string polling_booth, int voting_status
            //var name = one.GetVoterName();
            //var constituency = one.GetVoterConstituency();

            //one.GetVoterId(name,constituency);
            //one.GetVoterPollingBooth(voter_id);
            var age = one.GetVoterAge(voter_id);
            //var status = one.GetVoterStatus(age, false, voter_id);
            one.ResetVotingStatusinDB(voter_id);
            one.RegisterVoteInDB(name, age, constituency, "SR_TEST_CANDIDATE", voter_id, false);
        }
    }
}
