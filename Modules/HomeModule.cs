using System;
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
        List<Category> categoryList = Category.GetAll();
        return View["tasks.cshtml", categoryList];
      };

      Get["/categories/{id}"] = parameters => {
        Category currentCategory = Category.Find(parameters.id);
        return View["category.cshtml", currentCategory];
      };

      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };

      Post["/categories"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        List<Category> allCategories = Category.GetAll();
        return View["categories.cshtml", allCategories];
      };

      Get["/tasks/new"] = _ => {
        int categoryId = int.Parse(Request.Query["category-id"]);
        Category returnCategory = Category.Find(categoryId);
        return View["tasks_form.cshtml", returnCategory];
      };

      Post["/categories/{id}"] = parameters => {
        int categoryId = int.Parse(Request.Form["category-id"]);
        string taskDescription = Request.Form["task-description"];

        Task newTask = new Task(taskDescription, categoryId);
        newTask.Save();

        Category returnCategory = Category.Find(categoryId);

        return View["category.cshtml", returnCategory];
      };

      Post["/categories/clear"] = _ => {
        Category.DeleteAll();
        List<Category> allCategories = Category.GetAll();
        return View["categories.cshtml", allCategories];
      };

      Get["/categories/{categoryId}/tasks/{taskId}/delete"] = parameters => {
        int taskIdNumber = int.Parse(parameters.taskId);
        int categoryIdNumber = int.Parse(parameters.categoryId);

        Task deleteTask = Task.Find(taskIdNumber);
        deleteTask.Delete();

        Category returnCategory = Category.Find(categoryIdNumber);
        return View["category.cshtml", returnCategory];
      };
    }
  }
}
