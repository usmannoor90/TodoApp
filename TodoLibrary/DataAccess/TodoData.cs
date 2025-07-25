﻿using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public class TodoData : ITodoData
{
    private readonly ISqlDataAccess _sql;

    public TodoData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
    {
        return _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_GetAllAssigned",
               new { AssignedTo = assignedTo },
               "Default");

    }
    public async Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId)
    {
        var results = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_GetOneAssigned",
               new { AssignedTo = assignedTo, TodoId = todoId },
               "Default");

        return results.FirstOrDefault();
    }
    public async Task<TodoModel?> Create(int assignedTo, string task)
    {
        var results = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_Create",
               new { AssignedTo = assignedTo, Task = task },
               "Default");

        return results.FirstOrDefault();
    }

    public Task UpdateTask(int assignedTo, int todoId, string task)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_UpdateTask",
               new { AssigneTo = assignedTo, Task = task, TodoId = todoId },
               "Default");

    }
    public Task CompleteTodo(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_ComleteTodo",
               new { AssigneTo = assignedTo, TodoId = todoId },
               "Default");

    }
    public Task Delete(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_Delete",
               new { AssigneTo = assignedTo, TodoId = todoId },
               "Default");

    }

}
