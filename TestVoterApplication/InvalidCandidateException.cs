using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVoterApplication
{
    class InvalidCandidateException: Exception
    {
        public InvalidCandidateException() {
            Console.WriteLine("<==========INVALID_CANDIDATE_CHOICE==========>");
        }
    }
}
