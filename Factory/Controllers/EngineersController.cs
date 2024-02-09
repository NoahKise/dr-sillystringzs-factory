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
        public ActionResult Details(int id)
        {
            Engineer thisEngineer = _db.Engineers
                                .Include(engineer => engineer.EngineerMachines)
                                .ThenInclude(join => join.Machine)
                                .FirstOrDefault(engineer => engineer.EngineerId == id);
            return View(thisEngineer);
        }

        //     public ActionResult Edit(int id)
        //     {
        //         Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        //         Dictionary<string, object> model = new() {
        //     {"Item", thisItem},
        //     {"CollectionSelect", new SelectList(_db.Collections, "CollectionId", "Name")},
        //     {"TodaysDate", DateTime.Now.ToString("dd-MM-yyyy")},
        //     {"Tags", _db.Tags.ToList()},
        //     {"Action", "Edit"}
        //   };
        //         return View(model);
        //     }

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

        // public ActionResult Delete(int id)
        // {
        //     Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        //     return View(thisItem);
        // }

        // [HttpPost, ActionName("Delete")]
        // public ActionResult DeleteConfirmed(int id)
        // {
        //     Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
        //     _db.Items.Remove(thisItem);
        //     _db.SaveChanges();
        //     return RedirectToAction("Index");
        // }

        //     public ActionResult AddTag(int id)
        //     {
        //         Item item = _db.Items.FirstOrDefault(items => items.ItemId == id);
        //         Dictionary<string, object> model = new()
        //   {
        //     {"item", item},
        //     {"selectList", new SelectList(_db.Tags, "TagId", "Name")}
        //   };
        //         return View(model);
        //     }

        //         [HttpPost]
        //         public ActionResult AddTag(Item item, int tagId)
        //         {
        // #nullable enable
        //             ItemTagJoinEntity? itemTagJoinEntity = _db.ItemTagJoinEntities
        //             .FirstOrDefault(join => (join.TagId == tagId && join.ItemId == item.ItemId));
        // #nullable disable
        //             if (itemTagJoinEntity == null && tagId != 0)
        //             {
        //                 _db.ItemTagJoinEntities.Add(new ItemTagJoinEntity()
        //                 {
        //                     TagId = tagId,
        //                     ItemId = item.ItemId
        //                 });
        //                 _db.SaveChanges();
        //             }
        //             return RedirectToAction("Details", new { id = item.ItemId });
        //         }

        // [HttpPost]
        // public ActionResult DeleteItemTagJoinEntity(int itemTagJoinEntityId)
        // {
        //     ItemTagJoinEntity join = _db.ItemTagJoinEntities.FirstOrDefault(entry => entry.ItemTagJoinEntityId == itemTagJoinEntityId);
        //     _db.ItemTagJoinEntities.Remove(join);
        //     _db.SaveChanges();
        //     return RedirectToAction("Index");
        // }
    }
}