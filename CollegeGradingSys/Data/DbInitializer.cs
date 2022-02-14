using CollegeGradingSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Nationality.Any())
            {
                return;   // DB has been seeded
            }

            var Nationalities = new Nationality[]
            {
            new Nationality{ NationalityName="يمني",CountryName="اليمن"},
            new Nationality{NationalityName="ماليزي",CountryName="ماليزي"},
            new Nationality{NationalityName="صومالي",CountryName="الصومال"},
            new Nationality{NationalityName="سوداني",CountryName="السودان"},
            new Nationality{NationalityName="سوري",CountryName="سوريا"},
            new Nationality{NationalityName="لبناني",CountryName="لبنان"},
            new Nationality{NationalityName="سعودي",CountryName="السعودية"},
            new Nationality{NationalityName="عماني",CountryName="عمان"},
            new Nationality{NationalityName="بحريني",CountryName="البحرين"},
            new Nationality{NationalityName="مصري",CountryName="مصر"},
            new Nationality{NationalityName="كويتي",CountryName="الكويت"},
            new Nationality{NationalityName="تونسي",CountryName="تونس"},
            new Nationality{NationalityName="جزائري",CountryName="الجزائر"},
            new Nationality{NationalityName="اثيوبي",CountryName="اثيوبيا"},
            new Nationality{NationalityName="هندي",CountryName="الهند"},
            new Nationality{NationalityName="اندنوسي",CountryName="اندنوسيا"},
            new Nationality{NationalityName="موريتاني",CountryName="موريتانا"},
            new Nationality{NationalityName="فلسطيني",CountryName="فلسطين"},
            new Nationality{NationalityName="ليبي",CountryName="ليبيا"},
            new Nationality{NationalityName="اماراتي",CountryName="الامارات"}
            };
            foreach (Nationality n in Nationalities)
            {
                context.Nationality.Add(n);
            }
            context.SaveChanges();

            var Governorates = new Governorate[]
           {
            new Governorate{GovernorateName="حضرموت", Nationality = Nationalities.Single(g => g.CountryName == "اليمن") },
            new Governorate{GovernorateName="شبوة",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="عمران",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="مأرب",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="عدن",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="البيضاء",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="الحديدة",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="الجوف",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="المحويت",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="أمانة العاصمة",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="ذمار",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="حجة",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="إب",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="ريمة",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="صعدة",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="صنعاء",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="تعز",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="أبين",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="تعز",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="أرخيب سقطرى",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="لحج",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")},
            new Governorate{GovernorateName="المهرة",Nationality = Nationalities.Single(g => g.CountryName == "اليمن")}
           };
            foreach (Governorate n in Governorates)
            {
                context.Governorate.Add(n);
            }
            context.SaveChanges();

            var Districts = new District[]
           {
            new District{DistrictName="الريدة وقصيعر", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="السوم", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="الديس", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="الشحر", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="الضليعه", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
             new District{DistrictName="العبر", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="القطن", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="القف", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="المكلا", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="بروم ميفع", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
             new District{DistrictName="تريم", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="ثمود", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="حجر", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="حجر الصيعر", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="حديبو", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
             new District{DistrictName="حريضة", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="دوعن", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="رخيه", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="رماه", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="زمنخ ومنوخ", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
             new District{DistrictName="ساه", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="سيئون", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="شبام", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="غيل باوزير", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="غيل بن يمين", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
             new District{DistrictName="قلنسية وعبد الكوري", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="مدينة المكلا", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="وادي العين", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
             new District{DistrictName="يبعث", Governorate=Governorates.Single(g => g.GovernorateName == "حضرموت")},
            new District{DistrictName="عرماء", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="مرخة", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="دهر", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="جردان", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="عسيلان", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="مرخة", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="عتق", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="نصاب", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")},
            new District{DistrictName="بيحان", Governorate=Governorates.Single(g => g.GovernorateName == "شبوة")}
           };
            foreach (District district in Districts)
            {
                context.District.Add(district);
            }
            context.SaveChanges();


            var College = new College { CollegeName = "الشريعة" };
            context.College.Add(College);
            context.SaveChanges();

            var Department = new Department { DepartmentName = "فقه", College = College };
            context.Department.Add(Department);
            context.SaveChanges();


            var Specialization = new Specialization { SpecializationName = "عام", Department = Department };
            context.Specialization.Add(Specialization);
            context.SaveChanges();




            var courses = new Course[]
            {
            new Course{ CourseName="تفسير (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="حديث (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="نحو (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="أصول دعوة (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="إيمان (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="تجويد (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="القران حفظ (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="القران تلاوة (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="تزكية (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="تطبيق عملي (101)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="تفسير (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="حديث (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="نحو (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="أصول فقه (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="تزكية (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="تجويد (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="القران حفظ (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="القران تلاوة (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="فقه (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="المدخل لدارسة الشريعة (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="تزكية عملية (102)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الأول , Term= Term.الثاني , IsSubCourse=false},
            new Course{ CourseName="تفسير (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
            new Course{ CourseName="نحو (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="مصطلح (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="إعجاز (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="فقه (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="سيرة (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="طرق تدريس (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="القران حفظ (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="القران تلاوة (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="التزكية (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تطبيق عملي (201)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تفسير (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="حديث (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="فرائض (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تزكية (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="إيمان (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="أصول فقه (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="أصول دعوة (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="القران حفظ (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="القران تلاوة (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تزكية عملية (202)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثاني , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تفسير (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="نحو (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="حديث (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="أصول فقه (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="فقه (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="إعجاز (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="مناهج بحث (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="قران كريم (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تزكية (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="علوم القران (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تطبيق عملي (301)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تفسير (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="نحو (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="سيرة (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="مصطلح حديث (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="فقه (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="إيمان (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تاريخ الخلافة الاسلامية (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="قران كريم (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="الفرائض (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تزكية (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="التزكية العملية (302)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الثالث , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تفسير (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="حديث (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="أصول فقه (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="إعجاز (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="قران كريم (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تزكية (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="مدخل إلى علم السياسة (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="التدبير المنزلي (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="مدخل إلى علم الاقتصاد (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="آيات أحكام (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="نظرات في الشريعة والقوانين (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تطبيق عملي (401)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الأول , IsSubCourse=false},
           new Course{ CourseName="تفسير (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="نحو (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="إيمان (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="قواعد فقهية (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="مصطلح حديث (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="قران كريم (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تزكية (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="علوم قران (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="حاضر العالم الاسلامي (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="آيات أحكام (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="نظرات في الشريعة والقوانين (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
           new Course{ CourseName="تزكية عملية (402)", Specialization=Specialization , BigGrade= 100 , SmallGrade=60, Level= Level.الرابع , Term= Term.الثاني , IsSubCourse=false},
            };
            foreach (Course c in courses)
            {
                context.Course.Add(c);
            }
            context.SaveChanges();

            //var SubCourses = new SubCourse[]
            //{
            // new SubCourse{  SubCourseName="الحلية" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName == "تزكية (101)")},
            // new SubCourse{  SubCourseName="المستخلص" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName == "تزكية (101)")},
            // new SubCourse{  SubCourseName="الحلية" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName == "تزكية (102)")},
            // new SubCourse{  SubCourseName="المستخلص" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName == "تزكية (102)")},
            //new SubCourse{  SubCourseName="التذكرة" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName == "التزكية (201)")},
            // new SubCourse{  SubCourseName="المستخلص" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName =="التزكية (201)")},
            // new SubCourse{  SubCourseName="التذكرة" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName == "تزكية (202)")},
            // new SubCourse{  SubCourseName="المستخلص" , BigGrade= 100 , SmallGrade=60 , Course=courses.Single(g => g.CourseName =="تزكية (202)")},

            //};
            //foreach (SubCourse subCourse in SubCourses)
            //{
            //    context.SubCourse.Add(subCourse);
            //}
            //context.SaveChanges();

            //    var enrollments = new Enrollment[]
            //    {
            //    new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            //    new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            //    new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            //    new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            //    new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            //    new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            //    new Enrollment{StudentID=3,CourseID=1050},
            //    new Enrollment{StudentID=4,CourseID=1050},
            //    new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            //    new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            //    new Enrollment{StudentID=6,CourseID=1045},
            //    new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            //    };
            //    foreach (Enrollment e in enrollments)
            //    {
            //        context.Enrollments.Add(e);
            //    }
            //    context.SaveChanges();
            //}
        }
    }
}
