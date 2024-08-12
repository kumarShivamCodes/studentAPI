using studentAPI.Models;

namespace studentAPI.Service;

public interface IStudentService
{
        Task<IEnumerable<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task <Student>UpdateStudentAsync(int id, Student student);
        Task DeleteStudentAsync(int id);
}
