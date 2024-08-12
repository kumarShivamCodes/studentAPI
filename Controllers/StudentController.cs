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
            if (student == null)
            {
                return NotFound();
            }
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
    try{
    return Ok(await _studentService.UpdateStudentAsync(id, student));
    }
    catch(ArgumentException)
    {
      return BadRequest();
    }
    catch(KeyNotFoundException)
    {
      return NotFound();
    }
  }

  [HttpDelete("{id}")]
  [Authorize (Roles = "ReadWrite")]
  public async Task<ActionResult> DeleteStudent(int id)
  {
    try
    {
      await _studentService.DeleteStudentAsync(id);
      return Ok();
    }
    catch(KeyNotFoundException)
    {
      return NotFound();
    }
  }
}
