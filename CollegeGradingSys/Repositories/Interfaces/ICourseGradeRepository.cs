using CollegeGradingSys.Models;
using CollegeGradingSys.Utilities.Exceptions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Interfaces
{
    public interface ICourseGradeRepository : IRepository<CourseGrade>
    {
        IQueryable<CourseGrade> Query(); // للاستعلامات المرنة
        Task<CourseGrade> FindWithRelationsAsync(int id);
        //Task UpdateAsync(CourseGrade entity);
        Task<List<CourseGrade>> GetGradesForBatchAsync(int batchId, int courseId);      
        
    }
}
