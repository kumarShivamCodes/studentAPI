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

  private readonly ILogger<StudentController> _logger;
  public StudentController(IStudentService studentService, ILogger<StudentController> logger)
  {
    _studentService = studentService;
    _logger = logger;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
  {
    _logger.LogInformation("Fetching all students");
    return Ok( await _studentService.GetStudentsAsync());
  } 

  [HttpGet("{id}")]
  public async Task<ActionResult<Student>> GetStudent(int id)
  {
   _logger.LogInformation("Get Student By Id request"); 
   var student = await _studentService.GetStudentByIdAsync(id);
   return Ok(student);
  }

  [HttpPost]
  public async Task<ActionResult<Student>> PostStudent(Student student)
  {
    _logger.LogInformation("Create Student request");
     if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create Student request with invalid data");
                return BadRequest(ModelState);
            }
            var createdStudent = await _studentService.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<Student>> PutStudent(int id, Student student)
  {
    _logger.LogInformation("Update Student request");
    if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update Student request with invalid data");
                return BadRequest(ModelState);
            }
    
    return Ok(await _studentService.UpdateStudentAsync(id, student));
  
    
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteStudent(int id)
  {
    _logger.LogInformation("Delete Student request");
    
      await _studentService.DeleteStudentAsync(id);
      return Ok();
    
  }
}
