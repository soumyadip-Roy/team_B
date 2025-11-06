using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppVoter
{
    internal class AlreadyVotedException:Exception
    {
        public AlreadyVotedException(string name):base(name)
        {

            Console.WriteLine("<==========MULTIPLE_VOTING_ATTEMPT==========>");
            Console.WriteLine($"{name} has voted once before, please contact EC in case of discripancies.");
            Console.WriteLine("EC Helpline Number: 8989898989");
        }
    }
}
