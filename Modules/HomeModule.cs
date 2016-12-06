using System.Collections.Generic;
using Nancy.ViewEngines.Razor;
using Nancy;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/categories"] = _ => {
        List<Category> allCategories = Category.GetAll();
        return View["categories.cshtml", allCategories];
      };
      Get["/tasks"] = _ => {
        //Get all task and categories and make lists
        List<Task> taskList = Task.GetAll();
        List<Category> categoryList = Category.GetAll();

        //Create and populate dictionary to hold tasks
        Dictionary<int, Object> taskDictionary = new Dictionary<int, Object>();
        foreach (task in taskList) {
          taskDictionary.Add(task.GetId(), task.GetDescription());
        };

        //Create and populate dictionary to hold categories
        Dictionary<int, Object> categoryDictionary = new Dictionary<int, Object>();
        foreach (category in categoryList) {
          categoryDictionary.Add(category.GetId(), category.GetName());
        };

        //Create a dictionary to hold task and category dictionaries
        Dictionary<string, Dictionary<string, Object>> returnDictionary = new Dictionary<string, Dictionary<string, Object>>()
        {
          { "tasks", taskDictionary },
          { "categories", categoryDictionary }
        };

        return View["tasks.cshtml", returnDictionary];
      };

      Get["/categories/{category_id}/tasks/new"] = parameters => {

        
      };
    }
  }
}
