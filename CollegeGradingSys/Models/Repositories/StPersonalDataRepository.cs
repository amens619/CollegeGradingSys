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
                new StPersonalData { AcademicID=40030045, StName ="امين سالم محمد باشعيب", BirthDate= new DateTime(2000,12,24), BirthPlace=new Nationality{ Id=1 } , Sex= Sex.ذكر , IdentificatioNO="0755556644", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "حضرموت" } , StAcademicDatas =new List<StAcademicData>()
            {
                new StAcademicData { Id=1, Specialization=new Specialization{ Id=1 , SpecializationName= "فقه" }, StStatus = StStatus.مقيد , StLevel = Level.الثاني},
                new StAcademicData { Id=2, Specialization=new Specialization{ Id=1 , SpecializationName= "فقه" },  StStatus =   StStatus.منحسب ,StLevel = Level.الثالث}

            } },

                  new StPersonalData { AcademicID=40030046, StName ="طالب احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030047, StName ="علي احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030048, StName ="محمد احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030049, StName ="صالح احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030050, StName ="موسى احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030051, StName ="خالد احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030052, StName ="عبدالخالق احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030053, StName ="رائد احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030054, StName ="سحر احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.انثى , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030055, StName ="شهد احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.انثى , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030056, StName ="سمير احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030057, StName ="عوض احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030058, StName ="يعقوب احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030059, StName ="فوزية احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.انثى , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030060, StName ="ناصر احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030061, StName ="صلاح احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030062, StName ="ايوب احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030063, StName ="انس احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030064, StName ="طارق احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030065, StName ="وهيب احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }},
                  new StPersonalData { AcademicID=40030066, StName ="عمر احمد الجابري", BirthDate= new DateTime(2001,5,14), BirthPlace=new Nationality{ Id=2 } , Sex= Sex.ذكر , IdentificatioNO="0798888822", Nationality= new Nationality{ Id=1 } , EnrollmentYearH="1442" , EnrollmentYearM="2019" , Governorate = new Governorate { Id=1 , GovernorateName= "المهرة" }}


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
                oldStPersonalData.EnrollmentYearH = newStPersonalData.EnrollmentYearH;
                oldStPersonalData.EnrollmentYearM = newStPersonalData.EnrollmentYearM;
                oldStPersonalData.Sex = newStPersonalData.Sex;
            }
            return newStPersonalData;
        }
    }
}
