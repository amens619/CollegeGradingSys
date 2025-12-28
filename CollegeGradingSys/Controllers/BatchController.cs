using CollegeGradingSys.Data;
using CollegeGradingSys.Models;
using CollegeGradingSys.Repositories.Implementations;
using CollegeGradingSys.Repositories.Interfaces;
using CollegeGradingSys.Services.Interfaces;
using CollegeGradingSys.Utilities.Exceptions;
using CollegeGradingSys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeGradingSys.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class BatchController : Controller
    {
       
        private readonly IBatchService _batchService;
        private readonly IGenericService<Specialization> _specializationService;
        private readonly IStAcademicDataService StAcademicDataRepository;

        public BatchController(IBatchService batchService,
            IGenericService<Specialization> specializationService,
            IStAcademicDataService StAcademicDataRepository)
        {
            _batchService = batchService;
            _specializationService = specializationService;
            this.StAcademicDataRepository = StAcademicDataRepository;
        }

        // GET: Batche
        public async Task<IActionResult> Index(int? id , int? SpecializationId)
        {


            var viewModel = new BatchIndexData
            {
                Batches = await _batchService.GetBatchesAsync(SpecializationId),
                SpecializationId = SpecializationId
            };

            ViewData["SpecializationId"] = new SelectList(
                await _specializationService.GetAllAsync(),
                "Id",
                "SpecializationName",
                SpecializationId ?? -1
            );

            return View(viewModel);
           
        }

      

        // GET: Batche/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id <= 0)
                return NotFound();

            var batch = await _batchService.GetByIdAsync(id ?? 0);
            if (batch == null)
                return NotFound();

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
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["SpecializationId"] = new SelectList(
               await FillSelectSpecializationsListAsync(),
               "Id",
               "SpecializationName"
           );
           
            return View();
        }

        // POST: Batche/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CreateBatchPolicy")]
        public async Task<IActionResult> Create(BatchCreateDataVM  batchCreateData)
        {
            ViewData["SpecializationId"] =
                    new SelectList(await FillSelectSpecializationsListAsync(),
                      "Id", "SpecializationName");

            if (!ModelState.IsValid)
                return View(batchCreateData);

            try
            {
                await _batchService.CreateAsync(batchCreateData);
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(batchCreateData);
            }          
          
        }

        // GET: Batche/Edit/5
        [Authorize(Policy = "EditBatchPolicy")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)            
                return NotFound();
           

            var Batch =await _batchService.GetByIdAsync(id);

            if (Batch == null)            
                return NotFound();
           
            
            var model = new BatchCreateDataVM
            {
                
                Id = Batch.Id,
                BatchName = Batch.BatchName,
                SpecializationId = Batch.Specialization?.Id ?? 0,
                Note = Batch.Note
                 
            };  
            
            ViewData["SpecializationId"] = new SelectList(
                  await _specializationService.GetAllAsync(),
                  "Id",
                  "SpecializationName",
                  model.SpecializationId
              );
            return View(model);
        }

        // POST: Batche/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditBatchPolicy")]
        public async Task<IActionResult> Edit(int id, BatchEditVM vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Specializations = await _specializationService.GetAllAsync();
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

            vm.Specializations = await _specializationService.GetAllAsync();
            return View(vm);

        }

        // GET: Batche/Delete/5
        [Authorize(Policy = "DeleteBatchPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return NotFound();

            var batch = await _batchService.GetByIdAsync(id);
            if (batch == null)
                return NotFound();

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
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Delete), new { id });
            }           
          
        }

      

        //async Task<List<Specialization>> FillSelectSpecializationsListAsync(string specializationName)
        //{
        //    var specializations = (await _specializationService.GetAllAsync()).ToList();
        //    specializations.Insert(0, new Specialization { Id = -1, SpecializationName = specializationName });

        //    return specializations;
        //}

        async Task<List<SelectItemVM>> FillSelectSpecializationsListAsync()
        {
            var SelectItemVM = await _specializationService.GetSelectItemsAsync(b => new SelectItemVM
            {
                Id = b.Id,
                Name = b.SpecializationName
            }, "-- أختر --");
            return SelectItemVM;
        }


    }

    //public class task<T>
    //{
    //}
}
