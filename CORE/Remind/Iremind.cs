using System;
namespace Candidate_Management.CORE.Remind
{
    public interface Iremind
    {
         void add(int id,DateTime date);
         void update(int id,DateTime date);
         void exec(int idJob,string fileName,string candidateName,string Candidatefirstname, DateTime meeting);
    }
}