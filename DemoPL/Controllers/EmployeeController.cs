using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using DemoPL.Helpers;
using DemoPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
            private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper) // Ask CLR forCreating object From Impement IEmployeeRepository
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Index
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            //ViewData["Message"] = "Hello From View Data";

            //ViewBag.Message = "Hello From View Bag";
            var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string searchString)
        {

            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(searchString);
            }
            var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(MappedEmployee);
        }

        #endregion

        #region Create
        public IActionResult Create()
        {
            // ViewBag.Department = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVm)
        {
            if (ModelState.IsValid)
            {
                /// Manual Mapping .. 
                ///var MappedEmployee = new Employee()
                ///{
                ///    Name = employeeVm.Name,
                ///    Age = employeeVm.Age,
                ///    Address = employeeVm.Address,
                ///    PhoneNumber = employeeVm.PhoneNumber,
                ///    DepartmentId = employeeVm.DepartmentId,
                ///};
                ///Employee employee = (Employee)employeeVm;
                ///int Result = _unitOfWork.EmployeeRepository.Add(MappedEmployee);


                employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");

                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);

                await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
                int Result = await _unitOfWork.CompleteAsync();
                //_unitOfWork.Dispose();
                if (Result > 0)
                    TempData["Message"] = "Department Is Created";

                return RedirectToAction(nameof(Index));

            }
            return View(employeeVm);
        }


        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(MappedEmployee);
        }

        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)

        {
            //ViewBag.Department = _departmentRepository.GetAll();

            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVm, [FromRoute] int id)
        {
            if (id != employeeVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVm.Image is not null)
                    {
                        employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");

                    }
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    int Result = await _unitOfWork.CompleteAsync();

                    if (Result > 0)
                        TempData["Message"] = "Department Has been Updated";

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVm);




        }


        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, nameof(Delete));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();



            try
            {
                var MappedEmplyee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(MappedEmplyee);
                int Result = await _unitOfWork.CompleteAsync();

                if (Result > 0 && employeeVM.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    TempData["Message"] = "Department Has been Deleted";
                }
                return RedirectToAction(nameof(Index));

            }
            catch (System.Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }

        } 
        #endregion
    }
}
