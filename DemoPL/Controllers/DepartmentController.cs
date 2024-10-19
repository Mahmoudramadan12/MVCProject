using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using DemoPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public DepartmentController(IUnitOfWork  unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;

			//_departmentRepository = new DepartmentRepository();
		}
        #region Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
			var MappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);


			return View(MappedDepartments);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentvm)
        {
            if (ModelState.IsValid) // Server Side Validation 
            {
				//var mapped = new Department()
				//{
				//    Id = department.Id,
				//    Code = department.Code,
				//    Name = department.Name,
				//    DateofCreation = department.DateofCreation,
				//    Employees = department.Employees


				//};
				var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentvm);



				await _unitOfWork.DepartmentRepository.AddAsync(MappedDepartment);
                int Reuslt = await _unitOfWork.CompleteAsync();

                if (Reuslt > 0)
                    TempData["Message"] = "Department Is Created";

                return RedirectToAction(nameof(Index));
            }
            return View(departmentvm);
        }
		#endregion

		#region Details
		public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); // Status Code 400
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null)
                return NotFound();
			var MappedDepartment = _mapper.Map< Department, DepartmentViewModel>(department);

			return View(ViewName, MappedDepartment);
        } 
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest(); // Status Code 400
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);


            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentvm, [FromRoute] int id)
        {


            if (id != departmentvm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {

					var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentvm);

					_unitOfWork.DepartmentRepository.Update(MappedDepartment);
                    int Reuslt = await _unitOfWork.CompleteAsync();

                    if (Reuslt > 0)
                        TempData["Message"] = "Department Is Updated";

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentvm);

        }

		#endregion

		#region Delete
		public async Task<IActionResult> Delete(int id)
        {

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentvm, [FromRoute] int id)
        {

            if (id != departmentvm.Id)
                return BadRequest();

            try
            {
				var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentvm);



				_unitOfWork.DepartmentRepository.Delete(MappedDepartment);
                int Reuslt = await _unitOfWork.CompleteAsync();
                if (Reuslt > 0)
                    TempData["Message"] = "Department Is Deleted";

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentvm);
            }

        } 
        #endregion
    }


    
}
