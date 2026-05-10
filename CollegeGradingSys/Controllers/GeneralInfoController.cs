using CollegeGradingSys.Models;
using CollegeGradingSys.Services;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels.GeneralInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    
    public class GeneralInfoController : Controller
    {
        private readonly IGenericService<GeneralInfo> _generalInfoService;

        public GeneralInfoController(IGenericService<GeneralInfo> generalInfoService)
        {
            _generalInfoService = generalInfoService;
        }

        [Authorize(Roles = "Admin, Owner")]
        // GET: GeneralInfo/Index
        // هذه الدالة لعرض البيانات في الصفحة عند فتحها
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // جلب السجل الوحيد
            var generalInfos = await _generalInfoService.GetAllAsync();
            var generalInfo = generalInfos.FirstOrDefault();

            if (generalInfo == null)
            {
                // إذا كانت قاعدة البيانات فارغة تماماً (أول تشغيل للنظام)
                return View(new GeneralInfoVM());
            }

            var model = new GeneralInfoVM
            {
                Id = generalInfo.Id, // 💡 مهم جداً: يجب إرسال الـ Id للشاشة لكي نستخدمه عند الحفظ
                //BackupPath = generalInfo.BackupPath,
                BranchHeadName = generalInfo.BranchHeadName,
                StDepartmentHead = generalInfo.StDepartmentHead,
                SystemLicenseKey = generalInfo.SystemLicenseKey,
            };

            return View(model);
        }

        [Authorize(Roles = "Admin, Owner")]
        // POST: GeneralInfo/Index
        // هذه الدالة لاستقبال التعديلات عند الضغط على زر "حفظ" في نفس الصفحة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(GeneralInfoVM model)
        {
            if (!ModelState.IsValid)
            {
                // إذا كان هناك خطأ في الإدخال، نُعيد المستخدم لنفس الصفحة مع عرض الأخطاء
                return View(model);
            }

            try
            {
                // نجلب السجل الأصلي من الداتابيز باستخدام الـ Id
                var generalInfoVM = await _generalInfoService.GetByIdAsync(model.Id);

                // كإجراء احتياطي: إذا لم يجده بالـ Id، نجلب أول سجل موجود
                if (generalInfoVM == null)
                {
                    var allInfos = await _generalInfoService.GetAllAsync();
                    generalInfoVM = allInfos.FirstOrDefault();

                    if (generalInfoVM == null) return NotFound("السجل غير موجود.");
                }

                // تحديث البيانات
                //generalInfoVM.BackupPath = model.BackupPath;
                if(model.BranchHeadName != null) generalInfoVM.BranchHeadName = model.BranchHeadName;

                if (model.StDepartmentHead != null) generalInfoVM.StDepartmentHead = model.StDepartmentHead;
               
                if (model.SystemLicenseKey != null) generalInfoVM.SystemLicenseKey = model.SystemLicenseKey;
                

                // حفظ التعديلات
                await _generalInfoService.UpdateAsync(generalInfoVM);

                // إرسال رسالة نجاح للشاشة
                TempData["SuccessMessage"] = "تم حفظ الإعدادات بنجاح!";

                // إعادة تحميل الصفحة لرؤية التعديلات الجديدة
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }


        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Activation()
        {
            // إرسال البصمة للشاشة لكي يعرضها العميل
            ViewBag.MachineId = LicenseValidator.GetMachineId();
            // جلب السجل الوحيد
            var generalInfos = await _generalInfoService.GetAllAsync();
            var generalInfo = generalInfos.FirstOrDefault();

            if (generalInfo == null)
            {
                // إذا كانت قاعدة البيانات فارغة تماماً (أول تشغيل للنظام)
                return View(new GeneralInfoVM());
            }
          
            var model = new GeneralInfoVM
            {
                Id = generalInfo.Id, // 💡 مهم جداً: يجب إرسال الـ Id للشاشة لكي نستخدمه عند الحفظ
                //BackupPath = generalInfo.BackupPath,
              
                SystemLicenseKey = generalInfo.SystemLicenseKey,
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activation(GeneralInfoVM model)
        {
            // إرسال البصمة للشاشة لكي يعرضها العميل
            ViewBag.MachineId = LicenseValidator.GetMachineId();
            // 1. جلب السجل الحالي
            var generalInfos = await _generalInfoService.GetAllAsync();
            var generalInfoVM = generalInfos.FirstOrDefault();

            if (generalInfoVM == null)
                return NotFound();

            // 2. تحديث مفتاح الترخيص فقط
            generalInfoVM.SystemLicenseKey = model.SystemLicenseKey;
            await _generalInfoService.UpdateAsync(generalInfoVM);

            // 3. التحقق: هل المفتاح الجديد الذي تم حفظه صالح لهذا الجهاز؟
            if (LicenseValidator.IsLicenseValid(model.SystemLicenseKey))
            {
                // إذا كان صحيحاً، نرسله للصفحة الرئيسية للنظام
                TempData["SuccessMessage"] = "تم تفعيل النظام بنجاح! شكراً لك.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
               model.Id = generalInfoVM.Id;
                model.BranchHeadName = generalInfoVM.BranchHeadName;
                model.StDepartmentHead = generalInfoVM.StDepartmentHead;
                model.BackupPath = generalInfoVM.BackupPath;
                model.SystemLicenseKey = generalInfoVM.SystemLicenseKey;
                // إذا كان خاطئاً، نظهر له رسالة خطأ في نفس الشاشة
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "مفتاح التفعيل غير صحيح أو لا يطابق هذا الجهاز.");
                return View(model);
            }
        }
    }
}