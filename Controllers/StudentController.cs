using Microsoft.AspNetCore.Mvc;
using studentAPI.Service;
using studentAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace studentAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController: ControllerBase
{
  private readonly IStudentService _studentService;
  public StudentController(IStudentService studentService)
  {
    _studentService = studentService;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
  {
    return Ok( await _studentService.GetStudentsAsync());
  } 

  [HttpGet("{id}")]
  public async Task<ActionResult<Student>> GetStudent(int id)
  {
   var student = await _studentService.GetStudentByIdAsync(id);
   return Ok(student);
  }

  [HttpPost]
  public async Task<ActionResult<Student>> PostStudent(Student student)
  {
     if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdStudent = await _studentService.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Student>> PutStudent(int id, Student student)
  {
    if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
    
    return Ok(await _studentService.UpdateStudentAsync(id, student));
  
    
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteStudent(int id)
  {
    
    
      await _studentService.DeleteStudentAsync(id);
      return Ok();
    
  }
}
