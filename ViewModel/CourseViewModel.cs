namespace ClassAideApp.ViewModel;

public class CourseViewModel
{
    public int Id { get; set; }
    public String Code { get; set; }
    public String Name { get; set; }

    public CourseViewModel(int id, String code, String name)
    {
        Id = id;
        Code = code;
        Name = name;
    }
}