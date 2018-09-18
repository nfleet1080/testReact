using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        private static string[] todos = new[]
        {
            "Jog", "Hike", "Run", "Relax", "Sleep", "Eat", "Sit", "Jump", "Hop", "Swim"
        };

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.

                //_context.TodoItems.Add(new TodoItem { Name = "Item" });
                for (int i = 0; i < todos.Length; i++)
                {
                    _context.TodoItems.Add(new TodoItem { Name = todos[i] });
                }
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpGet("[action]")]
        public IEnumerable<TodoItem> GetHops()
        {
            // grab Hopping actions, sort by name
            List<TodoItem> hopAndJumpItems = _context.TodoItems.Where(a => a.Name == "Hop" || a.Name == "Jump").OrderBy(o => o.Name).ToList();
            
            // update hop to complete
            TodoItem updateHop = hopAndJumpItems.FirstOrDefault(s=>s.Name=="Hop");
            if(updateHop != null) updateHop.IsComplete = true;

            // save the updated value
            _context.SaveChanges();

            IEnumerable<TodoItem> hopList = hopAndJumpItems;
            return hopList;
        }
    }
}