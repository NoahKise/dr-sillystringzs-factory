using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
    public class EngineersController : Controller
    {
        private readonly FactoryContext _db;
        public EngineersController(FactoryContext db)
        {
            _db = db;
        }


        public ActionResult Index()
        {
            List<Engineer> model = _db.Engineers.ToList();
            return View(model);
        }

        [HttpGet("/engineers/create/")]
        public ActionResult Create()
        {
            Dictionary<string, object> model = new() {
        {"Engineer", new Engineer()},
        {"Machines", _db.Machines.ToList()},
        {"Action", "Create"}
      };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Engineer engineer, List<int> MachineIds)
        {
            if (!ModelState.IsValid)
            {
                Dictionary<string, object> model = new() {
        {"Engineer", new Engineer()},
        {"Machines", _db.Machines.ToList()},
        {"Action", "Create"}
                };
                return View(model);
            }
            else
            {
                _db.Engineers.Add(engineer);
                _db.SaveChanges();

                if (MachineIds != null && MachineIds.Any())
                {
                    foreach (int machineId in MachineIds)
                    {
                        _db.EngineerMachines.Add(new EngineerMachine
                        {
                            EngineerId = engineer.EngineerId,
                            MachineId = machineId
                        });
                    }
                    _db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
        }
        public ActionResult Details(int id)
        {
            Engineer thisEngineer = _db.Engineers
                                .Include(engineer => engineer.EngineerMachines)
                                .ThenInclude(join => join.Machine)
                                .FirstOrDefault(engineer => engineer.EngineerId == id);
            return View(thisEngineer);
        }

        public ActionResult Edit(int id)
        {
            Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
            Dictionary<string, object> model = new() {
            {"Engineer", thisEngineer},
            {"Machines", _db.Machines.ToList()},
            {"Action", "Edit"}
          };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Engineer engineer, List<int> MachineIds)
        {
            _db.Engineers.Update(engineer);
            List<EngineerMachine> joins = _db.EngineerMachines
            .Where((join) => join.EngineerId == engineer.EngineerId).ToList();
            foreach (EngineerMachine join in joins)
            {
                _db.EngineerMachines.Remove(join);
            }
            foreach (int machineId in MachineIds)
            {

                _db.EngineerMachines.Add(new EngineerMachine
                {
                    EngineerId = engineer.EngineerId,
                    MachineId = machineId
                });
            }
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
            return View(thisEngineer);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Engineer thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
            _db.Engineers.Remove(thisEngineer);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}