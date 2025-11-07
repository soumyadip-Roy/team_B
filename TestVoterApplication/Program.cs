namespace TestVoterApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ECElectionDayUI ec = new ECElectionDayUI();

            ec.ResetVotingRightsInDB();
            ec.VotingScreenUI();
            ec.DisplayElectionResults();
        }
    }
}
