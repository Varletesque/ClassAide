namespace ClassAideApp.Model;

public record Student(int id, String FirstName, String? MiddleName, String LastName)
{
    public override string ToString() => 
        string.IsNullOrWhiteSpace(MiddleName)
        ? $"{LastName.Trim()}, {FirstName.Trim()}"
        : $"{LastName.Trim()}, {FirstName.Trim()} {LastName.Trim()[0]}";
}