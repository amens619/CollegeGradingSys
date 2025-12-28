using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Repositories.Implementations
{
    public class StAcademicDataRepository : ICollegeGradingSysRepository<StAcademicData>
    {
        IList<StAcademicData> stPersonalDatas;
        public StAcademicDataRepository()
        {
            //stPersonalDatas = new List<StAcademicData>()
            //{
            //    new StAcademicData { AcademicID=40030045, StName ="امين سالم محمد باشعيب", BirthDate= new DateTime(2000,12,24), Birthcountry=new Nationality{ Id=1 } , Sex= Sex.ذكر , IdentificatioNO="0755556644", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "حضرموت" } , StAcademicDatas =new List<StAcademicData>()
            //{
            //    new StAcademicData { Id=1,  StStatus = StStatus.مقيد , StLevel = Level.الثاني},
            //    new StAcademicData { Id=2,   StStatus =   StStatus.منحسب ,StLevel = Level.الثالث}

            //} },

            //      new StAcademicData { AcademicID=40030046, StName ="طالب احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030047, StName ="علي احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030048, StName ="محمد احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030049, StName ="صالح احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030050, StName ="موسى احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030051, StName ="خالد احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030052, StName ="عبدالخالق احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030053, StName ="رائد احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030054, StName ="سحر احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.انثى , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030055, StName ="شهد احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.انثى , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030056, StName ="سمير احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030057, StName ="عوض احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030058, StName ="يعقوب احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030059, StName ="فوزية احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.انثى , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030060, StName ="ناصر احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030061, StName ="صلاح احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030062, StName ="ايوب احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030063, StName ="انس احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030064, StName ="طارق احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030065, StName ="وهيب احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
            //      new StAcademicData { AcademicID=40030066, StName ="عمر احمد الجابري", BirthDate= new DateTime(2001,5,14), Birthcountry=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , BirthGovernorate = new Governorate { Id=1 , GovernorateName= "المهرة" }}


            //};
        }
        public StAcademicData Add(StAcademicData entity)
        {
            stPersonalDatas.Add(entity);
            return entity;
        }

        public StAcademicData Delete(int academicID)
        {
            var stPersonalData = Find(academicID);
            if (stPersonalData != null)
            {
                stPersonalDatas.Remove(stPersonalData);
            }
            return stPersonalData;
            
        }

        public StAcademicData Find(int id)
        {
            return stPersonalDatas.SingleOrDefault(a => a.Id == id);
        }

        public IList<StAcademicData> List()
        {
            return stPersonalDatas;
        }

        public StAcademicData Update(int academicID, StAcademicData newStAcademicData)
        {
            var oldStAcademicData = Find(academicID);
            if (oldStAcademicData != null)
            {
                oldStAcademicData.IsTerm = newStAcademicData.IsTerm;
                oldStAcademicData.StLevel = newStAcademicData.StLevel;
                oldStAcademicData.StPersonalData = newStAcademicData.StPersonalData;
                oldStAcademicData.StStatus = newStAcademicData.StStatus;
                //oldStAcademicData.Batch = newStAcademicData.Batch;
                oldStAcademicData.Term = newStAcademicData.Term;
                oldStAcademicData.GPA = newStAcademicData.GPA;
                oldStAcademicData.Average = newStAcademicData.Average;
                //oldStAcademicData.AcademicYear = newStAcademicData.AcademicYear;
            }
            return newStAcademicData;
        }
    }
}
