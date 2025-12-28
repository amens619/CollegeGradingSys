using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class BatchRepository : ICollegeGradingSysRepository<Batch>
    {
        IList<Batch> studentBatches;
        public BatchRepository()
        {
            studentBatches = new List<Batch>()
            {
                new Batch { Id=1, BatchName="2021-2022 فقه" , Note =""  },
                new Batch { Id=2, BatchName="2022-2023 فقه" , Note ="" }

            };
        }
        public Batch Add(Batch newBatch)
        {
            var batch = studentBatches.FirstOrDefault();
            newBatch.Id = batch != null ? studentBatches.Max(b => b.Id) + 1 : 1;
            studentBatches.Add(newBatch);
            return newBatch;
        }

        public Batch Delete(int id)
        {
            var batch = Find(id);
            if (batch != null)
            {
                studentBatches.Remove(batch);
            }
            return batch;
        }

        public Batch Find(int id)
        {
            return studentBatches.SingleOrDefault(a => a.Id == id);
        }

        public IList<Batch> List()
        {
            return studentBatches;
        }

        public Batch Update(int id, Batch newBatch)
        {
            var oldBatch = Find(id);
            if (oldBatch != null)
            {
                oldBatch.BatchName = newBatch.BatchName;
                oldBatch.Note = newBatch.Note;
                //oldBatch.AcademicYear = newBatch.AcademicYear;
            }
            return newBatch;
        }
    }
}
