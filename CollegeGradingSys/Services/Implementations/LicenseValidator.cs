using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace CollegeGradingSys.Services
{
    public static class LicenseValidator
    {
        // 🛑 السر الأكبر: الكلمة السرية (يجب أن تكون متطابقة تماماً مع الموجودة في برنامج الـ Keygen لديك)
        private const string SECRET_SALT = "AbuRami_CollegeGradingSys_CodeRysh_418_Key_2026!";

        /// <summary>
        /// الدالة التي يستخدمها الفلتر لمعرفة هل الترخيص ساري وصحيح أم لا
        /// </summary>
        public static bool IsLicenseValid(string savedLicenseKey)
        {
            try
            {
                // 1. إذا كان المفتاح المحفوظ فارغاً، فالنظام غير مفعل
                if (string.IsNullOrWhiteSpace(savedLicenseKey)) return false;

                // 2. قراءة بصمة الجهاز الحالي الذي يعمل عليه النظام
                string currentMachineId = GetMachineId();
                if (currentMachineId == "ERROR-ID") return false;

                // 3. إعادة حساب المفتاح (دمج البصمة مع الكلمة السرية وتشفيرها)
                string rawData = $"{currentMachineId}-{SECRET_SALT}";
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    string hex = BitConverter.ToString(hash).Replace("-", "");

                    // توليد المفتاح المتوقع (نفس تنسيق الـ Keygen)
                    string expectedKey = $"{hex.Substring(0, 5)}-{hex.Substring(5, 5)}-{hex.Substring(10, 5)}";

                    // 4. مطابقة المفتاح المتوقع مع المفتاح المدخل من العميل
                    return expectedKey == savedLicenseKey;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// دالة استخراج بصمة الجهاز (مدمجة هنا لكي لا تحتاج لكلاسات خارجية)
        /// </summary>
        public static string GetMachineId()
        {
            try
            {
                string cpuId = GetWmi("Win32_Processor", "ProcessorId");
                string mbId = GetWmi("Win32_BaseBoard", "SerialNumber");
                string bios = GetWmi("Win32_BIOS", "SerialNumber");

                using (SHA256 sha = SHA256.Create())
                {
                    byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes($"{cpuId}-{mbId}-{bios}"));
                    string hex = BitConverter.ToString(hash).Replace("-", "");

                    // تنسيق البصمة (4 حروف - 4 حروف - 4 حروف - 4 حروف)
                    return $"{hex.Substring(0, 4)}-{hex.Substring(4, 4)}-{hex.Substring(8, 4)}-{hex.Substring(12, 4)}";
                }
            }
            catch { return "ERROR-ID"; }
        }

        /// <summary>
        /// دالة مساعدة لقراءة العتاد من نظام الويندوز
        /// </summary>
        private static string GetWmi(string cls, string prop)
        {
            try
            {
                // ملاحظة: يتطلب تثبيت مكتبة System.Management من NuGet إذا لم تكن مثبتة
                using (var searcher = new ManagementObjectSearcher($"SELECT {prop} FROM {cls}"))
                {
                    foreach (var obj in searcher.Get()) return obj[prop]?.ToString().Trim() ?? "NA";
                }
            }
            catch { }
            return "NA";
        }
    }
}