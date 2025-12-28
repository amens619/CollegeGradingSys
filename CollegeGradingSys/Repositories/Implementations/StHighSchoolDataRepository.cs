using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class StHighSchoolDataRepository : ICollegeGradingSysRepository<StHighSchoolData>
    {
        IList<StHighSchoolData> stHighSchoolDatas;
        public StHighSchoolDataRepository()
        {
            stHighSchoolDatas = new List<StHighSchoolData>()
            {
                new  StHighSchoolData{ AcademicID=40030046,  SeatNo=141255,  CertificateType=  CertificateType.علمي, CertificateYear=2020 , Source= "اليمن", Average=95.5f , HighSchoolName="عمر بن الخطاب"  }
            };
        }
        public StHighSchoolData Add(StHighSchoolData entity)
        {                    
            stHighSchoolDatas.Add(entity);
            return entity;
        }

        public StHighSchoolData Delete(int id)
        {
            var stHighSchoolData = Find(id);
            if (stHighSchoolData != null)
            {
                stHighSchoolDatas.Remove(stHighSchoolData);
            }
            return stHighSchoolData;
        }

        public StHighSchoolData Find(int id)
        {
           return stHighSchoolDatas.SingleOrDefault(a => a.AcademicID == id);
        }

        public IList<StHighSchoolData> List()
        {
            return stHighSchoolDatas;
        }

        public StHighSchoolData Update(int id, StHighSchoolData newStHighSchoolData)
        {
           var oldStHighSchoolData = Find(id);
            if (oldStHighSchoolData != null)
            {
                oldStHighSchoolData.Average = newStHighSchoolData.Average;
                oldStHighSchoolData.CertificateType = newStHighSchoolData.CertificateType;
                oldStHighSchoolData.CertificateYear = newStHighSchoolData.CertificateYear;
                oldStHighSchoolData.HighSchoolName = newStHighSchoolData.HighSchoolName;
                oldStHighSchoolData.Note = newStHighSchoolData.Note;
                oldStHighSchoolData.SeatNo = newStHighSchoolData.SeatNo;
                oldStHighSchoolData.Source = newStHighSchoolData.Source;              
            }
            return newStHighSchoolData;
        }
    }
}
