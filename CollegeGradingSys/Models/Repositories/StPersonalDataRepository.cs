using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Models.Repositories
{
    public class StPersonalDataRepository : ICollegeGradingSysRepository<StPersonalData>
    {
        IList<StPersonalData> stPersonalDatas;
        public StPersonalDataRepository()
        {
            stPersonalDatas = new List<StPersonalData>()
            {
                new StPersonalData { AcademicID=40030045, StName ="امين سالم محمد باشعيب", BirthDate= new DateTime(2000,12,24), BirthPlace="اليمن حضرموت" , College=new College{ Id=1 }, Sex= Sex.ذكر , IdentificatioNO="0755556644", Nationality= new Nationality{ Id=1 } , EnrollmentYearH=1442 , EnrollmentYearM=2019 , Governorate = oldGovernorate.حضرموت  }
            };
        }
        public StPersonalData Add(StPersonalData entity)
        {
            stPersonalDatas.Add(entity);
            return entity;
        }

        public StPersonalData Delete(int academicID)
        {
            var stPersonalData = Find(academicID);
            if (stPersonalData != null)
            {
                stPersonalDatas.Remove(stPersonalData);
            }
            return stPersonalData;
            
        }

        public StPersonalData Find(int academicID)
        {
            return stPersonalDatas.SingleOrDefault(a => a.AcademicID == academicID);
        }

        public IList<StPersonalData> List()
        {
            return stPersonalDatas;
        }

        public StPersonalData Update(int academicID, StPersonalData newStPersonalData)
        {
            var oldStPersonalData = Find(academicID);
            if (oldStPersonalData != null)
            {
                oldStPersonalData.StName = newStPersonalData.StName;
                oldStPersonalData.Governorate = newStPersonalData.Governorate;
                oldStPersonalData.IdentificatioNO = newStPersonalData.IdentificatioNO;
                oldStPersonalData.Nationality = newStPersonalData.Nationality;
                oldStPersonalData.BirthDate = newStPersonalData.BirthDate;
                oldStPersonalData.BirthPlace = newStPersonalData.BirthPlace;
                oldStPersonalData.College = newStPersonalData.College;
                oldStPersonalData.EnrollmentYearH = newStPersonalData.EnrollmentYearH;
                oldStPersonalData.EnrollmentYearM = newStPersonalData.EnrollmentYearM;
                oldStPersonalData.Sex = newStPersonalData.Sex;
            }
            return newStPersonalData;
        }
    }
}
