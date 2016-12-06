using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Category
  {
    private int _id;
    private string _name;

    public Category(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public void SetId(int newId)
    {
      _id = newId;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetName(string newName)
    {
      _name = newName;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherCategory)
    {
      if(!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        bool idEquality = this.GetId() == newCategory.GetId();
        bool nameEquality = this.GetName() == newCategory.GetName();

        return (idEquality && nameEquality);
      }
    }

    public int Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name) OUTPUT INSERTED.id VALUES (@name);", conn);
      SqlParameter categoryNameParameter = new SqlParameter();
      categoryNameParameter.ParameterName = "@name";
      categoryNameParameter.Value = this.GetName();
      cmd.Parameters.Add(categoryNameParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int categoryId = 0;

      while (rdr.Read())
      {
        categoryId = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return categoryId;
    }

    public List<Task> GetTasks()
    {
      SqlConnection conn =  DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE category_id = @CategoryId;",conn);
      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();
      cmd.Parameters.Add(categoryIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Task> tasks = new List<Task>{};
      while (rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        int taskCategoryId = rdr.GetInt32(2);
        Task newTask = new Task(taskDescription, taskCategoryId, taskId);
        tasks.Add(newTask);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return tasks;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
