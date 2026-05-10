using CollegeGradingSys.Models;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels.Batch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class BatchController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IGenericService<Specialization> _specializationService;

        public BatchController(
            IBatchService batchService,
            IGenericService<Specialization> specializationService)
        {
            _batchService = batchService;
            _specializationService = specializationService;
        }

        // GET: Batche
        public async Task<IActionResult> Index(int? SpecializationId)
        {
            var specializations = await _specializationService.GetAllAsync();

            var viewModel = new BatchIndexData
            {
                Batches = await _batchService.GetBatchesAsync(SpecializationId),
                SpecializationId = SpecializationId,
                // الاستغناء عن ViewData
                SpecializationsList = new SelectList(specializations, "Id", "SpecializationName", SpecializationId ?? -1)
            };

            return View(viewModel);
        }

        // GET: Batche/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var batch = await _batchService.GetByIdAsync(id.Value);
            if (batch == null) return NotFound();

            var vm = new BatchDetailsVM
            {
                Id = batch.Id,
                BatchName = batch.BatchName,
                Note = batch.Note,
                SpecializationName = batch.Specialization?.SpecializationName
            };

            return View(vm);
        }

        // GET: Batche/Create
        [Authorize(Policy = "CreateBatchPolicy")]
        public async Task<IActionResult> Create()
        {
            var specializations = await _specializationService.GetAllAsync();

            var vm = new BatchCreateDataVM
            {
                // الاستغناء عن ViewData وتمرير القائمة عبر المودل
                SpecializationsList = new SelectList(specializations, "Id", "SpecializationName")
            };

            return View(vm);
        }

        // POST: Batche/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateBatchPolicy")]
        public async Task<IActionResult> Create(BatchCreateDataVM batchCreateData)
        {
            if (!ModelState.IsValid)
            {
                // إعادة تعبئة القائمة المنسدلة عند فشل التحقق
                var specializations = await _specializationService.GetAllAsync();
                batchCreateData.SpecializationsList = new SelectList(specializations, "Id", "SpecializationName", batchCreateData.SpecializationId);
                return View(batchCreateData);
            }

            try
            {
                await _batchService.CreateAsync(batchCreateData);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                // إعادة تعبئة القائمة في حال خطأ قواعد العمل
                var specializations = await _specializationService.GetAllAsync();
                batchCreateData.SpecializationsList = new SelectList(specializations, "Id", "SpecializationName", batchCreateData.SpecializationId);
                return View(batchCreateData);
            }
        }

        // GET: Batche/Edit/5
        [Authorize(Policy = "EditBatchPolicy")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return NotFound();

            var batch = await _batchService.GetByIdAsync(id);
            if (batch == null) return NotFound();

            var specializations = await _specializationService.GetAllAsync();

            // استخدام BatchEditVM كما هو متوقع في الـ POST
            var model = new BatchEditVM
            {
                Id = batch.Id,
                BatchName = batch.BatchName,
                SpecializationId = batch.Specialization?.Id ?? 0,
                Note = batch.Note,
                SpecializationsList = new SelectList(specializations, "Id", "SpecializationName", batch.Specialization?.Id)
            };

            return View(model);
        }

        // POST: Batche/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditBatchPolicy")]
        public async Task<IActionResult> Edit(int id, BatchEditVM vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var specializations = await _specializationService.GetAllAsync();
                vm.SpecializationsList = new SelectList(specializations, "Id", "SpecializationName", vm.SpecializationId);
                return View(vm);
            }

            try
            {
                await _batchService.UpdateBatchAsync(vm);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            // في حال الوصول إلى هنا (استثناء من السيرفس)
            var specs = await _specializationService.GetAllAsync();
            vm.SpecializationsList = new SelectList(specs, "Id", "SpecializationName", vm.SpecializationId);
            return View(vm);
        }

        // GET: Batche/Delete/5
        [Authorize(Policy = "DeleteBatchPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return NotFound();

            var batch = await _batchService.GetByIdAsync(id);
            if (batch == null) return NotFound();

            var vm = new BatchDeleteVM
            {
                Id = batch.Id,
                BatchName = batch.BatchName,
                SpecializationName = batch.Specialization?.SpecializationName
            };

            return View(vm);
        }

        // POST: Batche/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteBatchPolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _batchService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                // استخدام TempData بدلاً من ViewBag لنقل الرسالة إلى صفحة أخرى
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}