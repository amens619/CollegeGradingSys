using CollegeGradingSys.Models;
using CollegeGradingSys.Services;
using CollegeGradingSys.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Filters
{
    // 💡 تم تغيير الواجهة إلى IAsyncActionFilter
    public class LicenseCheckFilter : IAsyncActionFilter
    {
        private readonly IConfiguration _config;
        private readonly IGenericService<GeneralInfo> _generalInfoService;

        public LicenseCheckFilter(IConfiguration config, IGenericService<GeneralInfo> generalInfoService)
        {
            _config = config;
            _generalInfoService = generalInfoService;
        }

        // 💡 تم تغيير اسم الدالة وإضافة المعامل 'next'
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // جلب اسم الأكشن (الدالة) الحالي
            var actionName = context.RouteData.Values["action"]?.ToString();

            // السماح بالمرور إذا كان المستخدم متجهاً لدالة "Activation"
            if (actionName != null && actionName.Equals("Activation", StringComparison.OrdinalIgnoreCase))
            {
                await next(); // 👈 معناها: اسمح بفتح الشاشة وأكمل التنفيذ
                return;
            }

            // جلب بيانات الترخيص من الداتابيز
            var generalInfos = await _generalInfoService.GetAllAsync();
            var generalInfo = generalInfos.FirstOrDefault();

            if (generalInfo != null && !string.IsNullOrWhiteSpace(generalInfo.SystemLicenseKey))
            {
                // فحص المفتاح
                string savedLicenseKey = generalInfo.SystemLicenseKey;

                if (!LicenseValidator.IsLicenseValid(savedLicenseKey))
                {
                    // توجيهه إلى شاشة التفعيل
                    context.Result = new RedirectToActionResult("Activation", "GeneralInfo", null);
                    return; // 👈 نوقف التنفيذ هنا ولا نستدعي next
                }
            }
            else
            {
                // إذا لم يجد مفتاحاً في الداتابيز أصلاً
                context.Result = new RedirectToActionResult("Activation", "GeneralInfo", null);
                return;
            }

            // إذا وصل الكود إلى هنا، فهذا يعني أن الترخيص سليم 100%
            await next(); // 👈 اسمح بفتح الشاشة التي طلبها المستخدم
        }
    }
}