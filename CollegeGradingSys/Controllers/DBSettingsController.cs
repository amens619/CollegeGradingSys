using CollegeGradingSys.Models;
using CollegeGradingSys.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin, Owner")]
    public class DBSettingsController : Controller
    {
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;
        public IConfiguration _configuration { get; }
        private readonly ICollegeGradingSysRepository<DBSettings> _DBSettingsRepository;

        [Obsolete]
        public DBSettingsController(IHostingEnvironment hostingEnvironment, IConfiguration Configuration, ICollegeGradingSysRepository<DBSettings> DBSettingsRepository)
        {
            _configuration = Configuration;
            _DBSettingsRepository = DBSettingsRepository;
            this.hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            var dBSettings = _DBSettingsRepository.List();

            dBSettings = dBSettings.OrderByDescending(x => x.Id).ToList();



            return View(dBSettings);
        }




        //public string  => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sysFiles", "Backups");


        [HttpPost]
        [Obsolete]
        [Authorize(Policy = "CreateBackupPolicy")]
        public ActionResult Backup(int id)
        {
            string BackupPath = Path.Combine(hostingEnvironment.ContentRootPath.ToString(), "wwwroot", "Backups");
            //string BackupPath = Path.Combine("D:", "Backups");

            if (!Directory.Exists(BackupPath))
            {
                Directory.CreateDirectory(BackupPath);
            }

            string uniqueFileName = null;
            uniqueFileName = DateTime.Now.ToString().Replace('/', '-').Replace(':', '-').Replace(" ", "");

            var con = new SqlConnection(_configuration.GetConnectionString("SqlCon"));
            var sql = $"BACKUP DATABASE [{con.Database}]"
                + $" TO DISK ='{BackupPath}\\{uniqueFileName}.bak'"
                + $"WITH NOFORMAT, NOINIT, SKIP, STATS = 10; ";

            SqlCommand cmd = new();
            cmd.Connection = con;
            cmd.CommandText = sql;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                this.SetSuccessMessage("تمت أخذ نسخة احتياطية بنجاح");
                con.Close();
                var DBSetting = new DBSettings() { BackupName = uniqueFileName + ".bak" };
                _DBSettingsRepository.Add(DBSetting);
            }
            catch (Exception ex)
            {
                this.SetErrorDetailsMessge(ex.Message);
                this.SetErrorMessge("حدث خطأ لم يتم أخذ نسخة احتياطية");
            }
            return RedirectToAction(nameof(Index));
        }

        [Obsolete]
        public IActionResult GetBlobDownload(int id)
        {
            var GP = _DBSettingsRepository.Find(id);

            string uploadFolder = Path.Combine(hostingEnvironment.ContentRootPath.ToString(), "wwwroot", "Backups");


            string fileePath = Path.Combine(uploadFolder, GP.BackupName);

            var net = new System.Net.WebClient();
            var data = net.DownloadData(fileePath);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            //var fileName = "something.bin";

            return File(content, contentType, GP.BackupName);
        }


        // GET: DBSettings/Delete/5
        [Authorize(Policy = "DeleteBackupPolicy")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DBSetting = _DBSettingsRepository.Find((int)id);
            if (DBSetting == null)
            {
                return NotFound();
            }

            return View(DBSetting);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteBackupPolicy")]
        public IActionResult DeleteConfirmed(int id)
        {
            var DBSetting = _DBSettingsRepository.Find((int)id);
            if (DBSetting == null)
            {
                return NotFound();
            }
            _DBSettingsRepository.Delete((int)id);

            string previousFileUrl = DBSetting.BackupName;

            string uploadFolder = Path.Combine(hostingEnvironment.ContentRootPath.ToString(), "wwwroot", "Backups");
            //===============delete previous file=============
            string previousfileePath = Path.Combine(uploadFolder, previousFileUrl);
            if (System.IO.File.Exists(previousfileePath))
            {
                System.IO.File.Delete(previousfileePath);

            }

            return RedirectToAction(nameof(Index));
        }
        private void SetErrorMessge(string v)
        {

        }

        private void SetErrorDetailsMessge(object p)
        {

        }

        private void SetSuccessMessage(string v)
        {

        }
    }
}
