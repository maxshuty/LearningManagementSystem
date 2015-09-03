using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using BootcampLMS.Data.Repositories;
using BootcampLMS.Models;

namespace BootcampLMS.UI.Controllers
{
    public class DevAssTrackerController : Controller
    {
        AssignmentTrackerRepo _myAssTrackerRepo = new AssignmentTrackerRepo();
        RosterRepo _myRosterRepo = new RosterRepo();
        AssignmentRepo _myAssRepo = new AssignmentRepo();

        public static List<int> AllAssignmentIds = new List<int>();
        public static List<int> AllRosterIds = new List<int>(); 
        public List<Assignment> AllAssignments = new List<Assignment>();
        public List<Roster> AllRosters = new List<Roster>(); 

        public DevAssTrackerController()
        {
            AllAssignmentIds.Clear();
            AllAssignments.Clear();

            AllAssignments = _myAssRepo.GetAll();
            foreach (var ass in AllAssignments)
            {
                AllAssignmentIds.Add(ass.AssignmentId);
            }

            AllRosters.Clear();
            AllRosterIds.Clear();

            AllRosters = _myRosterRepo.GetAll();
            foreach (var roster in AllRosters)
            {
                AllRosterIds.Add(roster.RosterId);
            }
        }

        public ActionResult Delete(int id)
        {
            _myAssTrackerRepo.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(AssignmentTracker assTrack)
        {
            _myAssTrackerRepo.Edit(assTrack);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var myAssTrack = _myAssTrackerRepo.GetById(id);

            return View(myAssTrack);
        }

        // GET: AssignmentTracker
        public ActionResult Index()
        {
            List<AssignmentTracker> myAssTrackerList = _myAssTrackerRepo.GetAll();
            return View(myAssTrackerList);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AssignmentTracker assTracker)
        {
            _myAssTrackerRepo.Add(assTracker);

            return RedirectToAction("Index");
        }
    }
}