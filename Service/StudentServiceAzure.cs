using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using studentAPI.Models;
using studentAPI.Service;

public class StudentServiceAzure : IStudentService
{
    private readonly TableClient _tableClient;

    public StudentServiceAzure()
    {
        _tableClient = new TableClient( new Uri("https://apidatastorage123.table.core.windows.net/student"),"student",new DefaultAzureCredential());
    }
   public async Task<Student> CreateStudentAsync(Student student)
{
    student.PartitionKey = "StudentPartition";
    try
    {
        // Try to get the entity
        var existingStudent = await _tableClient.GetEntityAsync<Student>(student.PartitionKey, student.Id.ToString());
        
        // If no exception is thrown, the student exists
        throw new InvalidOperationException("Student already exists!");
    }
    catch (RequestFailedException ex) when (ex.Status == 404)
    {
        // If a 404 is thrown, it means the student does not exist, so proceed with creation
        student.RowKey = student.Id.ToString();
        await _tableClient.AddEntityAsync(student);
        return student;
    }
}


    public async Task DeleteStudentAsync(int id)
    {
      var student = await _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString());
      if (student == null)
        {
          throw new KeyNotFoundException("Student not found");
        }
      await _tableClient.DeleteEntityAsync("StudentPartition", id.ToString());
        
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        var student = await _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString());
        if (student == null)
        {
            throw new KeyNotFoundException("Student not found");
        }
        return student;

    }

    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        var students = new List<Student>();
        await foreach (var student in _tableClient.QueryAsync<Student>())
        {
            students.Add(student);
        }
        return students;
    }

    public async Task<Student> UpdateStudentAsync(int id, Student student)
    {
        if (id != student.Id)
        {
            throw new KeyNotFoundException("Id mismatched!");
        }

        var existingStudent = _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString());
        if (existingStudent == null)
        {
            throw new KeyNotFoundException("Student not found!");
        }
        student.PartitionKey = "StudentPartition";
        student.RowKey = id.ToString();
        
        await _tableClient.UpdateEntityAsync(student, ETag.All, TableUpdateMode.Replace);
        return await _tableClient.GetEntityAsync<Student>("StudentPartition", id.ToString());
    }
}