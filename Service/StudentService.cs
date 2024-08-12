
using Microsoft.EntityFrameworkCore;
using studentAPI.Data;
using studentAPI.Models;

namespace studentAPI.Service;

public class StudentService : IStudentService
{
  private readonly AppDbContext _dbContext;

  public StudentService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }
    public async Task<Student> CreateStudentAsync(Student student)
    {
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
        return student;
    }

    public async Task DeleteStudentAsync(int id)
    {
        var student= await _dbContext.Students.FindAsync(id);
        if(student == null)
        {
            throw new KeyNotFoundException("Student not found");
        }
        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync(); 
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        return await _dbContext.Students.FindAsync(id);
    }

    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        return await _dbContext.Students.ToListAsync();
    }

    public async Task<Student> UpdateStudentAsync(int id, Student student)
    {
        if(id != student.Id)
        {
            throw new KeyNotFoundException("Id mismatched!");
        }
        else if(await _dbContext.Students.FindAsync(id) == null)
        {
            throw new KeyNotFoundException("Student not found!");
        }

        _dbContext.Entry(student).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return await _dbContext.Students.FindAsync(id) ;
    }

}
