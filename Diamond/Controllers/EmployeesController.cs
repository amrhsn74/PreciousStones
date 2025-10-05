using Diamond.Data;
using Diamond.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Diamond.Controllers
{
    public class EmployeesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        //HTTP -> Hyper-text Transfer Protocol
        //HTTP Verbs -> HttpGet - HttpPost
        [HttpGet]
        public IActionResult GetIndexView(string? search)
        {
            ViewBag.CurrentSearch = search;

            if(string.IsNullOrEmpty(search) == true)
                return View("Index", context.Employees.ToList());
            else
                return View("Index", context.Employees.Where(e => e.FullName.Contains(search) ||
                                                                  e.Position.Contains(search)).ToList());
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(context.Employees.ToList());
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Employee emp = context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);

            if (emp == null) return NotFound();

            return View("Details", emp);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Employee emp = context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);

            if (emp == null)
            {
                return NotFound();
            }

            return View(emp);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            return View("Create");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNew(Employee employee)
        {
            if (employee.DateOfBirth.AddYears(18) > DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError(nameof(employee.DateOfBirth),
                                         "Illegal Hiring Age (Under 18 years old).");
            }

            if (ModelState.IsValid == true)
            {
                context.Employees.Add(employee);
                context.SaveChanges();
                return RedirectToAction(nameof(GetIndexView));
            }
            else
            {
                return View("Create", employee);
            }
        }


        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Employee emp = context.Employees.FirstOrDefault(e => e.Id == id);

            if (emp == null) return NotFound();

            ViewBag.AllDepartments = new SelectList(context.Departments, "Id", "Description");
            return View("Edit", emp);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee emp = context.Employees.FirstOrDefault(e => e.Id == id);

            if (emp == null) return NotFound();

            ViewBag.AllDepartments = new SelectList(context.Departments, "Id", "Description");
            return View(emp);
        }


        [HttpPost]
        public IActionResult EditCurrent(Employee employee)
        {
            if (employee.DateOfBirth.AddYears(18) > DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError(nameof(employee.DateOfBirth),
                                         "Illegal Hiring Age (Under 18 years old).");
            }

            if (ModelState.IsValid == true)
            {
                context.Employees.Update(employee);
                context.SaveChanges();
                return RedirectToAction(nameof(GetIndexView));
            }
            else
            {
                ViewBag.AllDepartments = new SelectList(context.Departments, "Id", "Description");
                return View("Edit", employee);
            }
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Employee emp = context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);

            if (emp == null) return NotFound();

            return View("Delete", emp);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee emp = context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);

            if (emp == null) return NotFound();

            return View(emp);
        }

        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Employee employee = context.Employees.FirstOrDefault(x => x.Id == id);
            context.Employees.Remove(employee);
            context.SaveChanges();

            return RedirectToAction(nameof(GetIndexView));
        }















        //https://localhost:7033/Employees/GreetVisitor
        public string GreetVisitor()
        {
            return "Welcome to Precious Stones!";
        }

        // Query String ->   ?parameter1=argument1&parameter2=argument2
        //https://localhost:7033/Employees/CalculateAge?name=Mostafa&birthYear=2005
        public string CalculateAge(string name, int birthYear)
        {
            return $"Hi, {name}. You are {DateTime.Now.Year - birthYear} years old.";
        }


    }
}
