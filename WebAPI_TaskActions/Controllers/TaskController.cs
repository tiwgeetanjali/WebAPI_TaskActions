using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using WebAPI_TaskActions.Model;
using static WebAPI_TaskActions.Model.ValidStatus;
using Task = WebAPI_TaskActions.Model.Task;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI_TaskActions.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly TaskDBContext _context;

        public TaskController(TaskDBContext context)
        {
            _context = context;
        }

        //GET list of all tasks
        [HttpGet]
        public List<Task> GetTasks()
        {
            return _context.Task.ToList();
        }

        //GET list of tasks by id
        [HttpGet("{task_id}")]
        public ActionResult<Task> GetTaskByName(int task_id)
        {
            #region validation

            //Validate task id is null
            if (task_id == 0)
            {
                return NotFound("Please enter task id to get details.");
            }

            var taskvar = _context.Task.SingleOrDefault(x => x.task_id == task_id);

            //Validate if task id is not valid
            if (taskvar == null)
            {
                return NotFound("Task id could not be found.");
            }

            #endregion

            return taskvar;
        }

        // POST api/values
        [HttpPost]
        public IActionResult AddTask(Task task)
        {
            #region validation

            //check if task id is present/not
            var i = _context.Task.Select(p => p.task_id).FirstOrDefault();
            if (i == 0)
            {
                task.task_id = 1;
            }
            else
            {
                //check max task id & increment
                i = _context.Task.Max(p => p.task_id);
                task.task_id = i + 1;
            }

            //Validate if task name is not enetered
            if (task.task_name == null)
            {
                return NotFound("Task name cannot be null.");
            }

            //Validate if task priority is not enetered
            if (task.task_priority == null)
            {
                return NotFound("Task Priority cannot be null.");
            }

            //Validate if task status is not enetered
            if (task.task_status == null)
            {
                return NotFound("Task status cannot be null.");
            }
            else
                task.task_status = task.task_status.ToLower();

            //Validate if task staus is not valid
            if (!Enum.IsDefined(typeof(ValidStatus.status), task.task_status))
            {
                return NotFound("Task Status is not valid.");
            }

            var Check_task_name = _context.Task.SingleOrDefault(x => x.task_name.ToLower() == task.task_name.ToLower());
            if (Check_task_name != null)
            {
                return Ok("Task name: " + Check_task_name.task_name + " already exist.");
            }

            #endregion

            _context.Task.Add(task);
            _context.SaveChanges();
            return Created("api/task/"+task.task_name, task);

        }

        // PUT api/values/5
        [HttpPut("{task_id}")]
        public IActionResult UpdateTask(Task task)
        {
            #region validation

            //Validate task id is null
            if (task.task_id == null)
            {
                return NotFound("Please enter task id to update details.");
            }

            var taskvar = _context.Task.SingleOrDefault(x => x.task_id == task.task_id);

            //Validate if task id is not enetered
            if (taskvar == null)
            {
                return NotFound("Task id could not be found..");
            }

            //Validate if task name is not enetered
            if (task.task_name == null)
            {
                return NotFound("Task name cannot be null.");
            }

            //Validate if task priority is not enetered
            if (task.task_priority == null)
            {
                return NotFound("Task Priority cannot be null.");
            }

            //Validate if task status is not enetered
            if (task.task_status == null)
            {
                return NotFound("Task status cannot be null.");
            }
            else
                task.task_status = task.task_status.ToLower();

            //Validate if task staus is not valid
            if (!Enum.IsDefined(typeof(ValidStatus.status), task.task_status))
            {
                return NotFound("Task Status is not valid.");
            }

            var Check_task_name = _context.Task.SingleOrDefault(x => x.task_name.ToLower() == task.task_name.ToLower() && x.task_id != task.task_id);
            if (Check_task_name != null)
            {
                return Ok("Task name: " + Check_task_name.task_name + " already exist.");
            }

            #endregion

            taskvar.task_name = task.task_name;
            taskvar.task_priority = task.task_priority;
            taskvar.task_status = task.task_status;

            _context.ChangeTracker.Clear();
            _context.Update(taskvar);
            _context.SaveChanges();
            return Ok("Task with name " + taskvar.task_name + " updated successfully.");
        }

        // DELETE api/values/taskname
        [HttpDelete("{task_name}")]
        public IActionResult Delete(string task_name)
        {
            #region validation

            if(String.IsNullOrEmpty(task_name))
            {
                return NotFound("Task name cannot be null.");
            }
            var delete_task_name = _context.Task.SingleOrDefault(x => x.task_name == task_name);
            if(delete_task_name == null)
            {
                return NotFound("Task id couldnot be found.");
            }

            #endregion

            _context.Task.Remove(delete_task_name);
            _context.SaveChanges();
            return Ok("Task with name " + task_name + " deleted successfully.");
        }
    }
}

