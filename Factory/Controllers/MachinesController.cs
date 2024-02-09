using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
    public class MachinesController : Controller
    {
        private readonly FactoryContext _db;
        public MachinesController(FactoryContext db)
        {
            _db = db;
        }


        public ActionResult Index()
        {
            List<Machine> model = _db.Machines.ToList();
            return View(model);
        }

        [HttpGet("/machines/create/")]
        public ActionResult Create()
        {
            Dictionary<string, object> model = new() {
        {"Machine", new Machine()},
        {"Engineers", _db.Engineers.ToList()},
        {"Action", "Create"}
      };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Machine machine, List<int> EngineerIds)
        {
            _db.Machines.Add(machine);
            _db.SaveChanges();

            if (EngineerIds != null && EngineerIds.Any())
            {
                foreach (int engineerId in EngineerIds)
                {
                    _db.EngineerMachines.Add(new EngineerMachine
                    {
                        MachineId = machine.MachineId,
                        EngineerId = engineerId
                    });
                }
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Machine thisMachine = _db.Machines
                                .Include(machine => machine.EngineerMachines)
                                .ThenInclude(join => join.Engineer)
                                .FirstOrDefault(machine => machine.MachineId == id);
            return View(thisMachine);
        }
        public ActionResult Edit(int id)
        {
            Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
            Dictionary<string, object> model = new() {
            {"Machine", thisMachine},
            {"Engineers", _db.Engineers.ToList()},
            {"Action", "Edit"}
          };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Machine machine, List<int> EngineerIds)
        {
            _db.Machines.Update(machine);
            List<EngineerMachine> joins = _db.EngineerMachines
            .Where((join) => join.MachineId == machine.MachineId).ToList();
            foreach (EngineerMachine join in joins)
            {
                _db.EngineerMachines.Remove(join);
            }
            foreach (int engineerId in EngineerIds)
            {

                _db.EngineerMachines.Add(new EngineerMachine
                {
                    MachineId = machine.MachineId,
                    EngineerId = engineerId
                });
            }
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
            return View(thisMachine);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Machine thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
            _db.Machines.Remove(thisMachine);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}