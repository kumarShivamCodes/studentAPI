using Microsoft.AspNetCore.Mvc;
using studentAPI.Service;
using studentAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

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

  /// <summary>
  /// Retrieves a list of all students
  /// </summary>
  /// <returns>List of students</returns>
  [HttpGet]
  [SwaggerResponse(StatusCodes.Status200OK,Type = typeof(IEnumerable<Student>))]
  [SwaggerResponse(StatusCodes.Status500InternalServerError)]
  [Produces("application/json")]
  public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
  {
    _logger.LogInformation("Fetching all students");
    return Ok( await _studentService.GetStudentsAsync());
  } 

  /// <summary>
  /// Retrieves a specific student by ID.
  /// </summary>
  /// <param name="id">ID of the student </param>
  /// <returns>Student with specified id  </returns>
  [HttpGet("{id:int}")]
  [SwaggerResponse(StatusCodes.Status200OK)]
  [SwaggerResponse(StatusCodes.Status404NotFound)]
  [Produces("application/json")]
  public async Task<ActionResult<Student>> GetStudent(int id)
  {
   _logger.LogInformation("Get Student By Id request"); 
   var student = await _studentService.GetStudentByIdAsync(id);
   return Ok(student);
  }

  /// <summary>
  /// Creates a new a student with the provided data
  /// </summary>
  /// <returns>Created student object</returns>
  [HttpPost]
  [SwaggerResponse(StatusCodes.Status201Created)]
  [SwaggerResponse(StatusCodes.Status400BadRequest)]
  [Produces("application/json")]
  [Consumes("application/json")]
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

  /// <summary>
  /// Updates an existing student.
  /// </summary>
  /// <param name="id">Student ID</param>
  /// <param name="student">Updated student object</param>
  /// <returns>Updated student object</returns>
  [HttpPut("{id:int}")]
  [SwaggerResponse(StatusCodes.Status200OK)]
  [SwaggerResponse(StatusCodes.Status400BadRequest)]
  [SwaggerResponse(StatusCodes.Status404NotFound)]
  [Produces("application/json")]
  [Consumes("application/json")]
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

  /// <summary>
  /// Deletes an existing student.
  /// </summary>
  /// <param name="id">Student ID</param>
  /// <returns>No content</returns>
  [HttpDelete("{id:int}")]
  [SwaggerResponse(StatusCodes.Status200OK)]
  [SwaggerResponse(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> DeleteStudent(int id)
  {
    _logger.LogInformation("Delete Student request");
    
      await _studentService.DeleteStudentAsync(id);
      return Ok();
    
  }
}
