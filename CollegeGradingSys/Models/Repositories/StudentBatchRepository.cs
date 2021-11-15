using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class StudentBatchRepository : ICollegeGradingSysRepository<StudentBatch>
    {
        IList<StudentBatch> studentBatches;
        public StudentBatchRepository()
        {
            studentBatches = new List<StudentBatch>()
            {
                new StudentBatch { Id=1, StudentBatchName="2021-2022" , Note ="" },
                new StudentBatch { Id=2, StudentBatchName="2022-2023" , Note ="" }

            };
        }
        public void Add(StudentBatch newBatch)
        {
            newBatch.Id = studentBatches.Max(a => a.Id) + 1;
            studentBatches.Add(newBatch);
        }

        public void Delete(int id)
        {
            var Batch = Find(id);
            studentBatches.Remove(Batch);
        }

        public StudentBatch Find(int id)
        {
            return studentBatches.SingleOrDefault(a => a.Id == id);
        }

        public IList<StudentBatch> List()
        {
            return studentBatches;
        }

        public void Update(int id, StudentBatch newBatch)
        {
            var oldBatch = Find(id);
            oldBatch.StudentBatchName = newBatch.StudentBatchName;
            oldBatch.Note = newBatch.Note;
        }
    }
}
