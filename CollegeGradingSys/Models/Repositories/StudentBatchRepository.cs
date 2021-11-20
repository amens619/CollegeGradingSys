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
        public StudentBatch Add(StudentBatch newBatch)
        {
            var batch = studentBatches.FirstOrDefault();
            newBatch.Id = batch != null ? studentBatches.Max(b => b.Id) + 1 : 1;
            studentBatches.Add(newBatch);
            return newBatch;
        }

        public StudentBatch Delete(int id)
        {
            var batch = Find(id);
            if (batch != null)
            {
                studentBatches.Remove(batch);
            }
            return batch;
        }

        public StudentBatch Find(int id)
        {
            return studentBatches.SingleOrDefault(a => a.Id == id);
        }

        public IList<StudentBatch> List()
        {
            return studentBatches;
        }

        public StudentBatch Update(int id, StudentBatch newBatch)
        {
            var oldBatch = Find(id);
            if (oldBatch != null)
            {
                oldBatch.StudentBatchName = newBatch.StudentBatchName;
                oldBatch.Note = newBatch.Note;
            }
            return newBatch;
        }
    }
}
