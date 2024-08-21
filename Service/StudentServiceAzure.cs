using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using studentAPI.Models;
using studentAPI.Service;

public class StudentServiceAzure : IStudentService
{
    private readonly TableClient _tableClient;
    private readonly ILogger<StudentServiceAzure> _logger;

    public StudentServiceAzure(TableServiceClient tableClient, ILogger<StudentServiceAzure> logger)
    {
        _tableClient = tableClient.GetTableClient("student");
        _logger = logger; 
    }
   public async Task<Student> CreateStudentAsync(Student student)
{
    student.PartitionKey = "StudentPartition";
    _logger.LogInformation("Creating student with id {Id}", student.Id);
    try
    {
        // Try to get the entity
        var existingStudent = await _tableClient.GetEntityAsync<Student>(student.PartitionKey, student.Id.ToString());
        
        // If no exception is thrown, the student exists
        _logger.LogInformation("Student with id {Id} already exists", student.Id);
        throw new InvalidOperationException("Student already exists!");
    }
    catch (RequestFailedException ex) when (ex.Status == 404)
    {
        // If a 404 is thrown, it means the student does not exist, so proceed with creation
        student.RowKey = student.Id.ToString();
        await _tableClient.AddEntityAsync(student);
        _logger.LogInformation("Created student with id {Id}", student.Id);
        return student;
    }
    catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student with ID: {Id}", student.Id);
            throw;
        }
}


    public async Task DeleteStudentAsync(int id)
    {
        _logger.LogInformation("Deleting student with id {Id}", id);
       try{
              await _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString()); 
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        { 
            _logger.LogWarning("Student with id {Id} not found", id);
            throw new KeyNotFoundException("Student not found!");
        }
      await _tableClient.DeleteEntityAsync("StudentPartition", id.ToString());
      _logger.LogInformation("Deleted student with id {Id}", id);
        
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
         _logger.LogInformation("Retrieving student with ID: {Id}", id);
        try{
             return await _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString()); 
        }
        
        catch (RequestFailedException ex) when (ex.Status == 404)
        { 
             _logger.LogWarning("Student with id {Id} not found", id);
            throw new KeyNotFoundException("Student not found!");
        }
         catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student with ID: {Id}", id);
            throw;
        }
            
    }

    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        _logger.LogInformation("Retrieving all students");
        var students = new List<Student>();
        await foreach (var student in _tableClient.QueryAsync<Student>())
        {
            students.Add(student);
        }
        _logger.LogInformation("Retrieved {Count} students", students.Count);
        return students;
    }

    public async Task<Student> UpdateStudentAsync(int id, Student student)
    {
        _logger.LogInformation("Updating student with id {Id}", id);
        if (id != student.Id)
        {
            _logger.LogWarning("ID mismatch: {Id} does not match student ID: {StudentId}", id, student.Id);
            throw new KeyNotFoundException("Id mismatched!");
        }

         try{
              await _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString()); 
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        { 
            _logger.LogWarning("Student with id {Id} not found", id);
            throw new KeyNotFoundException("Student not found!");
        }
        
        await _tableClient.UpdateEntityAsync(student, ETag.All, TableUpdateMode.Replace);
        _logger.LogInformation("Updated student with id {Id}", id);
        return await _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString());
    }
}